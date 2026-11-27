using HomeHunt.Common;
using static HomeHunt.Common.ValidationConstants.Property;

namespace HomeHunt.Validation
{
    public static class ImageFileValidator
    {
        private static readonly byte?[][] Signatures =
        {
            new byte?[] { 0xFF, 0xD8, 0xFF },
            new byte?[] { 0x89, 0x50, 0x4E, 0x47 },
            new byte?[] { 0x47, 0x49, 0x46, 0x38 },
            new byte?[] { 0x52, 0x49, 0x46, 0x46, null, null, null, null, 0x57, 0x45, 0x42, 0x50 }
        };

        public static async Task<List<string>> ValidateAsync(IEnumerable<IFormFile> files, CancellationToken cancellationToken = default)
        {
            List<string> errors = new List<string>();

            foreach (IFormFile file in files)
            {
                string name = Path.GetFileName(file.FileName);

                if (file.Length > MaxImageSizeInBytes)
                {
                    errors.Add(string.Format(Messages.Property.ImageTooLargeMessage, name, MaxImageSizeInMegabytes));
                }
                else if (!HasAllowedExtension(name) || !await HasImageSignatureAsync(file, cancellationToken))
                {
                    errors.Add(string.Format(Messages.Property.ImageTypeInvalidMessage, name));
                }
            }

            return errors;
        }

        private static bool HasAllowedExtension(string fileName)
        {
            return AllowedImageExtensions.Contains(Path.GetExtension(fileName), StringComparer.OrdinalIgnoreCase);
        }

        private static async Task<bool> HasImageSignatureAsync(IFormFile file, CancellationToken cancellationToken)
        {
            byte[] header = new byte[Signatures.Max(signature => signature.Length)];

            await using Stream stream = file.OpenReadStream();
            int read = await stream.ReadAsync(header, cancellationToken);

            return Signatures.Any(signature => signature.Length <= read && Matches(header, signature));
        }

        private static bool Matches(byte[] header, byte?[] signature)
        {
            return signature.Select((expected, index) => expected == null || header[index] == expected).All(match => match);
        }
    }
}
