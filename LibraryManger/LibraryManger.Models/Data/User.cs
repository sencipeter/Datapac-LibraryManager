using System.ComponentModel.DataAnnotations;

namespace LibraryManger.Models.Data
{
    public class User : BaseModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]
        public string Email { get; set; } = string.Empty;

        public ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();
    }
}