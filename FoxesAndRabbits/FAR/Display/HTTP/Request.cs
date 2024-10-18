using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxesAndRabbits.FAR.Display.HTTP {

    public class Request {
        
        int headerLength;

        byte[] rawData;
        byte[] body = Array.Empty<byte>();
        byte[] header = Array.Empty<byte>();

        public string headerString = "";
        public string requestType = "";
        public string httpVersion = "";

        public readonly string BODY = "";
        public readonly string PATH = "";

        public Dictionary<string, string> headers = new Dictionary<string, string>();

        public Request(byte[] rawData) {

            this.rawData = rawData;

            if (rawData.Length < 4) return;

            for (int i = 0; i < rawData.Length - 3; i++)
                if (rawData[i] == 13 && rawData[i + 1] == 10 && rawData[i + 2] == 13 && rawData[i + 3] == 10) {

                headerLength = i;
                break;

            }

            if (headerLength < 0) {

                header = Array.Empty<byte>();
                body = Array.Empty<byte>();
                return;

            }

            header = new byte[headerLength];
            for (int i = 0; i < headerLength; i++) header[i] = rawData[i];

            body = new byte[rawData.Length - (headerLength + 4)];
            for (int i = 0; i < body.Length; i++) body[i] = rawData[headerLength + 4 + i];

            headerString = Encoding.UTF8.GetString(header);
            BODY = Encoding.UTF8.GetString(body);

            string[] headers = headerString.Split("\r\n");
            
            string[] mainHeader = headers[0].Split(" ");

            if (mainHeader.Length < 3) return;

            requestType = mainHeader[0];
            PATH = mainHeader[1];
            httpVersion = mainHeader[2];

            for (int i = 1; i < headers.Length; i++) {
                
                string[] sh = headers[i].Split(": ");
                this.headers.Add(sh[0], sh[1]);

            }

        }

    }

}