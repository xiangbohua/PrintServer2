using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HTTPServerLib;
using System.IO;

namespace PrintServer2.Server
{
    public delegate string PostRequestReceived(HttpRequest request, string requestBody);
    public class HTTPServer : HttpServer
    {
        public PostRequestReceived OnPostRequestReceived = null;

        public HTTPServer(string ipAddress, int port)
            : base(ipAddress, port)
        {

        }

        public override void OnPost(HttpRequest request, HttpResponse response)
        {
            var returnMessage = "";
            try
            {
                if (this.OnPostRequestReceived == null)
                {
                    returnMessage = "Print server was not right configurated please concat your administrator!";
                }
                else
                {
                    returnMessage = this.OnPostRequestReceived(request, request.Body);
                }
            }
            catch (Exception ex)
            {
                returnMessage = "Unhanlded exception :" + ex.Message;
            }

            string jsonResult = "{\"code\":200, \"msg\":\"" + returnMessage + "\"}";

            //build the response header
            response.SetContent(jsonResult);
            response.Content_Encoding = "utf-8";
            response.StatusCode = "200";
            response.Content_Type = "text/json; charset=UTF-8";
            response.Headers = new Dictionary<string, string>();
            response.SetHeader("Access-Control-Allow-Origin", "*");
            response.SetHeader("Access-Control-Allow-Headers", "Origin, Content-Type, Cookie, Accept");
            response.SetHeader("Access-Control-Allow-Methods", "GET, POST");
            response.SetHeader("Access-Control-Allow-Credentials", "true");
            response.SetHeader("Server", "PrintServer");

            //send the response
            response.Send();
        }

        /// <summary>
        /// Report server status when GET request received
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public override void OnGet(HttpRequest request, HttpResponse response)
        {
            response = response.SetContent("Print server is running...", Encoding.UTF8);
            response.Content_Type = "text/html; charset=UTF-8";
            response.StatusCode = "200";
            response.Send();
        }

        private string ConvertPath(string[] urls)
        {
            string html = string.Empty;
            int length = ServerRoot.Length;
            foreach (var url in urls)
            {
                var s = url.StartsWith("..") ? url : url.Substring(length).TrimEnd('\\');
                html += String.Format("<li><a href=\"{0}\">{0}</a></li>", s);
            }

            return html;
        }

        private string ListDirectory(string requestDirectory, string requestURL)
        {
            //List directory
            var folders = requestURL.Length > 1 ? new string[] { "../" } : new string[] { };
            folders = folders.Concat(Directory.GetDirectories(requestDirectory)).ToArray();
            var foldersList = ConvertPath(folders);

            //List files
            var files = Directory.GetFiles(requestDirectory);
            var filesList = ConvertPath(files);

            //build html
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("<html><head><title>{0}</title></head>", requestDirectory));
            builder.Append(string.Format("<body><h1>{0}</h1><br/><ul>{1}{2}</ul></body></html>",
                 requestURL, filesList, foldersList));

            return builder.ToString();
        }
    }
}
