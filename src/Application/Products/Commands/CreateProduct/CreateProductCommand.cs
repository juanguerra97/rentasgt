using MediatR;
using Microsoft.AspNetCore.Identity;
using rentasgt.Application.Common.Interfaces;
using rentasgt.Application.Common.Models;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        public List<RentCostCommand> Costs { get; set; }

    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, long>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly ILocation locationService;
        private readonly UserManager<AppUser> userManager;

        public CreateProductCommandHandler(IApplicationDbContext context, 
            ICurrentUserService currentUserService,
            ILocation locationService,
            UserManager<AppUser> userManager)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.locationService = locationService;
            this.userManager = userManager;
        }

        public async Task<long> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {

            Product newProduct = new Product()
            {
                Name = request.Name,
                Status = ProductStatus.Available,
                Description = request.Description,
                OtherNames = request.OtherNames,
                Location = request.Location
            };

            Address addr = this.locationService.GetAddress(new Geolocation.Coordinate
            {
                Latitude = request.Location.Latitude,
                Longitude = request.Location.Longitude
            });
            newProduct.Location.City = addr.City;
            newProduct.Location.State = addr.State;

            if (request.Categories != null)
            {
                AddCategoriesToNewProduct(newProduct, request.Categories);
            }

            await AddUserToProduct(newProduct);
            AddPicturesToProduct(newProduct, request.Pictures);
            AddCostsToProduct(newProduct, request.Costs);

            await this.context.Products.AddAsync(newProduct);
            await this.context.SaveChangesAsync(cancellationToken);

            return newProduct.Id;
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

        private void AddCostsToProduct(Product product, List<RentCostCommand> rentCosts)
        {
            foreach (var cost in rentCosts)
            {
                product.Costs.Add(new RentCost
                {
                    Duration = (RentDuration) cost.Duration,
                    Cost = cost.Cost,
                    Product = product
                });
            }
        }
    }

}
