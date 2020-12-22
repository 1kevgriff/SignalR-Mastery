import * as signalR from "@microsoft/signalr";


let btn = document.getElementById("btnGetFullName");

// create connection
let connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub/stringtools")
    .build();

btn.addEventListener("click", function (evt) {
    var firstName = (document.getElementById("inputFirstName") as HTMLInputElement).value;
    var lastName = (document.getElementById("inputLastName") as HTMLInputElement).value;

    connection
        .invoke("getFullName", firstName, lastName)
        .then((name: string) => { alert(name); });
});

// start the connection
function startSuccess() {
    console.log("Connected.");
}
function startFail() {
    console.log("Connection failed.");
}

connection.start().then(startSuccess, startFail);