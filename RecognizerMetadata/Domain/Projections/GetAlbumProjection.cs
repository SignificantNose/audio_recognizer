namespace Domain.Projections
{
    public record GetAlbumProjection
    {
        public long AlbumId { get; init; }
        public string Title { get; init; }
        public List<ArtistCredits> Artists { get; set; } = new ();  
        public DateOnly ReleaseDate { get; init; }
    }
}

