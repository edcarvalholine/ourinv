using Newtonsoft.Json;
using ourinv.WebAPI.DTOs.ProductDTO;

namespace ourinv.WebAPI.DTOs.CategoryDTO
{
    public class BaseCategoryDTO
    {
        public string Name { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<BaseProductDTO> Products { get; set; }
    }
}