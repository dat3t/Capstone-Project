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

function helpEnlarge() {
    enlargeIcon("filterHelp", "url(../Content/Icon/help2.png)");
}

function accommodationEnlarge(){
    enlargeIcon("filterHome", "url(../Content/Icon/home2.png)");
}

function jobEnlarge() {
    enlargeIcon("filterJob", "url(../Content/Icon/job2.png)");
  
}

function furnitureEnlarge() {
    enlargeIcon("filterFurnitureOffer", "url(../Content/Icon/free2.png)");
}

function handGoodsEnlarge() {
    enlargeIcon("filterHandGoodsOffer", "url(../Content/Icon/ship2.png)");
}

function tradeEnlarge() {
    enlargeIcon("filterSalesOffer", "url(../Content/Icon/sale2.png)");
}

function warningEnlarge() {
    enlargeIcon("filterWarning", "url(../Content/Icon/warning2.png)");
}

function maleEnlarge() {
    enlargeIcon("filterMale", "url(../Content/Icon/male2.png)");
}

function femaleEnlarge() {
    enlargeIcon("filterFemale", "url(../Content/Icon/female2.png)");
}

function LGBTEnlarge() {
    enlargeIcon("filterLGBT", "url(../Content/Icon/LGBT2.png)");
}

function userEnlarge() {
    enlargeIcon("filterUsers", "url(../Content/Icon/users2.png)");
}

function currentLocationEnlarge() {
    enlargeIcon("location", "url(../Content/Icon/location2.png)");
}

function myLocationEnlarge() {
    enlargeIcon("myLocation", "url(../Content/Icon/myhome2.png)");
}

function enlargeIcon(id, link) {
    returnToNormalState();
    document.getElementById(id).style.background = link;
    document.getElementById(id).style.backgroundSize = "100%";
}

function returnToNormalState() {

    document.getElementById("filterHelp").style.background = "url(../Content/Icon/help.png)";
    document.getElementById("filterHome").style.background = "url(../Content/Icon/home.png)";
    document.getElementById("filterJob").style.background = "url(../Content/Icon/job.png)";
    document.getElementById("filterFurnitureOffer").style.background = "url(../Content/Icon/free.png)";
    document.getElementById("filterHandGoodsOffer").style.background = "url(../Content/Icon/ship.jpg)";
    document.getElementById("filterSalesOffer").style.background = "url(../Content/Icon/sale.png)";
    document.getElementById("filterWarning").style.background = "url(../Content/Icon/warning.png)";
    document.getElementById("filterMale").style.background = "url(../Content/Icon/male.png)";
    document.getElementById("filterFemale").style.background = "url(../Content/Icon/female.png)";
    document.getElementById("filterLGBT").style.background = "url(../Content/Icon/LGBT.png)";
    document.getElementById("filterUsers").style.background = "url(../Content/Icon/users.png)";
    document.getElementById("location").style.background = "url(../Content/Icon/location.png)";
    document.getElementById("myLocation").style.background = "url(../Content/Icon/myhome.png)";

    document.getElementById("filterHelp").style.backgroundSize = "100%";
    document.getElementById("filterHome").style.backgroundSize = "100%";
    document.getElementById("filterJob").style.backgroundSize = "100%";
    document.getElementById("filterFurnitureOffer").style.backgroundSize = "100%";
    document.getElementById("filterHandGoodsOffer").style.backgroundSize = "100%";
    document.getElementById("filterSalesOffer").style.backgroundSize = "100%";
    document.getElementById("filterWarning").style.backgroundSize = "100%";
    document.getElementById("filterMale").style.backgroundSize = "100%";
    document.getElementById("filterFemale").style.backgroundSize = "100%";
    document.getElementById("filterLGBT").style.backgroundSize = "100%";
    document.getElementById("filterUsers").style.backgroundSize = "100%";
    document.getElementById("location").style.backgroundSize = "100%";
    document.getElementById("myLocation").style.backgroundSize = "100%";

}
