using FluentValidation;
using PlataformaEstagios.Communication.Requests;
using System.Text.RegularExpressions;

namespace PlataformaEstagios.Application.UseCases.Candidate.UpdateProfile
{
    public sealed class RequestUpdateCandidateProfileJsonValidator : AbstractValidator<RequestUpdateCandidateProfileJson>
    {
        public RequestUpdateCandidateProfileJsonValidator()
        {
            // ---- Dados básicos (só valida se vierem) ----
            When(x => !string.IsNullOrWhiteSpace(x.FullName), () =>
            {
                RuleFor(x => x.FullName!).MaximumLength(150);
            });

            When(x => x.BirthDate.HasValue, () =>
            {
                RuleFor(x => x.BirthDate!.Value)
                    .LessThan(DateOnly.FromDateTime(DateTime.UtcNow))
                    .WithMessage("BirthDate deve ser no passado.")
                    .GreaterThan(new DateOnly(1900, 1, 1));
            });

            When(x => !string.IsNullOrWhiteSpace(x.Course), () =>
            {
                RuleFor(x => x.Course!).MaximumLength(120);
            });

            // ---- Endereço (opcional) ----
            When(x => x.Address is not null, () =>
            {
                RuleFor(x => x.Address!.UF)
                    .Must(uf => string.IsNullOrWhiteSpace(uf) || Regex.IsMatch(uf!, "^[A-Z]{2}$"))
                    .WithMessage("UF deve conter 2 letras maiúsculas.");

                RuleFor(x => x.Address!.CEP)
                    .Must(IsCepValid)
                    .WithMessage("CEP inválido.");

                RuleFor(x => x.Address!.Street).MaximumLength(150);
                RuleFor(x => x.Address!.Neighborhood).MaximumLength(100);
                RuleFor(x => x.Address!.City).MaximumLength(100);
                RuleFor(x => x.Address!.Complement).MaximumLength(150);
            });

            // ---- Arquivos em Base64 (opcionais) ----
            When(x => !string.IsNullOrWhiteSpace(x.ProfilePictureBase64), () =>
            {
                RuleFor(x => x.ProfilePictureBase64!)
                    .Must(BeValidBase64Payload).WithMessage("ProfilePictureBase64 inválido.")
                    .Must(BeSupportedImage).WithMessage("Imagem deve ser JPEG, PNG ou WEBP.");
            });

            When(x => !string.IsNullOrWhiteSpace(x.ResumeBase64), () =>
            {
                RuleFor(x => x.ResumeBase64!)
                    .Must(BeValidBase64Payload).WithMessage("ResumeBase64 inválido.")
                    .Must(BePdf).WithMessage("Currículo deve ser um PDF válido.");
            });
        }

        private static bool IsCepValid(string? cep)
        {
            if (string.IsNullOrWhiteSpace(cep)) return true; // opcional
            var digits = new string(cep.Where(char.IsDigit).ToArray());
            return digits.Length == 8;
        }

        // Aceita "data:*;base64,..." ou base64 puro
        private static bool BeValidBase64Payload(string base64)
        {
            try
            {
                var raw = StripDataUri(base64);
                // Checagem de base64 válida
                _ = Convert.FromBase64String(raw);
                return true;
            }
            catch { return false; }
        }

        private static bool BeSupportedImage(string base64)
        {
            try
            {
                var raw = StripDataUri(base64, out var mime);
                var bytes = Convert.FromBase64String(raw);

                if (!string.IsNullOrWhiteSpace(mime))
                {
                    mime = mime.ToLowerInvariant();
                    if (mime is "image/jpeg" or "image/jpg" or "image/png" or "image/webp") return true;
                }

                // Magic numbers
                return IsJpeg(bytes) || IsPng(bytes) || IsWebp(bytes);
            }
            catch { return false; }
        }

        private static bool BePdf(string base64)
        {
            try
            {
                var raw = StripDataUri(base64, out var mime);
                var bytes = Convert.FromBase64String(raw);

                if (!string.IsNullOrWhiteSpace(mime))
                    return string.Equals(mime, "application/pdf", StringComparison.OrdinalIgnoreCase);

                // "%PDF"
                return bytes.Length >= 4 &&
                       bytes[0] == 0x25 && bytes[1] == 0x50 &&
                       bytes[2] == 0x44 && bytes[3] == 0x46;
            }
            catch { return false; }
        }

        private static string StripDataUri(string input) => StripDataUri(input, out _);

        private static string StripDataUri(string input, out string? mime)
        {
            mime = null;
            if (!input.StartsWith("data:", StringComparison.OrdinalIgnoreCase)) return input;

            // data:<mime>;base64,<payload>
            var colon = input.IndexOf(':');
            var semi = input.IndexOf(';');
            var comma = input.IndexOf(',');

            if (colon >= 0 && semi > colon && comma > semi)
            {
                mime = input.Substring(colon + 1, semi - colon - 1);
                return input[(comma + 1)..];
            }
            return input;
        }

        private static bool IsJpeg(byte[] b)
            => b.Length >= 3 && b[0] == 0xFF && b[1] == 0xD8 && b[2] == 0xFF;

        private static bool IsPng(byte[] b)
            => b.Length >= 8 && b[0] == 0x89 && b[1] == 0x50 && b[2] == 0x4E && b[3] == 0x47;

        private static bool IsWebp(byte[] b)
            => b.Length >= 12 &&
               b[0] == 0x52 && b[1] == 0x49 && b[2] == 0x46 && b[3] == 0x46 && // RIFF
               b[8] == 0x57 && b[9] == 0x45 && b[10] == 0x42 && b[11] == 0x50; // WEBP
    }
}
