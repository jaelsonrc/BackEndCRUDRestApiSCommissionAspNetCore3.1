using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace ServiceCommission.Utils
{
    public static class Helpers
    {

        public static int RandomNumber6digits(char padLeft = '0')
        {
           return int.Parse(new Random().Next(999999).ToString().PadLeft(6, padLeft));
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static string GetTemplete(string name)
        {
            return File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "Pages", name));
        }

        public static bool SendEmail(string destino, string assunto, string corpo, IList<KeyValuePair<byte[], ContentType>> anexos)
        {
            try
            {

                var mail = new MailMessage();

                mail.From = new MailAddress(Setting.DefaultEmail);
                mail.To.Add(destino); // para
                mail.Subject = assunto; // assunto
                mail.Body = corpo; // mensagem
                mail.IsBodyHtml = true;

                // em caso de anexos
                if (anexos?.Count > 0)
                    anexos.ToList().ForEach(a =>
                    {
                        Stream file = new MemoryStream(a.Key);
                        mail.Attachments.Add(new Attachment(file, a.Value));
                    });

                using (var smtp = new SmtpClient("smtp.gmail.com"))
                {
                    smtp.EnableSsl = true; // GMail requer SSL
                    smtp.Port = 587;       // porta para SSL
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // modo de envio
                    smtp.UseDefaultCredentials = false; // vamos utilizar credencias especificas

                    // seu usuário e senha para autenticação
                    smtp.Credentials = new NetworkCredential(Setting.DefaultEmail, Setting.PasswordEmail);

                    // envia o e-mail
                    smtp.Send(mail);

                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
