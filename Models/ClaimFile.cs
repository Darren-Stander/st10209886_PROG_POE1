namespace st10209886_PROG_POE1.Models
{
    public class ClaimFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        // Foreign Key to the Claim
        public int ClaimId { get; set; }
        public Claim Claim { get; set; } // Navigation property to Claim
    }
}
