using SelectPdf;
using System;
using System.Linq;

namespace AllupBackendProject.Helpers
{
    public class Helper
    {
        public static void DeleteImg(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        public enum UserRoles
        {
            Admin,
            Member,
            SuperAdmin
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static byte[] SendInvoice(int id)
        {
            var desktopView = new HtmlToPdf();
            desktopView.Options.WebPageWidth = 1024;


            var pdf = desktopView.ConvertUrl($"https://localhost:44300/order/invoice/{id}");
            var pdfBytes = pdf.Save();

            //using (var streamWriter = new StreamWriter($@"C:\Users\User\Desktop\Back-End Project\Back-End-Project\Allup\wwwroot\Pdfs\invoice({id}).pdf"))
            //{
            //    streamWriter.BaseStream.WriteAsync(pdfBytes, 0, pdfBytes.Length);
            //}

            return pdfBytes;
        }
    }
}
