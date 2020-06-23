
namespace rentasgt.Domain.Entities
{

    /// <summary>
    /// Class that represents a join table to relate products and categories
    /// </summary>
    public class ProductCategory
    {

        public ProductCategory()
        { }

        public ProductCategory(Category category, Product product)
        {
            Category = category;
            Product = product;
        }

        public ProductCategory(Product product, Category category)
            : this(category, product)
        { }

        public long CategoryId { get; set; }
        public Category Category { get; set; }

        public long ProductId { get; set; }
        public Product Product { get; set; }

    }
}
