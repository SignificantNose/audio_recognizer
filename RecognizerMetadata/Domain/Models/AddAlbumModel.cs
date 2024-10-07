namespace Domain.Models;

public record class AddAlbumModel
{
    public string Title { get; init; }
    public IEnumerable<long> ArtistIds { get; init; }
    public DateOnly ReleaseDate { get; init; }  
}
