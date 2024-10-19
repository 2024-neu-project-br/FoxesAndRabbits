/* --- Define basic variables and values --- */
const game = document.getElementById("game");
const tickInterval = document.getElementById("tickInterval").value;
const gameInstanceName = document.getElementById("gameInstanceName").value;
const pauseGame = document.getElementById("pauseGame");
const newGame = document.getElementById("newGame");
pauseGame.toggleAttribute("disabled");

pauseGame.onclick = () => {
    // This checks whether the game is playing or not and toggles that attribute based on that
    if(getAttr(game, "playing")) game.removeAttribute("playing", "");
    else game.setAttribute("playing", "");

    // Updating the buttons
    newGame.toggleAttribute("disabled");
    pauseGame.innerText = getAttr(game, "playing") ? "Pause game" : "Unpause game";
    
    /* THIS IS FOR TESTING PURPOSES ONLY, IT WILL BE REMOVED IN THE FINAL RELEASE */
    console.log(`Game paused!\nGame started: ${getAttr(game, "started")}\nGame playing: ${getAttr(game, "playing")}`);
}

newGame.onclick = () => {
    // Checks if the pause button is disabled or not, enables it if yes
    if(getAttr(pauseGame, "disabled")) pauseGame.removeAttribute("disabled");

    // Setting basic attributes and updating the pause button
    game.setAttribute("started", "");
    game.setAttribute("playing", "");
    pauseGame.innerText = getAttr(game, "playing") ? "Pause game" : "Unpause game";

    // Disables the new game button until the game gets paused
    newGame.toggleAttribute("disabled");
    
    /* THIS IS FOR TESTING PURPOSES ONLY, IT WILL BE REMOVED IN THE FINAL RELEASE */
    console.log(`New game started!\nGame started: ${getAttr(game, "started")}\nGame playing: ${getAttr(game, "playing")}`);
}


/* This function just helps shortening the code a bit */
function getAttr(element, attribute){
    return element.attributes.getNamedItem(attribute) != null;
}

/* This function parses a map that got returned from a successful request */
function parseMap(mapString){
    // Split the response into lines
    let lines = mapString.split("\n").filter(x=>x);

    // Create an object and parse the data
    let map = {}    
    map.grassMap = lines.slice(lines.findIndex(x=>x=="GRASSMAP")+1, lines.findIndex(x=>x=="MOBMAP"));
    map.mobMap = lines.slice(lines.findIndex(x=>x=="MOBMAP")+1, lines.length-1);
    
    return map;
}