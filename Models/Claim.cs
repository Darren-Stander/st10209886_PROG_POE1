namespace st10209886_PROG_POE1.Models
{
    public class Claim
    {
        public int ClaimId { get; set; }
        public string LecturerNumber { get; set; }
        public double HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public string Status { get; set; } // "Pending", "Approved", "Rejected"

        public string? AdditionalNotes { get; set; } // Alphanumeric field for notes

        // Navigation property for related ClaimFiles
        public ICollection<ClaimFile> ClaimFiles { get; set; } = new List<ClaimFile>(); // Ensure initialization
    }
}
