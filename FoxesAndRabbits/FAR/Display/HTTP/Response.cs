using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FoxesAndRabbits.FAR.Display.HTTP {

    internal class Maps {

        static internal Dictionary<int, string> statusCodeMap = new Dictionary<int, string> {

            { 200, "OK" },
            { 201, "Created" },
            { 202, "Accepted" },
            { 204, "No Content" },
            { 400, "Bad Request" },
            { 401, "Unauthorized" },
            { 403, "Forbidden" },
            { 404, "Not Found" },
            { 405, "Method Not Allowed" },
            { 409, "Conflict" },
            { 500, "Internal Server Error" },
            { 501, "Not Implemented" },
            { 502, "Bad Gateway" },
            { 503, "Service Unavailable" },
            { 504, "Gateway Timeout" }

        };

        internal static string GetStatusCodeMessage(int statusCode) {

            if (!statusCodeMap.ContainsKey(statusCode)) return "Unknown Code";
            return statusCodeMap[statusCode];

        }

        static internal Dictionary<string, string> mimeTypeMap = new Dictionary<string, string> {

            { ".html", "text/html" },
            { ".css", "text/css" },
            { ".js", "application/javascript" },
            { ".json", "application/json" },
            { ".png", "image/png" },
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".gif", "image/gif" },
            { ".svg", "image/svg+xml" },
            { ".txt", "text/plain" },
            { ".xml", "application/xml" },
            { ".pdf", "application/pdf" },
            { ".zip", "application/zip" },
            { ".mp3", "audio/mpeg" },
            { ".mp4", "video/mp4" }

        };

        internal static string GetMIMEType(string extension) {

            if (!mimeTypeMap.ContainsKey(extension)) return "application/octet-stream";
            return mimeTypeMap[extension];

        }

    }

    public class Response {
        
        int statusCode, contentLength;
        string statusCodeName, contentType;

        byte[] bytes;

        public Response(int statusCode, FileInfo file) {

            this.statusCode = statusCode;
            statusCodeName = Maps.GetStatusCodeMessage(statusCode);

            contentType = Maps.GetMIMEType(file.Extension);

            bytes = File.ReadAllBytes(file.FullName);
            contentLength = bytes.Length;

        }

        public Response(int statusCode, byte[] bytes) {

            this.statusCode = statusCode;
            statusCodeName = Maps.GetStatusCodeMessage(statusCode);

            contentType = Maps.GetMIMEType("unknown");

            this.bytes = bytes;
            contentLength = bytes.Length;

        }

        public Response(int statusCode, string rawText) {

            this.statusCode = statusCode;
            statusCodeName = Maps.GetStatusCodeMessage(statusCode);

            contentType = "text/plain";

            bytes = Encoding.UTF8.GetBytes(rawText);
            contentLength = bytes.Length;

        }

        public byte[] Build() {

            string headerString = $"HTTP/1.1 {statusCode} {statusCodeName}\r\n" +
                            $"Content-Type: {contentType}\r\n" +
                            $"Content-Length: {contentLength}\r\n" +
                            "Access-Control-Allow-Origin: *" +
                            "Connection: close\r\n\r\n";

            byte[] header = Encoding.UTF8.GetBytes(headerString);

            byte[] response = new byte[header.Length + contentLength];
            Array.Copy(header, response, header.Length);
            Array.Copy(bytes, 0, response, header.Length, bytes.Length);

            return response;

        }

    }

}