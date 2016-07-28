$(document)
    .ready(function () {
        $("#getloc").click();
        $(".ui.floating.dropdown.button").dropdown({
            allowCategorySelection: true
        })
        ;
    //function createUploader() {
    //    var uploader = new qq.FineUploader({
    //        element: document.getElementById('fine-uploader'),
    //        request: {
    //            endpoint: '@Url.Action("UploadBatchDataFile")'
    //        }
    //    });
    //}
    //window.onload = createUploader;
       
        $(".ui.toggle.button")
            .state({
        text: {
            inactive: 'Bật',
            active: 'Tắt'
        }
            });

        $("#chatchat")
            .click(function () {
                $("#messagechat").slideToggle();
    });

    if ($(".searchType").val() === "SearchPosts") {
        $(".ui.user").css("display", "none");
        $(".ui.post").css("display", "inline-flex");
    } else {
        $(".ui.user").css("display", "inline-flex");
        $(".ui.post").css("display", "none");
    }
        $(".searchType")
            .dropdown({
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
        $(".right.menu.open")
            .on("click",
                function (e) {
        e.preventDefault();
        $(".ui.vertical.menu.open").toggle();
    });
    

    //SearchBox
        $('.ui.search.post')
            .search({
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
            });
        $('#user')
            .search({
      apiSettings: {
          url: '//api.github.com/search/repositories?q={query}'
      },
      fields: {
          results: 'items',
          title: 'name',
          url: 'html_url'
      },
        minCharacters: 3
            });
        $('.ui.search.user')
            .search({
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
            });

    //ThamDTH
    $('.clearing.star.rating').rating('setting', 'clearable', true);


    $('.ui.multiple.dropdown')
      .dropdown({
          allowAdditions: true
            });
    $('#drPostType')
      .dropdown({
          
            });
    $("#drdGender").dropdown({});

        $('.delete.icon.image-add')
            .on('click',
                function () {
        $(this).parent().remove();;
    });

    $('.ui.edit.button')
            .click(function () {
          $('.ui.fullscreen.modal').modal('show');
      });

    $('.icon.link').popup({});    
   
        $('.tabular.menu .item')
            .tab({
    
            });

        


    //ToanLM


    var $grid = $('.grids').isotope({
        itemSelector: '.grid-item',
        percentPosition: true,
        masonry: {
            columnWidth: '.grid-sizer',
            gutter: '.gutter-sizer'
        }
    });
    
    
    // change size of item by toggling gigante class
    $grid.on('click', '.content', function (e) {

        $(this).parent().find('.marker').toggleClass("hides");
                var id = $(this).parent().find('#postId').val();
       
        $.ajax({
            type: 'GET',
            data:{"postId":id},
            url: '_ShowPost',
            success: function (partialResult) {
                $("#forModal").empty();
                $("#forModal").html(partialResult);
                $('#forModal').modal('show')
                ;
            }
        });
     
//        $(this).parent().parent().toggleClass('gigante');
        // trigger layout after item size changes
        $grid.isotope('layout');
    });
    var $stamp = $grid.find('.stamp');
var isStamped = false;
   
$('.stamp-button').on('click', function () {
    $('body,html').animate({
        scrollTop: 0                       // Scroll to top of body
    }, 500);
    $("#CreatePostForm").data('validator').resetForm();
    $(".validation-summary-errors ul li").remove();
    $(".validation-summary-errors").addClass('validation-summary-valid').removeClass('validation-summary-errors');
    $(".stamp").toggleClass("hides");
    $(".edits").toggleClass("edits-cl");
    $(".plus").toggleClass("plus-cl");
  // stamp or unstamp element
          
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

$('.filter-group').on('click', 'a', function () {
    var filterValue = $(this).attr('data-filter');
    // use filterFn if matches value
    $grid.isotope({ filter: filterValue });
});
});
