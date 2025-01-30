using System.ComponentModel.DataAnnotations;

namespace LibraryManger.Models.Requests
{
    public class BookReturningRequest
    {
        [Required]
        public int? BookId { get; set; }
    }
}
