using AutoMapper;
using RecognizerGateway.Profiles;

namespace Tests.TestHelpers;

public static class MappingHelper
{
    public static IMapper GetMapper(){
    MapperConfiguration configuration = new MapperConfiguration(expression => 
        {
            expression.AddProfile<GatewayProfile>();
        });
        Mapper mapper = new Mapper(configuration);
        return mapper;
    }
}
