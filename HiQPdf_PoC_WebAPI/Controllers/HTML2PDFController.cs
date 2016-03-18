using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Net.Http.Headers;
using HiQPdf;

namespace HiQPdf_PoC_WebAPI.Controllers
{
    public class HTML2PDFController : ApiController
    {
        /// <summary>
        /// GET operation to convert a HTML document (given by URL) to a PDF using the CSS-Mediaquery (given by cssMediaType)
        /// </summary>
        /// <param name="URL">URL to load the HTML file from</param>
        /// <param name="cssMediaType">CSS MediaType to use, Possible values are "print" or "screen"; default is "print"</param>
        public HttpResponseMessage GET(String URL, String cssMediaType)
        {

            // create the HTML to PDF converter
            HtmlToPdf htmlToPdfConverter = new HtmlToPdf();

            if (cssMediaType != "screen") {
                cssMediaType = "print";
            }

            // set a demo serial number
            htmlToPdfConverter.SerialNumber = "YCgJMTAE-BiwJAhIB-EhlWTlBA-UEBRQFBA-U1FOUVJO-WVlZWQ==";

            // set the selected media type
            htmlToPdfConverter.MediaType = cssMediaType;

            htmlToPdfConverter.TriggerMode = ConversionTriggerMode.WaitTime;
            htmlToPdfConverter.WaitBeforeConvert = 10;

            htmlToPdfConverter.Document.Margins = new PdfMargins(12, 12, 40, 40);

            byte[] pdfBuffer = null;
            pdfBuffer = htmlToPdfConverter.ConvertUrlToMemory(URL);

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            MemoryStream memoryStream = new MemoryStream(pdfBuffer);
            result.Content = new ByteArrayContent(memoryStream.ToArray());

            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/pdf");
            result.Content.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("inline") { FileName = "File.pdf" };

            return result;
        }



    }

}
