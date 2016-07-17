
$(document).ready(function () {
    $('.icon')
  .popup()
    ;

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
            columnWidth: 50
        }
    });
    // change size of item by toggling gigante class
    $grid.on('click', '.grid-item', function () {
        $(this).find('.marker').toggleClass("hides");
        $(this).toggleClass('gigante');
        // trigger layout after item size changes
        $grid.isotope('layout');
    });
    var $stamp = $grid.find('.stamp');
var isStamped = false;

$('.stamp-button').on('click', function () {
    $('body,html').animate({
        scrollTop: 0                       // Scroll to top of body
    }, 500);
    $stamp.toggleClass("hides");
    $(".edits").toggleClass("edits-cl");
    $(".plus").toggleClass("plus-cl");
  // stamp or unstamp element
  if ( isStamped ) {
      $grid.isotope('unstamp', $stamp);
  } else {
      $grid.isotope('stamp', $stamp);
  }
  // trigger layout
  $grid.isotope('layout');
  // set flag
  isStamped = !isStamped;
});
    // ===== Scroll to Top ==== 
$(window).scroll(function () {
    if ($(this).scrollTop() >= 50) {        // If page is scrolled more than 50px
        $('#return-to-top').fadeIn(200);    // Fade in the arrow
    } else {
        $('#return-to-top').fadeOut(200);   // Else fade out the arrow
    }
});
$('#return-to-top').click(function () {      // When arrow is clicked
    $('body,html').animate({
        scrollTop: 0                       // Scroll to top of body
    }, 300);
});
    
});
