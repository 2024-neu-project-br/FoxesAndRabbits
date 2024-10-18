using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FoxesAndRabbits.FAR.Display.HTTP;
using FoxesAndRabbits.FAR.Game;

namespace FoxesAndRabbits.FAR.Display {
    
    public class HTTPHandler {
        
        HTTPConnection con;

        public HTTPHandler(HTTPConnection con) {

            this.con = con;

        }

        public void Handle(Request rq) {

            switch (rq.requestType) {

                case "GET": HandleGET(rq); break;
                case "POST": HandlePOST(rq); break;

            };

            con.Close();

        }

        private void HandleGET(Request rq) {

            FileInfo file = new FileInfo("./FAR/Display/WebPage" + rq.PATH);

            Response response;

            if (!file.Exists) {

                response = new Response(404, "File not found.");
                con.Send(response.Build());

                return;

            }

            response = new Response(200, file);
            con.Send(response.Build());

        }

        private void HandlePOST(Request rq) {

            Response response;
            string name = rq.BODY.Split("=")[1];

            if (name == "<empty>") {

                response = new Response(400, "No instance provided.");
                con.Send(response.Build());

                return;

            }

            if (rq.PATH == "/newInstance") {

                if (Game.Game.GetInstance(name) != null) {

                    response = new Response(403, "Instance already exists.");
                    con.Send(response.Build());

                    return;

                }

                int width, height;
                bool isMapBlank;

                try {

                    string[] sparamz = rq.BODY.Split("\n");
                    Dictionary<string, string> paramz = new Dictionary<string, string>();

                    foreach (string p in sparamz) {

                        string[] sparam = p.Split("=");
                        paramz.Add(sparam[0], sparam[1]);

                    }

                    width = int.Parse(paramz["width"]);
                    height = int.Parse(paramz["height"]);

                    isMapBlank = bool.Parse(paramz["isMapBlank"]);

                } catch (Exception) {

                    response = new Response(400, "You seriously fucked up something.");
                    con.Send(response.Build());

                    return;

                }

                GameInstance instance = new GameInstance(name, width, height, isMapBlank);
                Game.Game.instances.Add(instance);

                response = new Response(200, instance.State());
                con.Send(response.Build());

                return;

            }

            if (rq.PATH == "/tick") {

                GameInstance instance = Game.Game.GetInstance(name);

                if (instance == null) {

                    response = new Response(400, "WTF?");
                    con.Send(response.Build());

                    return;

                }

                instance.Tick();
                instance.Update();

                response = new Response(200, instance.State());
                con.Send(response.Build());

                return;

            }

            /*
            
            
            
                ADD ALL OTHER POST REQUEST COMMANDS HERE THANK YOU ZRAPHY
            
            
            
            */

            response = new Response(404, "You're a dumb fuck.");
            con.Send(response.Build());

        }

        public void Debug(Request rq) {

            Console.WriteLine(rq.headerString + "\n\n" + rq.BODY);
            byte[] response = new Response(200, "w skibidi rizz ohio sigma gyatt").Build();
            con.Send(response);
            con.Close();

        }

    }

}