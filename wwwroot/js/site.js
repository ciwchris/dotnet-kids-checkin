var webSocket;
$().ready(function () {
    var scheme = location.host.startsWith('localhost')
        ? 'ws://'
        : 'wss://';
    webSocket = new WebSocket(scheme + location.host);
    //webSocket = new WebSocket("wss://kids-checkin.azurewebsites.net");
    
    webSocket.onopen = function () {
        console.log('connected');
    };
    webSocket.onmessage = function (evt) {
        var classes = JSON.parse(evt.data);
        for (var i = 0; i < classes.length; i++) {
            var text = classes[i].count < classes[i].max ? 'Open' : 'Full';
            $('#'+ classes[i].color).text(text);
        }
    };
    webSocket.onclose = function () {
        console.log('disconnected');
    };
});