import * as signalR from "@microsoft/signalr";
import { MessagePackHubProtocol} from "@microsoft/signalr-protocol-msgpack"; 

let btnGetOne = document.getElementById("btnGetOne");
let btnGetTen = document.getElementById("btnGetTen");
let btnGetOneThousand = document.getElementById("btnGetOneThousand");
let userJson = document.getElementById("userJson") as HTMLTextAreaElement;

function receiveUsers(users) {
    userJson.value = JSON.stringify(users, null, 2);
}
function clear() {
    userJson.value = "Loading...";
}

btnGetOne.addEventListener("click", () => { clear(); connection.invoke("GetUsers", 1).then(receiveUsers); });
btnGetTen.addEventListener("click", () => { clear(); connection.invoke("GetUsers", 10).then(receiveUsers); });
btnGetOneThousand.addEventListener("click", () => { clear(); connection.invoke("GetUsers", 1000).then(receiveUsers); });

// create connection
let connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Trace)
    .withHubProtocol(new MessagePackHubProtocol())
    .withUrl("/hub/users")
    .build();

// client events

// start the connection
function startSuccess() {
    console.log("Connected.");
}
function startFail() {
    console.log("Connection failed.");
}

connection.start().then(startSuccess, startFail);