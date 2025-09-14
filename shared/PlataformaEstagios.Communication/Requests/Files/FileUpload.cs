namespace PlataformaEstagios.Communication.Requests.Files
{
    public sealed class FileUpload
    {
        public required Stream Content { get; init; }
        public required string FileName { get; init; }
        public required string ContentType { get; init; }
        public long Length { get; init; }
    }
}
