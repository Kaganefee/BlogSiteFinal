using System.ComponentModel.DataAnnotations;

namespace BlogSiteFinal.Models
{
    public class Admin
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
