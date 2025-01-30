using System.ComponentModel.DataAnnotations;

namespace LibraryManger.Models.Data
{
    public class Book:BaseModel
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        public bool IsBorrowed { get; set; }
        
        public ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();
    }
}
