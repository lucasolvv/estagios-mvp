// PlataformaEstagios.Infrastructure/Storage/LocalFileStorage.cs
using Microsoft.Extensions.Hosting;
using PlataformaEstagios.Domain.Repositories;

namespace PlataformaEstagios.Infrastructure.Storage
{
    public sealed class LocalFileStorage : IFileStorage
    {
        private readonly string _webRootPath;      // {contentRoot}/wwwroot
        private readonly string _contentRootPath;  // {contentRoot}

        public LocalFileStorage(IHostEnvironment env)
        {
            _contentRootPath = env.ContentRootPath;
            _webRootPath = Path.Combine(_contentRootPath, "wwwroot");
            Directory.CreateDirectory(_webRootPath);
        }

        public async Task<string> SavePublicProfilePictureBase64Async(
            Guid candidateId, string base64, string fileBaseName, CancellationToken ct)
        {
            var bytes = FromBase64(base64);
            var ext = DetectImageExtension(bytes, TryGetDataUriMime(base64)) ?? ".jpg"; // fallback

            var relDir = Path.Combine("uploads", "candidates", candidateId.ToString());
            var absDir = Path.Combine(_webRootPath, relDir);
            Directory.CreateDirectory(absDir);

            var safeBase = SafeFileBase(fileBaseName);
            var relFile = Path.Combine(relDir, $"{safeBase}_profile{ext}").Replace('\\', '/');
            var absFile = Path.Combine(_webRootPath, relFile.Replace('/', Path.DirectorySeparatorChar));

            await SaveBytesAsync(bytes, absFile, ct);

            return "/" + relFile; // caminho web relativo
        }

        public async Task<string> SavePublicResumeBase64Async(
            Guid candidateId, string base64, string fileBaseName, CancellationToken ct)
        {
            var bytes = FromBase64(base64);
            if (!IsPdf(bytes)) throw new InvalidOperationException("Apenas PDF é aceito para o currículo.");

            var relDir = Path.Combine("uploads", "candidates", candidateId.ToString());
            var absDir = Path.Combine(_webRootPath, relDir);
            Directory.CreateDirectory(absDir);

            var safeBase = SafeFileBase(fileBaseName);
            var relFile = Path.Combine(relDir, $"{safeBase}_resume.pdf").Replace('\\', '/');
            var absFile = Path.Combine(_webRootPath, relFile.Replace('/', Path.DirectorySeparatorChar));

            await SaveBytesAsync(bytes, absFile, ct);

            return "/" + relFile; // caminho web relativo (começando com /)
        }


        public Task<Stream> OpenPrivateAsync(string privatePath, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(privatePath))
                throw new ArgumentException("Caminho privado inválido.", nameof(privatePath));

            var abs = Path.Combine(_contentRootPath, privatePath.Replace('/', Path.DirectorySeparatorChar));
            if (!File.Exists(abs))
                throw new FileNotFoundException("Arquivo privado não encontrado.", abs);

            Stream stream = new FileStream(abs, FileMode.Open, FileAccess.Read, FileShare.Read, 64 * 1024, useAsync: true);
            return Task.FromResult(stream);
        }

        // --- Helpers ---

        private static byte[] FromBase64(string base64)
        {
            var comma = base64.IndexOf(',');
            var raw = comma >= 0 ? base64[(comma + 1)..] : base64;
            return Convert.FromBase64String(raw);
        }

        private static string? TryGetDataUriMime(string base64)
        {
            // Ex.: data:image/png;base64,AAA...
            if (!base64.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                return null;

            var semi = base64.IndexOf(';');
            var colon = base64.IndexOf(':');
            if (colon < 0 || semi < 0 || semi < colon) return null;

            return base64.Substring(colon + 1, semi - colon - 1).Trim();
        }

        private static string? DetectImageExtension(byte[] bytes, string? mime)
        {
            // 1) Se vier MIME na data URI, usa
            if (!string.IsNullOrWhiteSpace(mime))
            {
                return mime.ToLowerInvariant() switch
                {
                    "image/jpeg" => ".jpg",
                    "image/jpg" => ".jpg",
                    "image/png" => ".png",
                    "image/webp" => ".webp",
                    _ => null
                };
            }

            // 2) Por magic number
            if (IsJpeg(bytes)) return ".jpg";
            if (IsPng(bytes)) return ".png";
            if (IsWebp(bytes)) return ".webp";
            return null;
        }

        private static bool IsPdf(byte[] bytes)
            => bytes.Length >= 4 &&
               bytes[0] == 0x25 && bytes[1] == 0x50 && bytes[2] == 0x44 && bytes[3] == 0x46; // %PDF

        private static bool IsJpeg(byte[] b)
            => b.Length >= 3 && b[0] == 0xFF && b[1] == 0xD8 && b[2] == 0xFF;

        private static bool IsPng(byte[] b)
            => b.Length >= 8 && b[0] == 0x89 && b[1] == 0x50 && b[2] == 0x4E && b[3] == 0x47;

        private static bool IsWebp(byte[] b)
            => b.Length >= 12 &&
               b[0] == 0x52 && b[1] == 0x49 && b[2] == 0x46 && b[3] == 0x46 && // RIFF
               b[8] == 0x57 && b[9] == 0x45 && b[10] == 0x42 && b[11] == 0x50; // WEBP

        private static async Task SaveBytesAsync(byte[] bytes, string absPath, CancellationToken ct)
        {
            var dir = Path.GetDirectoryName(absPath);
            if (!string.IsNullOrWhiteSpace(dir)) Directory.CreateDirectory(dir);

            await using var fs = new FileStream(absPath, FileMode.Create, FileAccess.Write, FileShare.None, 128 * 1024, useAsync: true);
            await fs.WriteAsync(bytes, 0, bytes.Length, ct);
            await fs.FlushAsync(ct);
        }

        private static string SafeFileBase(string baseName)
        {
            if (string.IsNullOrWhiteSpace(baseName)) return "candidate";
            var allowed = new string(baseName.ToLowerInvariant()
                .Select(c => char.IsLetterOrDigit(c) || c == '-' || c == '_' ? c : '-')
                .ToArray());
            while (allowed.Contains("--")) allowed = allowed.Replace("--", "-");
            return string.IsNullOrWhiteSpace(allowed) ? "candidate" : allowed.Trim('-');
        }
    }
}
