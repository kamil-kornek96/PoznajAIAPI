using AutoMapper;
using PoznajAI.Data.Models;
using PoznajAI.Models.Auth;
using PoznajAI.Models.User;
using PoznajAI.Services;

namespace PoznajAI.Configuration
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserDto>().ForMember(dest => dest.IsAdmin, opt => opt.MapFrom<IsAdminResolver>());

            CreateMap<User, UserCreateDto>().ReverseMap();

            CreateMap<RegisterRequestDto, UserDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();

            CreateMap<Lesson, CreateLessonDto>().ReverseMap();
            CreateMap<Lesson, LessonDto>().ReverseMap();

            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<Course, OwnedCourseDto>().ReverseMap();
            CreateMap<Course, CourseCreateDto>().ReverseMap();



        }
    }
}

