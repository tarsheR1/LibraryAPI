namespace BusinessLogicLayer.Dto.Responses.Book
{
    public sealed record BookResponseDto(
    Guid Id,
    string Title,
    int PublishedYear,
    Guid AuthorId,
    string AuthorName,
    int BookAge);
}
