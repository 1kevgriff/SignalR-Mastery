import * as signalR from "@microsoft/signalr";

var currentTime = document.getElementById("currentTime");

// create connection
let connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub/time")
    .build();

// on view update message from client
connection.on("updateCurrentTime", (value: number) => {
    currentTime.innerText = value.toString();
});

// start the connection
function startSuccess(){
    console.log("Connected.");
}
function startFail(){
    console.log("Connection failed.");
}

connection.start().then(startSuccess, startFail);