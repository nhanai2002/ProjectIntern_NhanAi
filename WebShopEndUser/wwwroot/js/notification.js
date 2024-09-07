"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:7139/notificationHub").build();
connection.start()
    .then(function () {
        console.log('connected to hub');
    })
    .catch(function (err) {
        return console.error(err.toString());
    });


connection.on("OnConnected", function () {
    OnConnected();
});

function OnConnected() {
    var userId = $('#hfUserId').val();
    connection.invoke("SaveUserConnection", userId).catch(function (err) {
        return console.error(err.toString());
    });
}

connection.on("ReceivedNotification", function (data) {
    //alert(data);
    addNotification(data);

    //DisplayGeneralNotification(message, 'General Message');
});

//connection.on("ReceivedPersonalNotification", function (message, username) {
//     alert(message + ' - ' + username);
//    //DisplayPersonalNotification(message, 'hey ' + username);
//});
