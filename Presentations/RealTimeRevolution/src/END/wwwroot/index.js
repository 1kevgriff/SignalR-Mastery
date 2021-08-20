var textBox = document.getElementById("txtBox");
var checkBox = document.getElementById("checkBox");
var btnStart = document.getElementById("btnStart");
var btnEnd = document.getElementById("btnEnd");

var connection = new signalR.HubConnectionBuilder()
                    .withUrl("/hubs/sync")
                    .build();

/* signalr events */
connection.on("syncTextBox", (txt) => {
    textBox.value = txt;
});

connection.on("syncCheckBox", (isChecked) => {
    checkBox.checked = isChecked;
});

connection.on("newUser", (user) => {
    Toastify({
        text: "Welcome " + user.name.first,
        avatar: user.picture.medium,
        duration: 3000,
        close: true,
        gravity: "top", // `top` or `bottom`
        position: "right", // `left`, `center` or `right`
        backgroundColor: "linear-gradient(to right, #00b09b, #96c93d)",
      }).showToast();
});

/* local events */
textBox.addEventListener("change", (ev) => { 
    var target = ev.target;
    connection.invoke("syncTextBox", target.value);
});

checkBox.addEventListener("change", (ev) => {
    var target = ev.target;
    connection.invoke("syncCheckBox", target.checked);
});

btnStart.addEventListener("click", (ev) => {
    connection.send("startNotify");
    Toastify({
        text: "Start notifications...",
        duration: 1000,
        close: true,
        gravity: "top", // `top` or `bottom`
        position: "right", // `left`, `center` or `right`
        backgroundColor: "linear-gradient(to right, #00b09b, #96c93d)",
      }).showToast();
});

btnEnd.addEventListener("click", (ev) => {
    connection.send("endNotify");
    Toastify({
        text: "Stopped notifications...",
        duration: 1000,
        close: true,
        gravity: "top", // `top` or `bottom`
        position: "right", // `left`, `center` or `right`
        backgroundColor: "linear-gradient(to right, #00b09b, #96c93d)",
      }).showToast();
});

connection.start();