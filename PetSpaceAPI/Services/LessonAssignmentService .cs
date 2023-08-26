using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using PoznajAI.Data.Models;
using PoznajAI.Data.Repositories.Interfaces;
using PoznajAI.Models.LessonAssignment;

public class LessonAssignmentService : ILessonAssignmentService
{
    private readonly ILessonAssignmentRepository _assignmentRepository;
    private readonly IMapper _mapper;

    public LessonAssignmentService(ILessonAssignmentRepository assignmentRepository, IMapper mapper)
    {
        _assignmentRepository = assignmentRepository;
        _mapper = mapper;
    }

    public async Task<LessonAssignmentDto> GetAssignmentById(int assignmentId)
    {
        var assignment = await _assignmentRepository.GetAssignmentById(assignmentId);
        return _mapper.Map<LessonAssignmentDto>(assignment);
    }

    public async Task<List<LessonAssignmentDto>> GetAllAssignmentsForLesson(int lessonId)
    {
        var assignments = await _assignmentRepository.GetAllAssignmentsForLesson(lessonId);
        return _mapper.Map<List<LessonAssignmentDto>>(assignments);
    }

    public async Task<LessonAssignmentDto> AddAssignment(LessonAssignmentCreateDto assignment)
    {
        var assignmentToAdd = _mapper.Map<LessonAssignment>(assignment);
        return _mapper.Map<LessonAssignmentDto>(await _assignmentRepository.AddAssignment(assignmentToAdd));
    }

    public async Task<bool> UpdateAssignment(LessonAssignmentUpdateDto assignment)
    {
        var assignmentToUpdate = _mapper.Map<LessonAssignment>(assignment);
        return await _assignmentRepository.UpdateAssignment(assignmentToUpdate);
    }

    public async Task DeleteAssignment(int assignmentId)
    {
        await _assignmentRepository.DeleteAssignment(assignmentId);
    }
}
