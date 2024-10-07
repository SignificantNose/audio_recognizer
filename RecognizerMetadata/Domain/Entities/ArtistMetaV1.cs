namespace Domain.Entities
{
    public record ArtistMetaV1
    {
        public long ArtistId { get; init; }
        public string StageName { get; init; }
        public string RealName { get; init; }    
    }
}
