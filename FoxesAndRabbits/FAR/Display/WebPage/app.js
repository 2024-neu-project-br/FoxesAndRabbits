/* --- Define basic variables and values --- */
const game = document.getElementById("game");
const tickInterval = !document.getElementById("tickInterval").value ? 10 : document.getElementById("tickInterval").value;
const gameInstanceName = !document.getElementById("gameInstanceName").value ? "FAR" : document.getElementById("gameInstanceName").value;
const pauseGame = document.getElementById("pauseGame");
const newGame = document.getElementById("newGame");
const addFox = document.getElementById("addFox");
const addRabbit = document.getElementById("addRabbit");

// There's no instance yet, so you can't do much ¯\_(ツ)_/¯
[pauseGame, addFox, addRabbit].forEach(element => element.toggleAttribute("disabled"));

pauseGame.onclick = () => {
    // This checks whether the game is playing or not and toggles that attribute based on that
    if(getAttr(game, "playing")) game.removeAttribute("playing", "");
    else game.setAttribute("playing", "");

    // Updating the buttons
    [newGame, addFox, addRabbit].forEach(element => element.toggleAttribute("disabled"))
    pauseGame.innerText = getAttr(game, "playing") ? "Pause game" : "Unpause game";

    handleResponse(command("T"));

    /* THIS IS FOR TESTING PURPOSES ONLY, IT WILL BE REMOVED IN THE FINAL RELEASE */
    console.log(`Game paused!\nGame started: ${getAttr(game, "started")}\nGame playing: ${getAttr(game, "playing")}`);
}

newGame.onclick = () => {
    // Checks if these buttons are disabled or not, enables them if yes
    [pauseGame, addFox, addRabbit].forEach(element => {if(getAttr(element, "disabled")) element.removeAttribute("disabled");});

    // Setting basic attributes and updating the pause button
    game.setAttribute("started", "");
    game.setAttribute("playing", "");
    pauseGame.innerText = getAttr(game, "playing") ? "Pause game" : "Unpause game";

    // Disables the new game button until the game gets paused
    newGame.toggleAttribute("disabled");
    
    handleResponse(newInstance());

    /* THIS IS FOR TESTING PURPOSES ONLY, IT WILL BE REMOVED IN THE FINAL RELEASE */
    console.log(`New game started!\nGame started: ${getAttr(game, "started")}\nGame playing: ${getAttr(game, "playing")}`);
}

// Spawns an entity on the map upon clicking the button
addFox.onclick = () => handleResponse(command("+E")/* Fox */)
addRabbit.onclick = () => handleResponse(command("+E")/* Rabbit */)

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

/* This doesn't do anything yet, just console.logs the response, later it will actually handle the response it gets */
function handleResponse(response){
    console.log(response);
}