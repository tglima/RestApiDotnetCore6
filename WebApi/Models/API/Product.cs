using System.ComponentModel;
using System.Text.Json.Serialization;

namespace WebApi.API
{
    [DisplayName("Product")]
    public class Product
    {
        [JsonPropertyName("id_product")]
        public int IdProduct { get; set; }

        [JsonPropertyName("nm_product")]
        public string NmProduct { get; set; } = string.Empty;

        [JsonPropertyName("vl_month_price")]
        public double VlMonthPrice { get; set; }

        [JsonPropertyName("nm_videoQuality")]
        public string NmVideoQuality { get; set; } = string.Empty;

        [JsonPropertyName("nm_resolution")]
        public string NmResolution { get; set; } = string.Empty;

        [JsonPropertyName("qt_simultaneous_screens")]
        public int QtSimultaneousScreens { get; set; }
    }


    [DisplayName("ArrayProduct")]
    public class ArrayProduct
    {
        public ArrayProduct()
        {
            this.Products = new List<Product>();
        }

        public List<Product> Products { get; set; }
    }
}