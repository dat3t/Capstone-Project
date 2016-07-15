$(document).ready(function () {
    $('.item.write')
       .click(function () {
           $('.ui.modal').modal('show');
       });
    if ($(".searchType").val() === "SearchPosts") {
        $(".ui.user").css("display", "none");
        $(".ui.post").css("display", "inline-flex");
    } else {
        $(".ui.user").css("display", "inline-flex");
        $(".ui.post").css("display", "none");
    }
    $(".searchType").dropdown({
        onChange: function (value, text, $selectedItem) {            
            if (value === "SearchPosts") {                
                $(".ui.user").css("display", "none");
                $(".ui.post").css("display", "inline-flex");

            } else {                
                $(".ui.user").css("display", "inline-flex");
                $(".ui.post").css("display", "none");
            }
        }
    });
    $("div#myId").dropzone({ url: "/file/post" });
    $(".right.menu.open").on("click", function (e) {
        e.preventDefault();
        $(".ui.vertical.menu.open").toggle();
    });
    

    //SearchBox
    $('.ui.search.post').search({
        apiSettings: {
            url: '/Search/search?query={query}'
        },
        fields: {
            results: 'Result',
            title: 'Title',
            description: 'Description',
            url: 'Url'
        },
        minCharacters: 3
    })
    ;
    $('.ui.search.user').search({
        apiSettings: {
            url: '/Search/UsersSearch?query={query}'
        },
        fields: {
            results: 'Result',
            title: 'Title',
            description: 'Description',
            url: 'Url'
        },
        minCharacters: 2
    })
    ;

    //ThamDTH
    $('.clearing.star.rating').rating('setting', 'clearable', true);

    $('.ui.multiple.dropdown')
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

    $('.icon.link').popup({});    
});
