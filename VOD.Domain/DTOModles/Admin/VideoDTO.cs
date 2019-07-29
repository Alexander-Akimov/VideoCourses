using System.ComponentModel.DataAnnotations;

namespace VOD.Domain.DTOModles.Admin
{
    public class VideoDTO
    {
        public int Id { get; set; }
        [MaxLength(80), Required]
        public string Title { get; set; }
        [MaxLength(1024)]
        public string Description { get; set; }
        [MaxLength(1024)]
        public string Thumbnail { get; set; }
        [MaxLength(1024)]
        public string Url { get; set; }
        public int Duration { get; set; }

        public int ModuleId { get; set; }
        public string Module { get; set; }
        public int CourseId { get; set; }
        public string Course { get; set; }

        public ButtonDTO ButtonDTO { get { return new ButtonDTO(courseId: CourseId, moduleId: ModuleId, id: Id); } }
    }
}