namespace BusinessLogicLayer.Dto.Responses.Author
{
    public sealed record AuthorResponseDto(
      Guid Id,
      string Name,
      DateOnly DateOfBirth,
      int Age,
      int BooksCount);
}
