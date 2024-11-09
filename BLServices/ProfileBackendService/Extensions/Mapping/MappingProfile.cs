using AutoMapper;
using RegistrationPlayerServerResponse = Auth.Backend.Gen.RegistrationPlayerResponse;
using RegistrationPlayerClientRequest = Auth.Database.Gen.RegistrationPlayerRequest;

namespace ProfileBackendService.Extensions.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegistrationPlayerClientRequest, RegistrationPlayerServerResponse>();
    }
}
