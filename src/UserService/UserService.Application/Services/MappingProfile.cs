using AutoMapper;
using UserService.Application.DTOs;
using UserService.Domain.Entities;

namespace UserService.Application.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, AuthResponseDTO>();

            CreateMap<User, UserDTO>();
            CreateMap<CreateUserDTO, User>();

            CreateMap<User, RegistrationResponseDTO>();

            CreateMap<LoginDTO, User>();
        }
    }
}
