import * as signalR from "@microsoft/signalr";

var counter = document.getElementById("viewCounter");

// create connection
let connection = new signalR.HubConnectionBuilder()
    .withAutomaticReconnect()
    .withUrl("/hub/view")
    .build();

// on view update message from client
connection.on("viewCountUpdate", (value: number) => {
    counter.innerText = value.toString();
});

// notify server we're watching
function notify() {
    connection.send("notifyWatching");
}

// start the connection
function startSuccess() {
    console.log("Connected.");
    notify();
}
function startFail() {
    console.log("Connection failed.");
}

connection.start().then(startSuccess, startFail);

// *** CONNECTION EVENTS ***
let body = document.getElementsByTagName("body")[0];

connection.onreconnected((connectionId: string) => {
    body.style.backgroundColor = "green";

    setTimeout(() => {
        body.style.backgroundColor = "white";
    }, 5000);
});

connection.onreconnecting((error: Error) => {
    body.style.backgroundColor = "yellow";
});

connection.onclose((error: Error) => {
    body.style.backgroundColor = "red";
});