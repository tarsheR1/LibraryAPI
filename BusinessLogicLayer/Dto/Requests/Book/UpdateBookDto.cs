namespace BusinessLogicLayer.Dto.Requests.Book
{
    public sealed record UpdateBookDto(string Title, int PublishedYear, Guid AuthorId);
}
