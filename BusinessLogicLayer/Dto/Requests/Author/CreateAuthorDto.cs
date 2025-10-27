namespace BusinessLogicLayer.Dto.Requests.Author
{
    public sealed record CreateAuthorDto(string Name, DateOnly DateOfBirth);
}
