$(document).ready(function () {
    $('.item.write')
       .click(function () {


           $('.ui.modal').modal('show');

       });

  
    $("div#myId").dropzone({ url: "/file/post" });
    $(".right.menu.open").on("click", function (e) {
        e.preventDefault();
        $(".ui.vertical.menu.open").toggle();
    });    
    $('.ui.search').search({
        apiSettings: {
            url: '/Home/search?id={query}'
        },
        fields: {
            results: 'UserList',
            title: 'UserName',
            url: '',
            Image:"Images/logo.png"
        },
        minCharacters: 3
    })
    ;

    //ThamDTH
    $('.clearing.star.rating').rating('setting', 'clearable', true);

    $('.ui.dropdown')
      .dropdown({
          allowAdditions: true
      })
    ;
    $('.delete.icon.image-add').on('click', function () {
        $(this).parent().remove();;
    });

    $('.ui.edit.button')
      .click(function () {
          $('.ui.fullscreen.modal').modal('show');
      });

});
