namespace DataAccessLayer.Entities
{
    public class AuthorEntity : BaseEntity
    {
       protected AuthorEntity() { }

       public AuthorEntity(Guid id, string name, DateTime dateOfBirth)
       {
           Id = id;
           Name = name;
           DateOfBirth = dateOfBirth;
           Books = new List<BookEntity>(); 
       }

       public string Name { get; set; }
       public DateTime DateOfBirth { get; set; }

       public ICollection <BookEntity> Books { get; set; }
    }
}
