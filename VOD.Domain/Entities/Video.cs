using System;
using System.Collections.Generic;
using System.Text;

namespace VOD.Domain.Entities
{
   public  class Video
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public string Url { get; set; }
        public int Duration { get; set; }

        public int ModuleId { get; set; }
        public Module Module { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
