using rentasgt.Domain.Common;
using System.Collections.Generic;

namespace rentasgt.Domain.Entities
{

    /// <summary>
    /// Information about a product category
    /// </summary>
    public class Category : AuditableEntity
    {

        public static readonly int MAX_NAME_LENGTH = 128;
        public static readonly int MAX_DESCRIPTION_LENGTH = 512;

        public Category()
        {
            Products = new List<ProductCategory>();
        }


        public Category(string name, string description, List<ProductCategory> products = null)
            :this()
        {

            Name = name;
            Description = description;
            if (products != null)
            {
                Products.AddRange(products);
            }
        }

        public long Id { get; set; }

        /// <summary>
        /// Name of the category
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description of the category
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// List of products that belong to the category
        /// </summary>
        public List<ProductCategory> Products { get; set; }

    }
}
