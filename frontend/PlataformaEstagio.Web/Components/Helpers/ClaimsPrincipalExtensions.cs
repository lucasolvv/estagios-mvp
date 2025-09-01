using System.Security.Claims;

namespace PlataformaEstagio.Web.Components.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetEmail(this ClaimsPrincipal u) =>
            u?.FindFirst(ClaimTypes.Email)?.Value
            ?? u?.FindFirst("email")?.Value;

        public static string? GetRole(this ClaimsPrincipal u) =>
            u?.FindFirst(ClaimTypes.Role)?.Value
            ?? u?.FindFirst("role")?.Value;

        public static string? GetNickname(this ClaimsPrincipal u) =>
            u?.FindFirst(ClaimTypes.Name)?.Value
            ?? u?.FindFirst("nickname")?.Value;

        public static Guid? GetUserId(this ClaimsPrincipal u)
        {
            var v = u?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                 ?? u?.FindFirst("uid")?.Value
                 ?? u?.FindFirst("sub")?.Value;
            return Guid.TryParse(v, out var g) ? g : null;
        }

        public static Guid? GetUserTypeId(this ClaimsPrincipal u)
        {
            var v = u?.FindFirst("userTypeId")?.Value;
            return Guid.TryParse(v, out var g) ? g : null;
        }

        public static bool IsEnterprise(this ClaimsPrincipal u) => u?.IsInRole("Enterprise") ?? false;
        public static bool IsCandidate(this ClaimsPrincipal u) => u?.IsInRole("Candidate") ?? false;
    }
}
