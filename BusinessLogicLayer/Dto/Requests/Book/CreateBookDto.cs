namespace BusinessLogicLayer.Dto.Requests.Book
{
    public sealed record CreateBookDto(string Title, int PublishedYear, Guid AuthorId);
}
