namespace HROnboarding.API.Models
{
    public class Candidate
    {
        public int CandidateID { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? status { get; set; }
        public string? JoinDate { get; set; }
        public string? Team { get; set; }
    }
}
