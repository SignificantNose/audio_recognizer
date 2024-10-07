namespace Domain.Projections
{
    public record ArtistCredits
    {
        public long ArtistId { get; init; }
        public string StageName { get; init; }
    }
}