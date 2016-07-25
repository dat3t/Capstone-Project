    
function editableForm() {
   $('.tog').toggleClass('disabled');
   $('#btnSave').toggleClass('hides');
   $('#btnCancel').toggleClass('hides');
   $("#btnEditProfile").hide();
   $("#lblGender").hide();
   $("#drdGender").show();
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

function showChangePasswordForm(userId) {
    var auth = btoa("userId : " + userId);
    $.ajax({
        type: 'GET',
        url: 'ChangePassword',
        headers: {
            "Authorization": "Basic " + auth
        },
        //beforeSend: function (xhr) {
        //    xhr.setRequestHeader('Authorization', make_base_auth());
        //},
        success: function (partialResult) {
            $("#ChangePasswordForm").html(partialResult);
            $("#ShowPassword").html("");
            $("#btnChangePass").hide();
        },
        error: function(errorMessage) {
            alert(errorMessage);
        }
    });
}

function cancelChangePassword() {
    $("#ChangePasswordForm").html("");
    $("#ShowPassword").html("<div class='sub header'>Thay đổi mật khẩu thường xuyên để nâng cao bảo mật hơn</div>");
    $("#btnChangePass").show();
}
