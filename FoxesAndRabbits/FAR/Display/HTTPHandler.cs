using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FoxesAndRabbits.FAR.Display.HTTP;
using FoxesAndRabbits.FAR.Entities;
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

            string[] sparamz = rq.BODY.Split("\n");
            Dictionary<string, string> paramz = new Dictionary<string, string>();

            foreach (string p in sparamz) {

                string[] sparam = p.Split("=");
                paramz.Add(sparam[0], sparam[1]); // this is not foolproof

            }

            string name = paramz["name"];

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

                    width = int.Parse(paramz["width"]);
                    height = int.Parse(paramz["height"]);

                    isMapBlank = bool.Parse(paramz["isMapBlank"]);

                } catch (Exception) {

                    response = new Response(400, "You seriously messed up something.");
                    con.Send(response.Build());

                    return;

                }

                GameInstance instance = new GameInstance(name, width, height, isMapBlank);
                Game.Game.instances.Add(instance);

                response = new Response(200, instance.State());
                con.Send(response.Build());

                return;

            }

            if (rq.PATH == "/removeInstance") {

                GameInstance instance = Game.Game.GetInstance(name);
                if (instance == null) {

                    response = new Response(400, "This instance doesn't exist.");
                    con.Send(response.Build());

                    return;

                }

                Game.Game.instances.Remove(instance);

                response = new Response(200, "Done.");
                con.Send(response.Build());

                return;

            }

            if (rq.PATH == "/tick") {

                GameInstance instance = Game.Game.GetInstance(name);

                if (instance == null) {

                    response = new Response(400, "This instance doesn't exist.");
                    con.Send(response.Build());

                    return;

                }

                instance.Update();
                instance.Tick();

                response = new Response(200, instance.State());
                con.Send(response.Build());

                return;

            }

            if (rq.PATH == "/command") {

                GameInstance instance = Game.Game.GetInstance(name);

                if (instance == null) {

                    response = new Response(400, "Broooo, can you stop sending requests to non-existent instances? Please???");
                    con.Send(response.Build());

                    return;

                }

                string command = paramz["command"];
                string[] scommand = command.Split(" ");

                int x = int.Parse(scommand[2]), y = int.Parse(scommand[3]);

                if (scommand[0] == "-E") {

                    Entity? entity = instance.map.GetEntityAt(x, y);

                    if (entity == null) {

                        response = new Response(404, "The entity you're trying to remove does NOT exist. Please get real.");
                        con.Send(response.Build());

                        return;

                    }

                    if (entity.typeString != scommand[1]) {

                        response = new Response(403, "The entity you're trying to remove is not of the type that you've specified. Boo hoo.");
                        con.Send(response.Build());

                        return;

                    }

                    instance.map.RemoveEntity(entity);
                    
                    response = new Response(200, "GG");
                    con.Send(response.Build());

                    return;

                }

                if (scommand[0] == "+E") {

                    Entity? entity = scommand[1] switch {

                        "FOX" => new Fox(instance, [x, y]),
                        "RABBIT" => new Rabbit(instance, [x, y], 0.1),
                        _ => null

                    };

                    if (entity == null) {

                        response = new Response(400, "Hello boyki, I think you might've messed up something. Please, fix it. Danke Schön!");
                        con.Send(response.Build());

                        return;

                    }

                    instance.map.AddNewEntity(entity);

                    response = new Response(200, "Cool.");
                    con.Send(response.Build());

                    return;

                }

            }

            /*
            
            
            
                ADD ALL OTHER POST REQUEST COMMANDS HERE THANK YOU ZRAPHY
            
            
            
            */

            response = new Response(404, "WTF?");
            con.Send(response.Build());

        }

        public void Debug(Request rq) {

            Console.WriteLine(rq.headerString + "\n\n" + rq.BODY);
            byte[] response = new Response(200, "huh").Build();
            con.Send(response);
            con.Close();

        }

    }

}