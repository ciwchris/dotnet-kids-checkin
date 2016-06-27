var webSocket;
$().ready(function () {
    webSocket = new WebSocket("ws://localhost:5000");
    webSocket.onopen = function () {
        console.log("connected");
    };
    webSocket.onmessage = function (evt) {
        console.log(evt.data);
    };
    webSocket.onclose = function () {
        console.log("disconnected");
    };
});