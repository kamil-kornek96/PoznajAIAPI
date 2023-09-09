using AutoMapper;
using Microsoft.Extensions.Logging;
using PoznajAI.Data.Models;
using PoznajAI.Data.Repositories;
using PoznajAI.Exceptions;
using System;
using System.Threading.Tasks;

namespace PoznajAI.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<LessonService> _logger;

        public LessonService(ILessonRepository lessonRepository, IMapper mapper, ILogger<LessonService> logger)
        {
            _lessonRepository = lessonRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LessonDetailsDto> GetLessonById(Guid lessonId)
        {
            var lesson = await _lessonRepository.GetLessonById(lessonId);

            if (lesson == null)
            {
                throw new NotFoundException($"Lesson with ID {lessonId} not found.");
            }

            return _mapper.Map<LessonDetailsDto>(lesson);
        }

        public async Task<Guid> CreateLesson(CreateLessonDto lessonDto)
        {
            var lesson = _mapper.Map<Lesson>(lessonDto);
            var createdLesson = await _lessonRepository.CreateLesson(lesson);

            _logger.LogInformation("Lesson created: {@Lesson}", createdLesson);

            return createdLesson.Id;
        }

        public async Task UpdateLesson(UpdateLessonDto lessonDto)
        {
            var lesson = await _lessonRepository.GetLessonById(lessonDto.Id);

            if (lesson == null)
            {
                // Obsługa błędu: Lekcja o podanym ID nie została znaleziona.
                throw new NotFoundException($"Lesson with ID {lessonDto.Id} not found.");
            }

            _mapper.Map(lessonDto, lesson);

            await _lessonRepository.UpdateLesson(lesson);

            _logger.LogInformation("Lesson updated: {@Lesson}", lesson);
        }

        public async Task<bool> DeleteLesson(Guid lessonId)
        {
            var lesson = await _lessonRepository.GetLessonById(lessonId);

            if (lesson == null)
            {
                // Obsługa błędu: Lekcja o podanym ID nie została znaleziona.
                throw new NotFoundException($"Lesson with ID {lessonId} not found.");
            }

            return await _lessonRepository.DeleteLesson(lessonId);
        }
    }
}
