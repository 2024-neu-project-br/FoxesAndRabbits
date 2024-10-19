using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using FoxesAndRabbits.FAR.Display.HTTP;

namespace FoxesAndRabbits.FAR.Display {

    public class HTTPConnection {
        
        public Socket socket;
        HTTPHandler handler;

        bool killed = false;
        Thread thread;

        public HTTPConnection(Socket socket) {

            this.socket = socket;
            handler = new HTTPHandler(this);

            thread = new Thread(new ThreadStart(Main));
            thread.Start();

        }

        public void Main() {

            while (!killed) {

                Console.WriteLine("Buffer size: " + socket.ReceiveBufferSize);

                byte[] data = new byte[socket.ReceiveBufferSize]; // go fuck yourself
                int bytesReceived = socket.Receive(data);

                Request rq = new(data);
                Console.WriteLine(rq.headerString);
                handler.Handle(rq);

            }

        }

        public void Send(byte[] bytes) {

            socket.Send(bytes);

        }

        public void Close() {

            killed = true;
            thread.Join();
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();

        }

    }

}