import * as signalR from "@microsoft/signalr";

var body = (document.getElementsByTagName("body")[0]) as HTMLElement;
var btnRed = document.getElementById("btnRed") as HTMLElement;
var btnGreen = document.getElementById("btnGreen") as HTMLElement;
var btnBlue = document.getElementById("btnBlue") as HTMLElement;

// create connection
let connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub/color")
    .build();

// on background change update message from client
connection.on("changeBackground", (value: string) => {
    body.style.backgroundColor = value;
});

function onRed() {
    connection.invoke("changeBackground", "red").then(() => { }, (error) => { alert(error); });
}

function onGreen() {
    connection.invoke("changeBackground", "green").then(() => { }, (error) => { alert(error); });
}

function onBlue() {
    connection.invoke("changeBackground", "blue").then(() => { }, (error) => { alert(error); });
}

// start the connection
function startSuccess() {
    console.log("Connected.");
}
function startFail() {
    console.log("Connection failed.");
}

btnRed.onclick = onRed;
btnGreen.onclick = onGreen;
btnBlue.onclick = onBlue;

connection.start().then(startSuccess, startFail);