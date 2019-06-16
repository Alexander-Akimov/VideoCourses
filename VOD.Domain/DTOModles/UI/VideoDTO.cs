﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VOD.Domain.DTOModles.UI
{
    public class VideoDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string Thumbnail { get; set; }
        public string Url { get; set; }
    }
}
