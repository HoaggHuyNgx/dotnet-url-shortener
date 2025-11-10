namespace UrlShortener.Core.Interfaces;

public interface IQrCodeService
{
    /// <summary>
    /// Generates a QR code for the given text and returns it as a Base64 encoded string.
    /// </summary>
    /// <param name="text">The text to encode in the QR code.</param>
    /// <returns>A Base64 string representing the QR code image.</returns>
    string GenerateQrCodeAsBase64(string text);
}
