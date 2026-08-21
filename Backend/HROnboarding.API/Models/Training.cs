namespace HROnboarding.API.Models
{
    public class Training
    {
        public int CandidateID { get; set; }
        public string? Datadog { get; set; }
        public string? AKSTrained { get; set; }
        public string? ROVO {  get; set; }

        public string? AIFluency { get; set; }
        public string? Claude101 { get; set; }
        public string? OtherCertification { get; set; }
        public string? CoPilotTraining { get; set; }

    }
}
