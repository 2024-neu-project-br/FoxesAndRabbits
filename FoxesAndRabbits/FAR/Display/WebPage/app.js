/* --- Define basic variables and values --- */
const testMode = true;

const game = document.getElementById("game");
const tickIntervalInput = document.getElementById("tickInterval");
const gameInstanceNameInput = document.getElementById("gameInstanceName")
const mapWInput = document.getElementById("mapW");
const mapHInput = document.getElementById("mapH");

let tickInterval = !tickIntervalInput.value ? 1000 : tickIntervalInput.value;
let gameInstanceName = !gameInstanceNameInput.value ? "FAR" : gameInstanceNameInput.value;
let mapW = !mapWInput.value ? 30 : mapWInput.value;
let mapH = !mapHInput.value ? 15 : mapHInput.value;

tickIntervalInput.onchange = () => tickInterval = !tickIntervalInput.value ? 1000 : tickIntervalInput.value;
gameInstanceNameInput.onchange = () => gameInstanceName = !gameInstanceNameInput.value ? "FAR" : gameInstanceNameInput.value;
mapWInput.onchange = () => mapW = !mapWInput.value ? 30 : mapWInput.value;
mapHInput.onchange = () => mapH = !mapHInput.value ? 15 : mapHInput.value;

const pauseGame = document.getElementById("pauseGame");
const newGame = document.getElementById("newGame");
const addFox = document.getElementById("addFox");
const addRabbit = document.getElementById("addRabbit");
const newGameDialog = document.getElementById("newGameDialog");
const newGameToggle = document.getElementById("newGameToggle");

let tickSetInterval;

// There's no instance yet, so you can't do much ¯\_(ツ)_/¯
[pauseGame, addFox, addRabbit].forEach(element => element.toggleAttribute("disabled"));

newGameToggle.onclick = () => {
    newGameDialog.showModal()
}

newGame.onclick = async () => {
    // Checks if these buttons are disabled or not, enables them if yes
    [pauseGame, addFox, addRabbit].forEach(element => {if(getAttr(element, "disabled")) element.removeAttribute("disabled");});

    // Setting basic attributes and updating the pause button
    game.setAttribute("started", "");
    game.setAttribute("playing", "");
    pauseGame.innerText = getAttr(game, "playing") ? "Pause game" : "Unpause game";

    // Disables the new game button until the game gets paused
    newGameToggle.toggleAttribute("disabled");
    newGameDialog.close()

    handleResponse(await newInstance(gameInstanceName, mapW, mapH, false));
    tickSetInterval = setInterval(async () => {handleResponse(await tick()); if(testMode) console.log(`Tick!\nGame data:\ngameInstanceName: ${gameInstanceName}\nmapW: ${mapW}\nmapH: ${mapH}\ntickInterval: ${tickInterval}\nGame started: ${getAttr(game, "started")}\nGame playing: ${getAttr(game, "playing")}`);}, tickInterval);

    /* THIS IS FOR TESTING PURPOSES ONLY, IT WILL BE REMOVED IN THE FINAL RELEASE */
    if(testMode) console.log(`New game started!\nGame data:\ngameInstanceName: ${gameInstanceName}\nmapW: ${mapW}\nmapH: ${mapH}\ntickInterval: ${tickInterval}\nGame started: ${getAttr(game, "started")}\nGame playing: ${getAttr(game, "playing")}`);
}

