using QRCoder;

namespace TrackMeBack_Management.Clase_Utilitare.Servicii
{
    public class Servicii_QR
    {
        public static string Generare_QR(string cladire, string? corp, string index)
        {
            string text_qr = string.Empty;
            if (corp != null)
            {
                text_qr = cladire + "_" + corp + "_" + index;
            }
            else
            {
                text_qr = cladire + "_" + index;
            }

            QRCodeGenerator generator_qr = new QRCodeGenerator();
            QRCodeData data_qr = generator_qr.CreateQrCode(text_qr, QRCodeGenerator.ECCLevel.Q);
            Base64QRCode cod_qr = new Base64QRCode(data_qr);
            string qr_format_Base64 = cod_qr.GetGraphic(20);

            return qr_format_Base64;
        }
    }
}
