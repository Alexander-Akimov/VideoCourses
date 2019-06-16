using System.Collections.Generic;

namespace VOD.Domain.Entities
{
    public class Module
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
        public List<Video> Videos { get; set; }
        public List<Download> Downloads { get; set; }
    }
}