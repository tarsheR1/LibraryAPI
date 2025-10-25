using DataAccessLayer.Entities;

namespace DataAccessLayer.Context
{
    public class DataContext
    {
        public List<AuthorEntity> Authors { get; } = new();
        public List<BookEntity> Books { get; } = new();
        
        public DataContext()
        {
            SeedInitialData();
        }

        private void SeedInitialData()
        {
            Guid guid1;
            Guid guid2;
            Authors.Add(new AuthorEntity(guid1 = Guid.NewGuid(), "Лев Толстой", new DateTime(1828, 9, 9)));
            Authors.Add(new AuthorEntity(guid2 = Guid.NewGuid(), "Фёдор Достоевский", new DateTime(1821, 11, 11)));

            Books.Add(new BookEntity(Guid.NewGuid(), "Война и мир", 1869, guid1));
            Books.Add(new BookEntity(Guid.NewGuid(), "Преступление и наказание", 1866, guid2));
        }
    }
}
