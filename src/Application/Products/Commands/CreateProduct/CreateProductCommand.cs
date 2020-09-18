using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace rentasgt.Application.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<long>
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public string OtherNames { get; set; }
        public Ubicacion Location { get; set; }
        public List<long> Categories { get; set; }
        public List<long> Pictures { get; set; }
        public decimal CostPerDay { get; set; }
        public decimal? CostPerWeek { get; set; }
        public decimal? CostPerMonth { get; set; }

    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, long>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly ILocation locationService;
        private readonly UserManager<AppUser> userManager;
        private readonly IConfiguration config;

        private readonly string StaticMapsPath;

        public CreateProductCommandHandler(IApplicationDbContext context,
            ICurrentUserService currentUserService,
            ILocation locationService,
            UserManager<AppUser> userManager,
            IConfiguration config,
            IHostingEnvironment environment)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.locationService = locationService;
            this.userManager = userManager;
            this.config = config;
            StaticMapsPath = Path.Combine(environment.WebRootPath, "uploads", "maps");
        }

        public async Task<long> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {

            EnsureCreateUploadedMapsDirectory();
            string mapFileName = $"{request.Name}-{Guid.NewGuid().ToString()}.png";
            WebClient webClient = new WebClient();
            string apiKey = config.GetValue<string>("Maps:ApiKey");
            await webClient.DownloadFileTaskAsync($"https://maps.googleapis.com/maps/api/staticmap?markers=|{request.Location.Latitude},{request.Location.Longitude}&language=es&size=540x216&zoom=16&scale=1&key={apiKey}", Path.Combine(StaticMapsPath, mapFileName));
            request.Location.StaticMap = $"/uploads/maps/{mapFileName}";
            Product newProduct = new Product()
            {
                Name = request.Name,
                Status = ProductStatus.Available,
                Description = request.Description,
                OtherNames = request.OtherNames,
                Location = request.Location,
                CostPerDay = request.CostPerDay,
                CostPerWeek = request.CostPerWeek,
                CostPerMonth = request.CostPerMonth,
            };

            //Address addr = this.locationService.GetAddress(new Geolocation.Coordinate
            //{
            //    Latitude = request.Location.Latitude,
            //    Longitude = request.Location.Longitude
            //});
            //newProduct.Location.City = addr.City;
            //newProduct.Location.State = addr.State;

            if (request.Categories != null)
            {
                AddCategoriesToNewProduct(newProduct, request.Categories);
            }

            await AddUserToProduct(newProduct);
            AddPicturesToProduct(newProduct, request.Pictures);

            await this.context.Products.AddAsync(newProduct);
            await this.context.SaveChangesAsync(cancellationToken);

            return newProduct.Id;
        }

        private void EnsureCreateUploadedMapsDirectory()
        {
            if (!Directory.Exists(StaticMapsPath))
            {
                Directory.CreateDirectory(StaticMapsPath);
            }
        }

        private async Task AddUserToProduct(Product product)
        {
            var user = await this.userManager.FindByIdAsync(this.currentUserService.UserId);
            if (user != null)
            {
                product.Owner = user;
            }
            else
            {
                throw new ValidationException($"No existe el usuario");
            }
        }

        private void AddCategoriesToNewProduct(Product product, List<long> categoriesId)
        {
            foreach (var categoryId in categoriesId)
            {
                var category = this.context.Categories.FirstOrDefault(c => c.Id == categoryId);
                if (category != null)
                {
                    product.Categories.Add(new ProductCategory
                    {
                        Product = product,
                        Category = category
                    });
                }
                else
                {
                    throw new ValidationException($"No existe una categoria con Id={categoryId}");
                }
            }
        }

        private void AddPicturesToProduct(Product product, List<long> picturesId)
        {
            foreach (var pictureId in picturesId)
            {
                var picture = this.context.Pictures.FirstOrDefault(p => p.Id == pictureId);
                if (picture != null)
                {
                    product.Pictures.Add(new ProductPicture
                    {
                        Product = product,
                        Picture = picture
                    });
                }
                else
                {
                    throw new ValidationException($"No existe imagen con Id={pictureId}");
                }
            }
        }

    }

}
