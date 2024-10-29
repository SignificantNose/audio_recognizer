using System;
using AutoMapper;
using Covers.Profiles;

namespace Tests.TestHelpers;

public static class MappingHelper
{
    public static IMapper GetMapper(){
        MapperConfiguration configuration = new MapperConfiguration(expression => 
        {
            expression.AddProfile<CoversProfile>();
        });
        Mapper mapper = new Mapper(configuration);
        return mapper;
    }
}
