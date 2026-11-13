using System.ComponentModel.DataAnnotations;
using System.Data;



namespace FloraSA.Models
{
    public class Flower
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? AiReponse { get; set; }
        public string? location { get; set; }
        public string? flowerType { get; set; }
        public string? careOfFlower { get; set; }
 
        public  IFormFile? UploadImage { get; set; }
      
        public IFormFile? takePicture { get; set; }
        public string? ImageBase64 { get; set; }
        public string? ImageContentType { get; set; }

        [Required]
        public string? Language { get; set; }
    }
}

