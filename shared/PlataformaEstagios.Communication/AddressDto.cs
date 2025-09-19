namespace PlataformaEstagios.Communication
{
    public sealed class AddressDto
    {
        public string? Street { get; set; }
        public string? Complement { get; set; }
        public string? Neighborhood { get; set; }
        public string? City { get; set; }
        public string? Uf { get; set; }  // 2 letras
        public string? Cep { get; set; } // 8 dígitos numéricos
    }
}
