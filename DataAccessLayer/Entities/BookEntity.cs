namespace DataAccessLayer.Entities
{
    public class BookEntity : BaseEntity
    {
        protected BookEntity() { }

        public BookEntity(Guid id, string title, int publishedYear, Guid authorId)
        {
            Id = id;
            Title = title;
            PublishedYear = publishedYear;
            AuthorId = authorId;
        }

        public string Title { get; set; }
        public int PublishedYear { get; set; }
        public Guid AuthorId { get; set; }

        public AuthorEntity Author { get; set; }
    }
}
