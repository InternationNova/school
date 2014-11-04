using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using school.Models;
using System.IO;
using school.Classes;

using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace school.Classes
{

   
    public class StandardPdfRenderer
    {
        private const int HorizontalMargin = 40;
        private const int VerticalMargin = 40;

        public byte[] Render(string htmlText, string pageTitle)
        {
            byte[] renderedBuffer;

            using (var outputMemoryStream = new MemoryStream())
            {
                using (var pdfDocument = new Document(PageSize.A4, HorizontalMargin, HorizontalMargin, VerticalMargin, VerticalMargin))
                {
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDocument, outputMemoryStream);
                    pdfWriter.CloseStream = false;
                    pdfWriter.PageEvent = new PrintHeaderFooter { Title = pageTitle };
                    pdfDocument.Open();
                    using (var htmlViewReader = new StringReader(htmlText))
                    {
                        using (var htmlWorker = new HTMLWorker(pdfDocument))
                        {
                            htmlWorker.Parse(htmlViewReader);
                        }
                    }
                }

                renderedBuffer = new byte[outputMemoryStream.Position];
                outputMemoryStream.Position = 0;
                outputMemoryStream.Read(renderedBuffer, 0, renderedBuffer.Length);
            }

            return renderedBuffer;
        }
    }
}