using AutoMapper;
using PoznajAI.Data.Models;
using PoznajAI.Data.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace PoznajAI.Services
{

    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _LessonRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<LessonService> _logger;

        public LessonService(ILessonRepository LessonRepository, IMapper mapper, ILogger<LessonService> logger)
        {
            _LessonRepository = LessonRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<LessonDto>> GetAllLessonsForCourse(Guid courseId)
        {
            var Lessons = await _LessonRepository.GetAllLessonsByCourse(courseId);
            return _mapper.Map<IEnumerable<LessonDto>>(Lessons);
        }


        public async Task CreateLesson(CreateLessonDto lessonDto)
        {
            var lesson = _mapper.Map<Lesson>(lessonDto);
            await _LessonRepository.CreateLesson(lesson);
            _logger.LogInformation("Lesson created: {@Lesson}", lesson);
        }

    }

}
