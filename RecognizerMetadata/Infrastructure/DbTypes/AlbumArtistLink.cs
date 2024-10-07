namespace Infrastructure.DbTypes;

public record class AlbumArtistLink
{
    public long AlbumId { get; init; }
    public long ArtistId { get; init; }
}
