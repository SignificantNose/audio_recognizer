using System;
using AutoMapper;
using Metadata.Profiles;

namespace Tests.TestHelpers;

public static class MappingHelper
{
    public static IMapper GetMapper(){
        MapperConfiguration configuration = new MapperConfiguration(expression => 
        {
            expression.AddProfile<MetadataProfile>();
        });
        Mapper mapper = new Mapper(configuration);
        return mapper;
    }
}
