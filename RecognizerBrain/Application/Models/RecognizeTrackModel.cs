namespace Application.Models;

public record RecognizeTrackModel
{
    public string Fingerprint { get; init; }
    public int Duration { get; init; }
}
