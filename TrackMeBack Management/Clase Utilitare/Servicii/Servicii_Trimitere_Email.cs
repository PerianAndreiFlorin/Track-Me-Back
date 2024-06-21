using MimeKit;
using MailKit.Net.Smtp;
using MongoDB.Bson;
using TrackMeBack_Management.Clase_Utilitare.Management;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TrackMeBack_Management.Clase_Utilitare.Servicii
{
    public class Servicii_Trimitere_Email
    {
        public static void Email_Resetare_Parola(string email_destinatar, string cod, BsonDateTime data)
        {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Track Me Back", "track.me.back.upt@gmail.com"));
                message.To.Add(new MailboxAddress(Management_UtilizatorCurent.Nume_Email(email_destinatar), email_destinatar));

                //Subiect si continut.
                message.Subject = "Resetarea Parolei";
                string mesaj = "Bună " + Management_UtilizatorCurent.Nume_Email(email_destinatar) + ",\r\n\r\n" +
                               " Codul de resetare a parolei pentru contul: " + email_destinatar + " va fi disponibil pe o durată de 1:30 minute, începând din: " + data +
                               "\r\n\r\nCod: " + cod +
                               "\r\n\r\n-- Track Me Back - Management";

                message.Body = new TextPart("plain")
                {
                    Text = mesaj
                };

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 465, true);

                    // Autentificare Server SMTP - Gmail.
                    client.Authenticate("track.me.back.upt@gmail.com", "fvmj eofi anos umbu");

                    client.Send(message);
                    client.Disconnect(true);
                }
        }

        public static void Email_Informare_Parola(string email_destinatar, string parola_noua)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Track Me Back", "track.me.back.upt@gmail.com"));
            message.To.Add(new MailboxAddress(Management_UtilizatorCurent.Nume_Email(email_destinatar), email_destinatar));

            //Subiect si continut.
            message.Subject = "Modificarea Parolei";
            string mesaj = "Bună " + Management_UtilizatorCurent.Nume_Email(email_destinatar) + ",\r\n\r\n" +
                               " Parola dumneavoastră a fost modificată. "+
                               "\r\n\r\nNoua parolă este: " + parola_noua +
                               "\r\n\r\n-- Track Me Back - Management";

            message.Body = new TextPart("plain")
            {
                Text = mesaj
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 465, true);

                // Autentificare Server SMTP - Gmail.
                client.Authenticate("track.me.back.upt@gmail.com", "fvmj eofi anos umbu");

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
