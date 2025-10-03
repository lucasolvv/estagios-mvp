namespace PlataformaEstagios.Communication.Requests
{
    public class RequestCreateScheduleInterviewJson
    {
        public DateTimeOffset StartAt { get; set; }
        public int DurationMinutes { get; set; }
        public string? Location { get; set; }
        public string? MeetingLink { get; set; }
        public string? Notes { get; set; }
    }
}
