using PlataformaEstagios.Domain.Enums;

namespace PlataformaEstagios.Domain.Entities
{
    public class User : EntityBase
    {
        public Guid UserIdentifier { get; set; } = Guid.NewGuid();
        public string Nickname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public UserType UserType { get; set; }
    }
}
