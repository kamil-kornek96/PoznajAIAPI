using AutoMapper;
using PoznajAI.Data.Models;
using PoznajAI.Models.Auth;
using PoznajAI.Models.CourseModule;
using PoznajAI.Models.CourseUser;
using PoznajAI.Models.LessonAssignment;
using PoznajAI.Models.LessonComment;
using PoznajAI.Models.LessonRating;
using PoznajAI.Models.User;


namespace PoznajAI.Configuration
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<RegisterRequestDto, UserDto>().ReverseMap();

            #region DTO
            CreateMap<CourseUserDto, CourseUser>().ReverseMap();
            CreateMap<CourseModuleDto, CourseModule>().ReverseMap();
            CreateMap<LessonAssignmentDto, LessonAssignment>().ReverseMap();
            CreateMap<LessonCommentDto, LessonComment>().ReverseMap();
            CreateMap<LessonRatingDto, LessonRating>().ReverseMap();
            #endregion

            #region DTO CREATE
            CreateMap<CourseUserCreateDto, CourseUser>().ReverseMap();
            CreateMap<CourseModuleCreateDto, CourseModule>().ReverseMap();
            CreateMap<LessonAssignmentCreateDto, LessonAssignment>().ReverseMap();
            CreateMap<LessonCommentCreateDto, LessonComment>().ReverseMap();
            CreateMap<LessonRatingCreateDto, LessonRating>().ReverseMap();
            #endregion

            #region DTO UPDATE
            CreateMap<CourseUserUpdateDto, CourseUser>().ReverseMap();
            CreateMap<CourseModuleUpdateDto, CourseModule>().ReverseMap();
            CreateMap<LessonAssignmentUpdateDto, LessonAssignment>().ReverseMap();
            CreateMap<LessonCommentUpdateDto, LessonComment>().ReverseMap();
            CreateMap<LessonRatingUpdateDto, LessonRating>().ReverseMap();
            #endregion

        }
    }
}

