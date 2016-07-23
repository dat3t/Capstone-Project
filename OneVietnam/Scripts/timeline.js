
function showProfileForm(pUserId) {

    var param = {
        userId: pUserId
    };
    $.ajax({
        type: 'GET',
        url: 'EditProfile',
        data: param,
        success: function (partialResult) {
            $("#EditProfile").html(partialResult);
        }

    });
}


    
function editableForm() {
   $('.tog').toggleClass('disabled');
    $('#btnSave').toggleClass('hides');
}

function submitEditProfile() {
    var param = {        
        gender: $("#Gender").val(),
        email: $("#Email").val(),
        phone: $("#PhoneNumber").val(),
        address: $("#Address_Address").val()
};
    $.ajax({
        type: 'POST',
        url: 'EditProfile',
        data: param,
        success: function (partialResult) {
//            $("#EditProfile").html(partialResult);
//            $("#btnEdit").show();
        }
    });
}

function showTwoFactorAuthen(pUserId) {
    var param = {
        userId: pUserId
    };
    $.ajax({
        type: 'GET',
        url: 'ShowTwoFactorAuthen',
        data: param,
        success: function (partialResult) {
            $("#ShowTwoFactorAuthen").html(partialResult);
            $("#EditTwoFactorAuthen").html("");            
        }
    });
}

function enableTwoFactorAuthentication() {
    var param = $(".ui.toggle.button").text();
    $.ajax({
        type: 'POST',
        url: 'EnableTwoFactorAuthentication',
        data:{'value':param},
        success: function(partialResult) {
            if (partialResult !== null | partialResult !== "") {
//                $("#ShowTwoFactorAuthen").html("");
//                $("#ShowTwoFactorAuthen").html(partialResult);
            }                
        }
    });
}

function disableTwoFactorAuthentication() {
    $.ajax({
        type: 'POST',
        url: 'DisableTwoFactorAuthentication',
        success: function (partialResult) {
            if (partialResult !== null | partialResult !== "") {
                $("#ShowTwoFactorAuthen").html("");
                $("#ShowTwoFactorAuthen").html(partialResult);
            }
        }
    });
}

function showChangePasswordForm() {    
    $.ajax({
        type: 'GET',
        url: 'ChangePassword',        
        success: function (partialResult) {
            $("#ChangePassword").html(partialResult);
            $("#ShowPassword").html("");
            $("#btnChangePass").hide();
        }
    });
}

function cancelChangePassword() {
    $("#ChangePassword").html("");
    $("#ShowPassword").html("<div>Thay đổi mật khẩu thường xuyên để nâng cao bảo mật hơn</div>");
    $("#btnChangePass").show();
}

function changePassword() {
    var param = {
        oldPass: $("#OldPassword").val(),
        newPass: $("#NewPassword").val(),
        confirmPass: $("#ConfirmPassword").val()
    };
    $.ajax({
        type: 'POST',
        url: 'ChangePassword',
        data: param,
        success: function (partialResult) {
            if (partialResult === null | partialResult === "") {
                $("#ShowPassword").html("<div>Thay đổi mật khẩu thường xuyên để nâng cao bảo mật hơn</div>");
                $("#ChangePassword").html("");                
                $("#btnChangePass").show();                
            } else {
                $("#ChangePassword").html(partialResult);
                $("#ShowPassword").html("");
                $("#btnChangePass").hide();
            }
            
            
       }
    });
}