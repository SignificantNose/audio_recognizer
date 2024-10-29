namespace Domain.Projections
{
    public record GetTrackListProjection
    {
        public long TrackId { get; init; }
        public string Title { get; init;}
        public List<ArtistCredits> Artists { get; set; } = new ();
        public AlbumCredits? Album { get; set; }
    }
}

