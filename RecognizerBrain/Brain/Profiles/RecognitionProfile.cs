using Application.Models;
using AutoMapper;
using Domain.Models;
using GrpcBrain;

namespace Brain.Profiles;

public class RecognitionProfile : Profile
{
    public RecognitionProfile()
    {
        CreateMap<AddRecognitionNodeRequest, AddRecognitionNodeModel>();
        CreateMap<RecognizeTrackRequest, RecognizeTrackModel>();
    }
}
