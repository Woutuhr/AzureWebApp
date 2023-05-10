using System.ComponentModel.DataAnnotations;

namespace AzureWebApp.Models
{
    public class Todo
    {
        public int Id { get; set; }

        [MinLength(1), MaxLength(50), Required]
        public string Name { get; set; }

        [MinLength(1), MaxLength(100), Required]
        public string Description { get; set; }

        public bool Done { get; set; } = false;
    }
}
