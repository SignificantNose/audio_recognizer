namespace Domain.Projections
{
    public record AlbumCredits
    {
        public long AlbumId { get; init; }
        public string Title { get; init; }
    }
}

