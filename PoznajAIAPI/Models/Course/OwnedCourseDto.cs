﻿using PoznajAI.Data.Models;

namespace PoznajAI.Services
{
    public class OwnedCourseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Lesson> Lessons { get; set; }
    }
}