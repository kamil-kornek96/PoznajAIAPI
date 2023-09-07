using AutoMapper;
using PoznajAI.Data.Models;
using PoznajAI.Data.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace PoznajAI.Services
{

    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _CourseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CourseService> _logger;

        public CourseService(ICourseRepository CourseRepository, IMapper mapper, ILogger<CourseService> logger)
        {
            _CourseRepository = CourseRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserCoursesResponseDto> GetAllCoursesForUser(Guid userId)
        {
            try
            {
                var ownedCourses = await _CourseRepository.GetAllCoursesForUser(userId);
                var allCourses = await _CourseRepository.GetAllCourses();

                var availableCourses = allCourses.Where(c => !ownedCourses.Select(b => b.Id).Contains(c.Id)).ToList();

                var responseDto = new UserCoursesResponseDto
                {
                    AllCourses = _mapper.Map<List<CourseDto>>(availableCourses),
                    OwnedCourses = _mapper.Map<List<OwnedCourseDto>>(ownedCourses)
                };

                return responseDto;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Wystąpił błąd podczas pobierania kursów dla użytkownika.");
                throw;
            }
        }



        public async Task CreateCourse(CourseCreateDto CourseDto)
        {
            var Course = _mapper.Map<Course>(CourseDto);
            await _CourseRepository.CreateCourse(Course);
            _logger.LogInformation("Course created: {@Course}", Course);
        }

    }

}
