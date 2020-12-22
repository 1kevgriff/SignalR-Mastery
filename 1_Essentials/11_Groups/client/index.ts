import * as signalR from "@microsoft/signalr";

let btnJoinYellow = document.getElementById("btnJoinYellow");
let btnJoinBlue = document.getElementById("btnJoinBlue");
let btnJoinOrange = document.getElementById("btnJoinOrange");
let btnTriggerYellow = document.getElementById("btnTriggerYellow");
let btnTriggerBlue = document.getElementById("btnTriggerBlue");
let btnTriggerOrange = document.getElementById("btnTriggerOrange");

// create connection
let connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub/color")
    .build();

btnJoinYellow.addEventListener("click", () => { connection.invoke("JoinGroup", "Yellow"); });
btnJoinBlue.addEventListener("click", () => { connection.invoke("JoinGroup", "Blue"); });
btnJoinOrange.addEventListener("click", () => { connection.invoke("JoinGroup", "Orange"); });

btnTriggerYellow.addEventListener("click", () => { connection.invoke("TriggerGroup", "Yellow"); });
btnTriggerBlue.addEventListener("click", () => { connection.invoke("TriggerGroup", "Blue"); });
btnTriggerOrange.addEventListener("click", () => { connection.invoke("TriggerGroup", "Orange"); });

// client events
connection.on("triggerColor", (color) => {
    document.getElementsByTagName("body")[0].style.backgroundColor = color;
});

// start the connection
function startSuccess() {
    console.log("Connected.");
    connection.invoke("IncrementServerView");
}
function startFail() {
    console.log("Connection failed.");
}

connection.start().then(startSuccess, startFail);