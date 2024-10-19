var currentInstance = "<empty>";

function newInstance(name, width, height, isMapBlank) {

    currentInstance = name;

    fetch("/newInstance", {
        
        method: "POST",
        body: `name=${currentInstance}\nwidth=${width}\nheight=${height}\nisMapBlank=${isMapBlank ? "true" : "false"}`
    
    }).then(async (response) => {

        if (response.status == 200) {

            var gameState = await response.text();

            /*
            
                gameState is the variable that contains all the information that has to/can be rendered
            
            */

        }

        if (response.status == 403) {

            /*
            
                if this is triggered it means the user tried creating an instance with a name that already exists
            
            */

        }

    });

}

function tick() { // this function is run by the client, meaning here in javascript at a rate defined by the user themselves, when the function is run, it tells the server to update the current state of the game to the new one and it responds with that

    fetch("/tick", {
        
        method: "POST",
        body: `name=${currentInstance}`
    
    }).then(async (response) => {

        if (response.status == 200) {

            var gameState = await response.text();

            /*
            
                gameState is the variable that contains all the information that has to/can be rendered
            
            */

        }

    });

}

/*

    the value of the gameState in both the tick() function and the newInstance() function is structured the same way
    the data is always a string, its split into two sections: GRASSMAP, MOBMAP
    an example is provided below:

startOfExample

GRASSMAP
2;3;3;3;2;3;3;3;1;2;3;3;1
2;3;3;3;3;2;2;3;3;2;3;3;3
3;1;2;3;3;3;2;2;3;3;3;2;2
3;3;2;3;3;3;2;1;3;2;3;3;3
3;3;3;3;1;2;3;3;3;3;1;3;2
2;3;1;1;2;3;3;3;3;3;3;3;3
3;3;3;3;1;1;3;3;2;3;3;2;1

MOBMAP
FOX@-@5 2
FOX@-@3 6
FOX@-@7 9
FOX@-@10 5
RABBIT@-@2 2
RABBIT@-@7 7
RABBIT@-@9 6
RABBIT@-@2 6

endOfExample

    remember, all of this is one string, so you have to format it in such a way

*/

const toggle = () => command("T"); // toggles the game (PAUSE/PLAY)

const addEntity = (type, x, y) => command("+E " + type + " " + x + " " + y);

const removeEntity = (type, x, y) => command("-E " + type + " " + x + " " + y);

function command(command) {

    /*
    
        i will work on this later
    
    */

    fetch("/command", {

        method: "POST",
        body: `name=${currentInstance}\ncommand=${command}`

    });

}