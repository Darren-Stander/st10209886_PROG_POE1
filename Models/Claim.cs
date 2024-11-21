namespace st10209886_PROG_POE1.Models
{
    public class Claim
    {
        public int ClaimId { get; set; } // Primary Key
        public string LecturerNumber { get; set; } // Foreign Key to Lecturer
        public Lecturer Lecturer { get; set; } // Navigation property

        public double HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public string Status { get; set; } // "Pending", "Approved", "Rejected"
        public string? AdditionalNotes { get; set; }

        public ICollection<ClaimFile> ClaimFiles { get; set; } = new List<ClaimFile>();
    }
}

/////////////////////////////////////////////////END OF FILE/////////////////////////////////////////////////