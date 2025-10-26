using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIIncidentDetection.Shared.Models
{
    public class User
    {
        public int Id { get; set; }

        // basic identity fields
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // hashed password (later we’ll integrate Identity/JWT)
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
