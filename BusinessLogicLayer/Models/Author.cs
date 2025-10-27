namespace BusinessLogicLayer.Models
{
    public class Author
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
