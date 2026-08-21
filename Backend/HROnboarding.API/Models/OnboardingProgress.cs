namespace HROnboarding.API.Models
{
    public class OnboardingProgress
    {
        public int ProgressID { get; set; }
        public int CandidateID { get; set; }
        public int StepID { get; set; }
        public string? CompletedDate { get; set; }
        public string? Status { get; set; }
    }
}
