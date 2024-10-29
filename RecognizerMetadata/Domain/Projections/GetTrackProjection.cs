namespace Domain.Projections
{
    public record GetTrackProjection
    {
        public long TrackId { get; init; }
        public string Title { get; init; }
        public List<ArtistCredits> Artists { get; set; } = new ();
        public DateOnly ReleaseDate { get; init; }
        public AlbumCredits? Album { get; set; }
        public long? CoverArtId { get; init; }
    }
}

