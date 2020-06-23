using rentasgt.Domain.Common;
using rentasgt.Domain.Enums;
using System.Collections.Generic;

namespace rentasgt.Domain.Entities
{

    /// <summary>
    /// Class for representing info about products
    /// </summary>
    public class Product : AuditableEntity
    {

        public Product()
        {
            Pictures = new List<ProductPicture>();
            Costs = new List<RentCost>();
            Categories = new List<ProductCategory>();
            RentRequests = new List<RentRequest>();
        }

        public Product(ProductStatus status, string name, string otherNames,
            string description, AppUser owner, Ubicacion location, 
            List<ProductPicture> pictures, List<RentCost> costs = null, 
            List<ProductCategory> categories = null, 
            List<RentRequest> rentRequests = null)
            : this()
        {
            Status = status;
            Name = name;
            OtherNames = otherNames;
            Description = description;
            Owner = owner;
            Location = location;
            
            if (pictures != null)
            {
                Pictures.AddRange(pictures);
            }

            if (costs != null)
            {
                Costs.AddRange(costs);
            }

            if (categories != null)
            {
                Categories.AddRange(categories);
            }

            if (rentRequests != null)
            {
                RentRequests.AddRange(rentRequests);
            }
        }

        public Product(ProductStatus status, string name, string otherNames,
            string description, AppUser owner, Ubicacion location, 
            List<ProductPicture> pictures, List<RentCost> costs, 
            List<Category> categories)
            : this(status, name, otherNames, description, owner, location, pictures, 
                  costs, new List<ProductCategory>())
        {

            if (categories != null)
            {
                foreach(var category in categories)
                {
                    var catArt = new ProductCategory(category, this);
                    Categories.Add(catArt);
                }
            }
        }

        public long Id { get; set; }

        /// <summary>
        /// Status of the product
        /// </summary>
        public ProductStatus Status { get; set; }

        /// <summary>
        /// Name of the product
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Product's description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Other names this product is called separated by commas
        /// </summary>
        public string OtherNames { get; set; }

        /// <summary>
        /// User who owns the product
        /// </summary>
        public AppUser Owner { get; set; }

        /// <summary>
        /// Object that contains the location informacion of the product
        /// </summary>
        public Ubicacion Location { get; set; }

        /// <summary>
        /// List of pictures of the product
        /// </summary>
        public List<ProductPicture> Pictures { get; set; }

        /// <summary>
        /// Lists of costs of renting the product
        /// </summary>
        public List<RentCost> Costs { get; set; }

        /// <summary>
        /// List of the categories the product belongs to
        /// </summary>
        public List<ProductCategory> Categories { get; private set; }

        /// <summary>
        /// List with all rent request the product has received
        /// </summary>
        public List<RentRequest> RentRequests { get; set; }

    }
}