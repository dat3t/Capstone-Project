function pushNotification(url, title,id) {
    $.connection.hub.start().done(function () {                
        chat.server.pushNotification(url,title,id);        
    });
}