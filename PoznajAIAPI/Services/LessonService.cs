using AutoMapper;
using PoznajAI.Data.Models;
using PoznajAI.Data.Repositories;
using PoznajAI.Exceptions;
using PoznajAI.Models.Course;
using PoznajAI.Models.Lesson;
using Serilog;

namespace PoznajAI.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IMapper _mapper;

        public LessonService(ILessonRepository lessonRepository, IMapper mapper)
        {
            _lessonRepository = lessonRepository;
            _mapper = mapper;
        }

        public async Task<LessonDetailsDto> GetLessonById(Guid lessonId)
        {
            try
            {
                var lesson = await _lessonRepository.GetLessonById(lessonId);

                if (lesson == null)
                {
                    throw new NotFoundException($"Lesson with ID {lessonId} not found.");
                }

                return _mapper.Map<LessonDetailsDto>(lesson);
            }
            catch (Exception ex)
            {

                Log.Error(ex, "An error occurred while fetching the lesson by ID.");
                throw;
            }
        }

        public async Task<LessonDto> CreateLesson(CreateLessonDto lessonDto)
        {
            try
            {
                var lesson = _mapper.Map<Lesson>(lessonDto);
                var createdLesson = await _lessonRepository.CreateLesson(lesson);

                Log.Information("Lesson created: {@Lesson}", createdLesson);

                return _mapper.Map<LessonDto>(createdLesson);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while creating the lesson.");
                throw;
            }
        }

        public async Task<bool> UpdateLesson(Guid lessonId, UpdateLessonDto lessonDto)
        {
            try
            {
                var existingLesson = await _lessonRepository.GetLessonById(lessonId);

                if (existingLesson == null)
                {
                    return false;
                }

                _mapper.Map(lessonDto, existingLesson);

                await _lessonRepository.UpdateLesson(existingLesson);

                Log.Information("Lesson updated: {@Lesson}", existingLesson);
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while updating the lesson.");
                throw;
            }
        }

        public async Task<bool> DeleteLesson(Guid lessonId)
        {
            try
            {
                var existingLesson = await _lessonRepository.GetLessonById(lessonId);

                if (existingLesson == null)
                {
                    return false;
                }

                var result = await _lessonRepository.DeleteLesson(lessonId);

                Log.Information("Lesson deleted: {@Lesson}", existingLesson);
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while deleting the lesson.");
                throw;
            }
        }
    }
}
