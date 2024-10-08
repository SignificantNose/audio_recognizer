namespace Domain.Models;

public record AddCoverMetaModel
{
    public string? JpgUri { get; init; }
    public string? PngUri { get; init; }
}
