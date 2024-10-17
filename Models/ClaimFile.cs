
using System.Collections;

namespace st10209886_PROG_POE1.Models
    {
        public class ClaimFile
        {
            public int Id { get; set; }
            public string FileName { get; set; }

            // Store file data in byte array format
            public byte[] FileData { get; set; }

            // Foreign Key to the Claim
            public int ClaimId { get; set; }
            public Claim Claim { get; set; } // Navigation property to Claim
        }
    }

