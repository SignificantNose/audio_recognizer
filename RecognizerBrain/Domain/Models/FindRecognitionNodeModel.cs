namespace Domain.Models
{
    public record class FindRecognitionNodeModel
    {
        public uint IdentificationHash { get; init; }
        public int Duration { get; init; } 
    }
}
