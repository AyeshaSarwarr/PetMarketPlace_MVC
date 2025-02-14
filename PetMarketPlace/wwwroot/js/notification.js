
    var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();
    connection.start();
    connection.on("ReceiveNotification", function (message) {
        alert(message);
    });

    
