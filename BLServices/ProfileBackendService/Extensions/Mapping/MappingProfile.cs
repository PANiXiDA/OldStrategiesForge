using AutoMapper;
using RegistrationPlayerServerResponse = Auth.Backend.Gen.RegistrationPlayerResponse;
using RegistrationPlayerClientRequest = Auth.Database.Gen.RegistrationPlayerRequest;
using LoginPlayerServerResponse = Auth.Backend.Gen.LoginPlayerResponse;
using LoginPlayerClientResponse = Auth.Database.Gen.LoginPlayerResponse;

namespace ProfileBackendService.Extensions.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegistrationPlayerClientRequest, RegistrationPlayerServerResponse>();
        CreateMap<LoginPlayerClientResponse, LoginPlayerServerResponse>();
    }
}
