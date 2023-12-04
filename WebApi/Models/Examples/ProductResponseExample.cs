using WebApi.API;
using Swashbuckle.AspNetCore.Filters;

namespace WebApi.Models.Examples
{
    public class ProductExample : IExamplesProvider<Product>
    {
        public Product GetExamples()
        {
            return new Product()
            {
                IdProduct = 1,
                NmProduct = "Basic",
                VlMonthPrice = 14.99,
                NmVideoQuality = "Good",
                NmResolution = "480p",
                QtSimultaneousScreens = 1
            };
        }
    }

    public class ArrayProductExample : IExamplesProvider<ArrayProduct>
    {
        public ArrayProduct GetExamples()
        {
            var arrayProduct = new ArrayProduct();
            var productExample = new ProductExample();

            arrayProduct.Products.Add(productExample.GetExamples());
            arrayProduct.Products.Add(productExample.GetExamples());
            arrayProduct.Products.Add(productExample.GetExamples());
            return arrayProduct;
        }
    }

}