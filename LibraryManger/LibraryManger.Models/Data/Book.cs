using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryManger.Models.Data
{
    public class Book:BaseModel
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        public bool IsBorrowed { get; set; }
        
        [JsonIgnore]
        public ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();
    }
}
