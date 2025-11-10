using QRCoder;
using System.Drawing;
using System.IO;
using UrlShortener.Core.Interfaces;

namespace UrlShortener.Infrastructure.Services;

public class QrCodeService : IQrCodeService
{
    public string GenerateQrCodeAsBase64(string text)
    {
        using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
        {
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
            {
                byte[] qrCodeBytes = qrCode.GetGraphic(20); // Kích thước pixel của mỗi module
                return Convert.ToBase64String(qrCodeBytes);
            }
        }
    }
}
