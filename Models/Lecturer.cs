namespace st10209886_PROG_POE1.Models
{
    public class Lecturer
    {
        public int LecturerId { get; set; } // Primary Key
        public string LecturerNumber { get; set; } // Unique Identifier
        public string Name { get; set; } // Lecturer's Name
        public string Email { get; set; } // Lecturer's Email
        public string Phone { get; set; } // Lecturer's Phone Number

        public ICollection<Claim> Claims { get; set; } = new List<Claim>(); 
    }
}
/////////////////////////////////////////////////END OF FILE/////////////////////////////////////////////////