﻿using rentasgt.Domain.Common;
using rentasgt.Domain.Enums;
using System.Collections.Generic;

namespace rentasgt.Domain.Entities
{

    /// <summary>
    /// Class for representing info about products
    /// </summary>
    public class Product : AuditableEntity
    {

        public static readonly int MAX_NAME_LENGTH = 128;
        public static readonly int MAX_OTHERNAMES_LENGTH = 256;
        public static readonly int MAX_DESCRIPTION_LENGTH = 512;

        public Product()
        {
            Pictures = new List<ProductPicture>();
            Categories = new List<ProductCategory>();
            RentRequests = new List<RentRequest>();
        }

        public Product(ProductStatus status, string name, string otherNames,
            string description, AppUser owner, Ubicacion location, 
            List<ProductPicture> pictures,
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
                  new List<ProductCategory>())
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
        
        public decimal CostPerDay { get; set; }
        
        public decimal? CostPerWeek { get; set; }

        public decimal? CostPerMonth { get; set; }

        public double? Rating { get; set; }

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