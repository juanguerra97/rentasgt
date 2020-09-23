using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest
    {

        public long Id { get; set; }
        public ProductStatus? Status { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? OtherNames { get; set; }
        public decimal? CostPerDay { get; set; }
        public decimal? CostPerWeek { get; set; }
        public decimal? CostPerMonth { get; set; }
        public Ubicacion? Location { get; set; }
        public List<long> CategoriesToRemove { get; set; }
        public List<long> NewCategories { get; set; }
        public List<long> PicturesToRemove { get; set; }
        public List<long> NewPictures { get; set; }

    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {

        private readonly IApplicationDbContext context;
        private readonly IConfiguration config;
        private readonly string StaticMapsPath;

        public UpdateProductCommandHandler(IApplicationDbContext context,
            IConfiguration config,
            IHostingEnvironment environment)
        {
            this.context = context;
            this.config = config;
            StaticMapsPath = Path.Combine(environment.WebRootPath, "uploads", "maps");
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {

            var entity = await this.context.Products
                .Include(p => p.Pictures)
                .Include(p => p.Categories)
                .Where(p => p.Id == request.Id)
                .SingleOrDefaultAsync();

            if (entity == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }

            bool fieldUpdated = false;

            if (request.Status != null)
            {
                UpdateProductStatus(entity, (ProductStatus) request.Status);
                fieldUpdated = true;
            }

            if (request.Name != null)
            {
                entity.Name = request.Name;
                fieldUpdated = true;
            }

            if (request.Description != null)
            {
                entity.Description = request.Description;
                fieldUpdated = true;
            }

            if (request.OtherNames != null)
            {
                entity.OtherNames = request.OtherNames;
                fieldUpdated = true;
            }

            if (request.CostPerDay != null)
            {
                entity.CostPerDay = (decimal)request.CostPerDay;
                fieldUpdated = true;
            }

            if (entity.CostPerWeek != request.CostPerWeek)
            {
                entity.CostPerWeek = request.CostPerWeek;
                fieldUpdated = true;
            }

            if (entity.CostPerMonth != request.CostPerMonth)
            {
                entity.CostPerMonth = request.CostPerMonth;
                fieldUpdated = true;
            }

            if(request.Location != null)
            {
                EnsureCreateUploadedMapsDirectory();
                string mapFileName = $"{request.Name}-{Guid.NewGuid().ToString()}.png";
                WebClient webClient = new WebClient();
                string apiKey = config.GetValue<string>("GoogleMapsApiKey");
                await webClient.DownloadFileTaskAsync($"https://maps.googleapis.com/maps/api/staticmap?markers=|{request.Location.Latitude},{request.Location.Longitude}&language=es&size=540x216&zoom=16&scale=1&key={apiKey}", Path.Combine(StaticMapsPath, mapFileName));
                
                request.Location.StaticMap = $"/uploads/maps/{mapFileName}";
                
                entity.Location = request.Location;
                fieldUpdated = true;
            }

            await UpdateProductCategories(entity, request.CategoriesToRemove, request.NewCategories);
            await UpdateProductPictures(entity, request.PicturesToRemove, request.NewPictures);
            
            await this.context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private void UpdateProductStatus(Product product, ProductStatus newProductStatus)
        {
            if (newProductStatus == ProductStatus.Available || newProductStatus == ProductStatus.NotAvailable)
            {
                product.Status = newProductStatus;
            }
            else
            {
                throw new Exception("El nuevo estado es invalido");
            }
        }

        private async Task UpdateProductCategories(Product product, List<long> catsToDelete, List<long> catsToAdd)
        {
            var catsCount = product.Categories.Count - catsToDelete.Count + catsToAdd.Count;
            foreach(var catId in catsToDelete)
            {
                var prodCatEntity = await this.context.ProductCategories
                        .FirstOrDefaultAsync(pc => pc.CategoryId == catId
                            && pc.ProductId == product.Id);
                if (prodCatEntity != null)
                {
                    this.context.ProductCategories
                    .Remove(prodCatEntity);
                } else
                {
                    throw new NotFoundException(nameof(ProductCategory), $"{product.Id}, {catId}");
                }
            }
            foreach(var catId in catsToAdd)
            {
                var prodCatEntity = await this.context.ProductCategories
                        .FirstOrDefaultAsync(pc => pc.CategoryId == catId
                            && pc.ProductId == product.Id);
                
                if (prodCatEntity == null)
                {
                    var catEntity = await this.context.Categories
                    .FirstOrDefaultAsync(cat => cat.Id == catId);
                    if (catEntity != null)
                    {
                        await this.context.ProductCategories
                        .AddAsync(new ProductCategory
                        {
                            Product = product,
                            Category = catEntity
                        });
                    } else
                    {
                        throw new NotFoundException(nameof(Category), catId);
                    }
                    
                } else
                {
                    throw new DuplicateDataException($"ProductoCategoria duplicado");
                }
            }
        }

        private async Task UpdateProductPictures(Product product, List<long> picturesToDelete, List<long> picturesToAdd)
        {
            var picsCount = product.Pictures.Count - picturesToDelete.Count + picturesToAdd.Count;
            if (picsCount > 0 && picsCount <= 3)
            {
                foreach (var picId in picturesToDelete)
                {
                    var prodPicEntity = await this.context.ProductPictures
                        .Include(pc => pc.Picture)
                        .FirstOrDefaultAsync(pc => pc.ProductId == product.Id && pc.PictureId == picId);
                    if (prodPicEntity != null)
                    {
                        var pic = prodPicEntity.Picture;
                        this.context.ProductPictures.Remove(prodPicEntity);
                        this.context.Pictures.Remove(pic);
                    } else
                    {
                        throw new NotFoundException(nameof(ProductPicture), $"{product.Id}, {picId}");
                    }
                }
                foreach (var picId in picturesToAdd)
                {
                    var picEntity = await this.context.Pictures
                        .FirstOrDefaultAsync(p => p.Id == picId);
                    if (picEntity != null)
                    {
                        var prodPicEntity = await this.context.ProductPictures
                            .FirstOrDefaultAsync(pp => pp.PictureId == picId);
                        if (prodPicEntity == null)
                        {
                            await this.context.ProductPictures
                                .AddAsync(new ProductPicture
                                {
                                    Picture = picEntity,
                                    Product = product
                                });
                        } else
                        {
                            throw new DuplicateDataException($"ProductoImagen duplicado");
                        }
                    } else
                    {
                        throw new NotFoundException(nameof(Picture), picId);
                    }
                }
            }
        }

        private void EnsureCreateUploadedMapsDirectory()
        {
            if (!Directory.Exists(StaticMapsPath))
            {
                Directory.CreateDirectory(StaticMapsPath);
            }
        }

    }

}
