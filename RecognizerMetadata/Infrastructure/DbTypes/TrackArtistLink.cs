namespace Infrastructure.DbTypes;

public record class TrackArtistLink
{
    public long TrackId { get; init; }
    public long ArtistId { get; init; }
}
