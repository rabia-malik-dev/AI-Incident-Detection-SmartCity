using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIIncidentDetection.Shared.Models
{
    public class Video
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // relation
        public int UserId { get; set; }
        public User? UploadedBy { get; set; }

        // navigation property
        public ICollection<Violation>? Violations { get; set; }
    }
}
