using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSiteFinal.Models
{
    public class BlogItemsModel
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [DisplayName("Image Name")]
        public string? ImageName { get; set; }
       
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }
    }
}
