namespace Domain.Entities
{
    public record TrackMetaV1
    {
        public long TrackId { get; init; }
        public string Title { get; init; }
        public IEnumerable<long> ArtistIds { get; init; }
        public DateOnly ReleaseDate { get; init; }
        public long AlbumId { get; init; }
        public long CoverArtId { get; init; }
    }
}
