namespace PlataformaEstagios.Communication.Responses
{
    public class ResponseGetInterviewItemJson
    {
        public Guid ApplicationIdentifier { get; set; }
        public Guid InterviewIdentifier { get; set; }
        public Guid CandidateIdentifier { get; set; }
        public Guid EnterpriseIdentifier { get; set; }
        public DateTimeOffset StartAt { get; set; }
        public int DurationMinutes { get; set; }
        public string? Location { get; set; }
        public string? MeetingLink { get; set; }
        public string? Notes { get; set; }

    }
}
