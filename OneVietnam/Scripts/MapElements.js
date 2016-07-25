var isOpen = true;
function toggle() {
    if (isOpen == true) {
        closeNav();
        isOpen = false;
        document.getElementById("button").className = "arrow right icon";
        //document.getElementById("aa").setAttribute("data-tooltip", "Mở các bài đăng");
    } else {
        openNav();
        isOpen = true;
        document.getElementById("button").className = "arrow left icon";
        //document.getElementById("aa").setAttribute("data-tooltip", "Đóng các bài đăng");
    }
}
function openNav() {
    document.getElementById("mySidenav").style.width = "380px";
    document.getElementById("aa").style.marginLeft = "380px";
}

function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
    document.getElementById("aa").style.marginLeft = "0px";
}

function accommodationEnlarge(){
    document.getElementById("filterHome").style.background = "url(../Content/Icon/home.png)";
    document.getElementById("filterHome").style.backgroundSize = "100%";
    document.getElementById("filterHome").style.backgroundRepeat = "no-repeat";
    document.getElementById("filterHome").className += "disabled";
}

function jobEnlarge() {
    document.getElementById("filterJob").style.background = "url(../Content/Icon/job5.png)";
    document.getElementById("filterJob").style.backgroundSize = "100%";
    document.getElementById("filterJob").style.backgroundRepeat = "no-repeat";
    document.getElementById("filterJob").className += "disabled";
}
