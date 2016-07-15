
$(document).ready(function () {
    

    $('#searchType').dropdown({
      
        onChange: function (value, text, $selectedItem) {


            $("#" + value).removeClass("hide-search").siblings().not(".dropdown").addClass("hide-search");
        }
    });
  
    $('.item.write')
       .click(function () {
           $('.ui.modal').modal('show');
       });


  
    $("div#myId").dropzone({ url: "/file/post" });
    $(".right.menu.open").on("click", function (e) {
        e.preventDefault();
        $(".ui.vertical.menu.open").toggle();
    });    
    $('#post').search({
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
    $('#user').search({
      apiSettings: {
          url: '//api.github.com/search/repositories?q={query}'
      },
      fields: {
          results: 'items',
          title: 'name',
          url: 'html_url'
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
//    $('')
//      .dropdown({
//        
//      })
//    ;
    $('.delete.icon.image-add').on('click', function () {
        $(this).parent().remove();;
    });

    $('.ui.edit.button')
      .click(function () {
          $('.ui.fullscreen.modal').modal('show');
      });

    $('.icon.link').popup({});
   

    var $grid = $('.grids').isotope({
        itemSelector: '.grid-item',
        masonry: {
            columnWidth: 100
        }
    });
    // change size of item by toggling gigante class
    $grid.on('click', '.grid-item', function () {
        $(this).toggleClass('gigante');
        // trigger layout after item size changes
        $grid.isotope('layout');
    });
    
});
