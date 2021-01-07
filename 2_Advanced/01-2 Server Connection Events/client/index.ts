import * as signalR from "@microsoft/signalr";

var counter = document.getElementById("viewCounter");

// create connection
let connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub/view")
    .build();

// on view update message from client
connection.on("viewCountUpdate", (value: number) => {
    counter.innerText = value.toString();
});

// start the connection
function startSuccess(){
    console.log("Connected.");
}
function startFail(){
    console.log("Connection failed.");
}

connection.start().then(startSuccess, startFail);