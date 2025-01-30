using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryManger.Models.Data
{
    public class User : BaseModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]
        public string Email { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();
    }
}