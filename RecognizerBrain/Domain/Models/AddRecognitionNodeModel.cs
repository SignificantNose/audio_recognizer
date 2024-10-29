
namespace Domain.Models
{
    public record AddRecognitionNodeModel
    {
        public long TrackId { get; init; }
        public uint IdentificationHash { get; init; }
        public int Duration { get; init; }
    }
}