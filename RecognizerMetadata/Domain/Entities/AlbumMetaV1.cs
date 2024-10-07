namespace Domain.Entities
{
    public record AlbumMetaV1
    {
        public long AlbumId { get; init; }
        public string Title { get; init; }        
        public IEnumerable<long> ArtistIds { get; init; }
        public DateOnly ReleaseDate { get; init; }
    }
}
