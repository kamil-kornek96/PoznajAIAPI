﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using PoznajAI.Data.Models;
using PoznajAI.Data.Repositories;
using PoznajAI.Exceptions;
using System;
using System.Collections.Generic;
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
                _logger.LogError(ex, "An error occurred while fetching the lesson by ID.");
                throw;
            }
        }

        public async Task<Guid> CreateLesson(CreateLessonDto lessonDto)
        {
            try
            {
                var lesson = _mapper.Map<Lesson>(lessonDto);
                var createdLesson = await _lessonRepository.CreateLesson(lesson);

                _logger.LogInformation("Lesson created: {@Lesson}", createdLesson);

                return createdLesson.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the lesson.");
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

                _logger.LogInformation("Lesson updated: {@Lesson}", existingLesson);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the lesson.");
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

                _logger.LogInformation("Lesson deleted: {@Lesson}", existingLesson);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the lesson.");
                throw;
            }
        }
    }
}