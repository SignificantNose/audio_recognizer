using System;
using AutoMapper;
using Brain.Profiles;

namespace Tests.TestHelpers;

public static class MapperHelper
{
    public static IMapper GetMapper(){
        var configuration = new MapperConfiguration(expression =>
        {
            expression.AddProfile<RecognitionProfile>();
        });
        var mapper = new Mapper(configuration);
        return mapper;
    }
}
