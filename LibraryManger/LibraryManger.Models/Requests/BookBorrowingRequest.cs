using System.ComponentModel.DataAnnotations;

namespace LibraryManger.Models.Requests
{
    public class BookBorrowingRequest
    {
        [Required]
        public int? BookId { get; set; }

        [Required]
        public int? UserId { get; set; }
    }
}
