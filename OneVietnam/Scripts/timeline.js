﻿    
function editableForm() {
    $('.tog').toggleClass('disabled');
    $('#btnSave').toggleClass('hides');
    $('#btnCancel').toggleClass('hides');
    $("#btnEditProfile").hide();
    $("#lblGender").hide();
    $("#drdGender").show();
    $("#btnUpdateLocation").toggleClass('hides');
    $("#drdGender").dropdown({});
}

function submitEditProfile() {
    var oldName = document.getElementById("lblTimeLineUserName");
    var oldHeaderName = document.getElementById("txtHeaderUserName");
    var currentName = document.getElementById("UserName");
    
    oldName.innerText = currentName.value;
    oldHeaderName.innerText = currentName.value;
}

function cancelEditProfile() {
    $.ajax({
        type: 'GET',
        url: 'EditProfile',        
        success: function (partialResult) {            
            $("#EditProfileForm").html("");
            $("#EditProfileForm").html(partialResult);
        }
    });
}

function changeTwoFactorAuthentication() {
    var param = $(".ui.toggle.button").text();
    $.ajax({
        type: 'POST',
        url: 'ChangeTwoFactorAuthentication',
        data:{'value':param},
        success: function() {                           
        }
    });
}

function showChangePasswordForm() {    
    $.ajax({
        type: 'GET',
        url: 'ChangePassword',       
        success: function (partialResult) {
            $("#ChangePasswordForm").html(partialResult);
            $("#ShowPassword").html("");
            $("#btnChangePass").hide();
        }        
    });
}

function cancelChangePassword() {
    $("#ChangePasswordForm").html("");
    $("#ShowPassword").html("Thay đổi mật khẩu thường xuyên để nâng cao bảo mật hơn");
    $("#btnChangePass").show();
}


function closeChangePasswordForm() {
    var errorMsg = document.querySelector(".validation-summary-errors");
    if (errorMsg === null)
    {
        $("#ChangePasswordForm").html("");
        $("#ShowPassword").html("Thay đổi mật khẩu thường xuyên để nâng cao bảo mật hơn");
        $("#btnChangePass").show();
    }
    
}

function updateCurrentLocation(x,y,address) {
    var addr = document.getElementById("Location_Address");
    var xcoordinate = document.getElementById("Location_XCoordinate");
    var ycoordinate = document.getElementById("Location_YCoordinate");

    

    addr.value = "";
    xcoordinate.value = "";
    ycoordinate.value = "";

}

function showSetPasswordForm() {    
    $.ajax({
        type: 'GET',
        url: 'SetPassword',               
        success: function (partialResult) {
            $("#ChangePasswordForm").html(partialResult);
            $("#ShowPassword").html("");
            $("#btnChangePass").hide();
        }        
    });
}

function cancelSetPassword() {
    $("#ChangePasswordForm").html("");
    $("#ShowPassword").html("Tạo mật khẩu mới đê đăng nhập bằng email");
    $("#btnChangePass").show();
}

function closeSetPasswordForm() {  
    var errorMsg = document.querySelector(".validation-summary-errors");
    if (errorMsg === null) {
        $("#ChangePasswordForm").html("");
        $("#ShowPassword").html("Thay đổi mật khẩu thường xuyên để nâng cao bảo mật hơn");
        $("#btnChangePass").show();
        $("#btnChangePass").val('Đổi mật khẩu');
        $("#btnChangePass")
            .click(function () {
                showChangePasswordForm();
            });
    }
}

function showUserMarkerOnMap(x,y,address) {
    //Declare icon for userLocationmarker
    var icon = {
        url: "../Content/Icon/location.png",
        size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(25, 25)
    };

    // Declare a myLocation marker using icon declared above, and bind it to the map
    var userLocationMarker = new google.maps.Marker({
        title: address,
        icon: icon
    });
    var map = new google.maps.Map(document.getElementById('divShowMap'), {
        center: { lat: x, lng: y},
        zoom: 10,
        minZoom: 4,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    });

    userLocationMarker.setPosition({ lat: x, lng: y });
    userLocationMarker.setMap(map);
}
