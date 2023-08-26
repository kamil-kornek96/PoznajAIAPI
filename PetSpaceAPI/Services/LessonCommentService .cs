using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using PoznajAI.Data.Models;
using PoznajAI.Data.Repositories.Interfaces;
using PoznajAI.Models.LessonComment;

public class LessonCommentService : ILessonCommentService
{
    private readonly ILessonCommentRepository _commentRepository;
    private readonly IMapper _mapper;

    public LessonCommentService(ILessonCommentRepository commentRepository, IMapper mapper)
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
    }

    public async Task<LessonCommentDto> GetCommentById(int commentId)
    {
        var comment = await _commentRepository.GetCommentById(commentId);
        return _mapper.Map<LessonCommentDto>(comment);
    }

    public async Task<List<LessonCommentDto>> GetCommentsForLesson(int lessonId)
    {
        var comments = await _commentRepository.GetCommentsForLesson(lessonId);
        return _mapper.Map<List<LessonCommentDto>>(comments);
    }

    public async Task<LessonCommentDto> AddComment(LessonCommentCreateDto comment)
    {
        var commentToAdd = _mapper.Map<LessonComment>(comment);
        return _mapper.Map<LessonCommentDto>(await _commentRepository.AddComment(commentToAdd));
    }

    public async Task<bool> UpdateComment(LessonCommentUpdateDto comment)
    {
        var commentToUpdate = _mapper.Map<LessonComment>(comment);
        return await _commentRepository.UpdateComment(commentToUpdate);
    }

    public async Task DeleteComment(int commentId)
    {
        await _commentRepository.DeleteComment(commentId);
    }
}
