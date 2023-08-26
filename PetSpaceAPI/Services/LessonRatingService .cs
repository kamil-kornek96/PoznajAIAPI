using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using PoznajAI.Data.Models;
using PoznajAI.Data.Repositories.Interfaces;
using PoznajAI.Models.LessonRating;

public class LessonRatingService : ILessonRatingService
{
    private readonly ILessonRatingRepository _ratingRepository;
    private readonly IMapper _mapper;

    public LessonRatingService(ILessonRatingRepository ratingRepository, IMapper mapper)
    {
        _ratingRepository = ratingRepository;
        _mapper = mapper;
    }

    public async Task<LessonRatingDto> GetRatingById(int ratingId)
    {
        var rating = await _ratingRepository.GetRatingById(ratingId);
        return _mapper.Map<LessonRatingDto>(rating);
    }

    public async Task<List<LessonRatingDto>> GetAllRatingsForLesson(int lessonId)
    {
        var ratings = await _ratingRepository.GetAllRatingsForLesson(lessonId);
        return _mapper.Map<List<LessonRatingDto>>(ratings);
    }

    public async Task<LessonRatingDto> AddRating(LessonRatingCreateDto rating)
    {
        var ratingToAdd = _mapper.Map<LessonRating>(rating);
        return _mapper.Map<LessonRatingDto>(await _ratingRepository.AddRating(ratingToAdd));
    }

    public async Task<bool> UpdateRating(LessonRatingUpdateDto rating)
    {
        var ratingToUpdate = _mapper.Map<LessonRating>(rating);
        return await _ratingRepository.UpdateRating(ratingToUpdate);
    }

    public async Task DeleteRating(int ratingId)
    {
        await _ratingRepository.DeleteRating(ratingId);
    }
}
