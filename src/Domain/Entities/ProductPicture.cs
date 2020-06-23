
namespace rentasgt.Domain.Entities
{

    /// <summary>
    /// Class stablish a relationship between a product and an picture of the product
    /// </summary>
    public class ProductPicture
    {

        public ProductPicture()
        { }

        public ProductPicture(Product product, Picture productPicture)
        {
            Product = product;
            Picture = productPicture;
        }

        public long ProductId { get; set; }
        public Product Product { get; set; }

        public long PictureId { get; set; }
        public Picture Picture { get; set; }

    }
}
