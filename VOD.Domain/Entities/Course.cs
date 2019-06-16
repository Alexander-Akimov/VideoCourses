using System.Collections.Generic;

namespace VOD.Domain.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string MarqueeImageUrl { get; set; }

        public int InstructorId { get; set; }
        public Instructor Instructor { get; set; }
        public List<Module> Modules { get; set; }
    }
}