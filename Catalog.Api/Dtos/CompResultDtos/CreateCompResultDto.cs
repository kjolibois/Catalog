using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Catalog.Api.Dtos
{
    public class CreateCompResultDto
    {
        [JsonPropertyName("age: ")]
        public string Age { get; set; }

        [JsonPropertyName("baker_name: ")]
        public string BakerName { get; set; }

        [JsonPropertyName("hometown: ")]
        public string Hometown { get; set; }

        [JsonPropertyName("occupation: ")]
        public string Occupation { get; set; }
    }

    public class CreateResultsBySeason
    {
        public List<CreateCompResultDto> CompResults { get; set; }
    }

}