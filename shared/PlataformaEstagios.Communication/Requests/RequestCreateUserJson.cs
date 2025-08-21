using PlataformaEstagios.Domain.Enums;
namespace PlataformaEstagios.Communication.Requests
{ 
    public class RequestCreateUserJson
    {
        public string Nickname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public UserType UserType { get; set; }

        public RequestCandidateJson? Candidate { get; set; }
        public RequestEnterpriseJson? Enterprise { get; set; }
    }

    public class RequestCandidateJson
    {
        public string FullName { get; set; } = null!;
        public DateOnly? BirthDate { get; set; }
        public string? CourseName { get; set; }
        public RequestAddressJson? Address { get; set; }
    }

    public class RequestEnterpriseJson
    {
        public string? TradeName { get; set; }
        public string? Cnpj { get; set; }
        public string? ActivityArea { get; set; }
        public RequestAddressJson? Address { get; set; }
    }

    public class RequestAddressJson
    {
        public string? Street { get; set; }
        public string? Complement { get; set; }
        public string? Neighborhood { get; set; }
        public string? City { get; set; }
        public string? UF { get; set; }
        public string? Cep { get; set; }
    }
}
