namespace LibraryManger.Models.Data
{
    public class BaseModel
    {
        public int Id { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Created { get; set; }
        public bool Active { get; set; }
    }
}
