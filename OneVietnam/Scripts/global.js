$(document).ready(function () {
    $('.item.write')
       .click(function () {


           $('.ui.modal').modal('show');

       });

    $('.ui.dropdown')
        .dropdown({
            allowAdditions: true
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
            url: ''
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
});