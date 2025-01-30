namespace LibraryManger.Models.Data
{
    public class Borrowing:BaseModel
    {
        public int UserId { get; set; }
        public User? User { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public DateTime BorrowedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
    }
}