pauseGame.onclick = async () => {
    // This checks whether the game is playing or not and toggles that attribute based on that
    if(getAttr(game, "playing")){
        game.removeAttribute("playing", "");
        clearInterval(tickSetInterval);
    } else{
        game.setAttribute("playing", "");
        tickSetInterval = setInterval(async () => {handleResponse(await tick()); if(testMode) console.log(`Tick!\nGame data:\ngameInstanceName: ${gameInstanceName}\nmapW: ${mapW}\nmapH: ${mapH}\ntickInterval: ${tickInterval}\nGame started: ${getAttr(game, "started")}\nGame playing: ${getAttr(game, "playing")}`);}, tickInterval);
    }

    // Updating the buttons
    [newGameToggle, addFox, addRabbit].forEach(element => element.toggleAttribute("disabled"))
    pauseGame.innerText = getAttr(game, "playing") ? "Pause game" : "Unpause game";

    handleResponse(await toggle());

    /* THIS IS FOR TESTING PURPOSES ONLY, IT WILL BE REMOVED IN THE FINAL RELEASE */
    if(testMode) console.log(`Game paused!\nGame data:\ngameInstanceName: ${gameInstanceName}\nmapW: ${mapW}\nmapH: ${mapH}\ntickInterval: ${tickInterval}\nGame started: ${getAttr(game, "started")}\nGame playing: ${getAttr(game, "playing")}`);
}

// Spawns an entity on the map upon clicking the button
addFox.onclick = () => handleResponse(command("+E")/* Fox */)
addRabbit.onclick = () => handleResponse(command("+E")/* Rabbit */)

/* This function just helps shortening the code a bit */
getAttr = (element, attribute) => element.attributes.getNamedItem(attribute) != null;

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
async function handleResponse(response){
    //if(testMode) console.log(`Allah said: ${response}`)

    /* THIS IS FOR TESTING PURPOSES ONLY, IT WILL BE REMOVED IN THE FINAL RELEASE */
    let exampleString = "startOfExample\n\nGRASSMAP\n2;3;3;3;2;3;3;3;1;2;3;3;1\n2;3;3;3;3;2;2;3;3;2;3;3;3\n3;1;2;3;3;3;2;2;3;3;3;2;2\n3;3;2;3;3;3;2;1;3;2;3;3;3\n3;3;3;3;1;2;3;3;3;3;1;3;2\n2;3;1;1;2;3;3;3;3;3;3;3;3\n3;3;3;3;1;1;3;3;2;3;3;2;1\n\nMOBMAP\nFOX@-@5 2\nFOX@-@3 6\nFOX@-@7 9\nFOX@-@10 5\nRABBIT@-@2 2\nRABBIT@-@7 7\nRABBIT@-@9 6\nRABBIT@-@2 6\n\nendOfExample";
    
    /* Creating a new instance in case the gameInstance with the same name would exist */
    if(response == "Instance already exists.") {
        gameInstanceName = createID(10);
        handleResponse(await newInstance(gameInstanceName, mapW, mapH, false))
    }

    // If the response isn't an error, then draw the map
    if(response.includes("GRASSMAP")) drawMap(parseMap(response));
}

const entityEmojis = {
    FOX: "🦊",
    RABBIT: "🐰",
}

/* This function "draws" the map */
function drawMap(mapDict){
    game.innerHTML = "";
    for (let i = 0; i < mapDict.grassMap.length; i++) {
        let blocks = document.createElement("div")
        blocks.classList.add("line");
        for (let j = 0; j < mapDict.grassMap[i].split(";").length; j++) {
            let block = document.createElement("div");
            if(testMode) block.textContent = `${j};${i}`
            block.classList.add(`grass-${mapDict.grassMap[i].split(";")[j]}`);
            mapDict.mobMap.forEach(entity => {
                let entityData = entity.split("@-@");
                let entityType = entityData[0];
                let entityPos = entityData[1].split(" ");
                if(entityPos[1] == i && entityPos[0] == j) block.textContent = entityEmojis[entityType];
            });
            blocks.append(block);
        }
        game.append(blocks);
    }
}

/* This function creates an ID in case the gameInstance with the same name would exist */
function createID (length){
    let result = '';
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    const charactersLength = characters.length;
    let counter = 0;
    while (counter < length) {
        result += characters.charAt(Math.floor(Math.random() * charactersLength));
        counter += 1;
    }
    return result;
}