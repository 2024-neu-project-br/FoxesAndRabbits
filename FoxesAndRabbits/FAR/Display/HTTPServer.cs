using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FoxesAndRabbits.FAR.Display {

    public class HTTPServer {

        IPAddress IP;
        int PORT;

        Socket listener;

        public List<HTTPConnection> connections = new List<HTTPConnection>();

        public HTTPServer(IPAddress IP, int PORT) {

            this.IP = IP;
            this.PORT = PORT;

            listener = new Socket(IP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IP, PORT));

            listener.Listen(10);

            while (true) {

                Socket con = listener.Accept();
                connections.Add(new HTTPConnection(con));

            }

        }


    }

}