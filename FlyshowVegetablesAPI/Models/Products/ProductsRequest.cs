using Infrastructure.Entities;

namespace FlyshowVegetablesAPI.Models.Request.Products
{
    public class ProductsRequest
    {
        /// <summary>
        /// start index
        /// </summary>
        public int StartItem { get; set; }

        /// <summary>
        /// which count want to get
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Order { get; set; }

        /// <summary>
        /// condition with product
        /// </summary>
        public Product ProductCondition { get; set; }

        public ProductsRequest()
        {
            ProductCondition = new Product();
        }
    }
}
