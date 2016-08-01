function pushNotification(url, title,id) {
    $.connection.hub.start().done(function () {                
        chat.server.pushNotification(url,title,id);        
    });
}
$('#myNotifications').click(function () {
    $('#NotificationsNo').text('0');
    $('#NotificationsNo').hide();
    getNotifications();
});
function getNotifications() {
    var controller = "/Home/GetNotifications";
    $.ajax({
        type: 'POST',
        url: controller,
        success: function(notifications) {
            if (notifications == '') {
                return;
            }
            document.getElementById('Notifications').innerHTML = "";
            for (var i = notifications.length - 1; i >= 0; i--) {
                var not;
                if (notifications[i]["Seen"] == false) {
                    not = '<div class="item notifications" id="' + notifications[i]["Id"] + '" style="padding:0em!important; background: #E0F2F1 !important;" >';
                } else {
                    not = '<div class="item notifications" id="' + notifications[i]["Id"] + '" style="padding:0em!important" >';
                }
                not = not + '<a class="item" href="' + notifications[i]["Url"] + '" style="padding: 0 !important">';
                not = not + '<div class="item">';
                not = not + '<div class="ui mini image" style="margin-right: 1em !important">';
                not = not + '<img class="ui mini image" src="' + notifications[i]["Avatar"] + '"/>';
                not = not + '</div>';
                not = not + '<div class="content">';
                not = not + '<div class="description notification">';
                not = not + '<p>';
                not = not + notifications[i]["Description"];
                not = not + '</p>';
                not = not + '</div>';
                not = not + '<div class="extra">';
                not = not + '<i>' + notifications[i]["CreatedDate"] + '</i>';
                not = not + '</div>';
                not = not + '</div>';
                not = not + '</div>';
                not = not + '</a>';
                not = not + '<a>';
                not = not + '<i class="minus circle red icon"></i>';
                not = not + '</a>';
                not = not + '</div>';
                $("#Notifications").append(not);
            }
            
        }
    });
}