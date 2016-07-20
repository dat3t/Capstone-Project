
function showProfileForm(pUserId) {

    var param = {
        userId: pUserId
    };
    $.ajax({
        type: 'GET',
        url: 'ShowProfile',
        data: param,
        success: function (partialResult) {
            $("#ShowProfile").html(partialResult);
            $("#EditProfile").html("");
            $("#btnEdit").show();
        }

    });
}

function showEditProfileForm() {           
    $.ajax({
        type: 'GET',
        url: 'EditProfile',        
        success: function (partialResult) {
            $("#EditProfile").html(partialResult);
            $("#ShowProfile").html("");
            $("#btnEdit").hide();

        },
        error: function (error) {
            alert(error);
        }
    });
}

function submitEditProfile() {
    $("#ShowProfile").html("");
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
            $("#ShowProfile").html(partialResult);
            $("#EditProfile").html("");
            $("#btnEdit").show();
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
    $.ajax({
        type: 'POST',
        url: 'EnableTwoFactorAuthentication',
        success: function(partialResult) {
            if (partialResult !== null | partialResult !== "") {
                $("#ShowTwoFactorAuthen").html("");
                $("#ShowTwoFactorAuthen").html(partialResult);
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