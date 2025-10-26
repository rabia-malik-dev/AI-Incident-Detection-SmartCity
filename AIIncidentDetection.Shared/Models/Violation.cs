using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIIncidentDetection.Shared.Models
{
    public class Violation
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;  // e.g. "Helmet Missing"
        public double Confidence { get; set; }            // AI confidence score
        public string FrameImagePath { get; set; } = string.Empty;

        public DateTime DetectedAt { get; set; } = DateTime.UtcNow;

        // relation
        public int VideoId { get; set; }
        public Video? Video { get; set; }
    }
}
