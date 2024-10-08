namespace Domain.Models
{
    public record class FindRecognitionNodeModel
    {
        public long IdentificationHash { get; init; }
        public int Duration { get; init; } 
    }
}
