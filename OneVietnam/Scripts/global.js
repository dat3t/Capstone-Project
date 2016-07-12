$(document).ready(function () {
    $(".right.menu.open").on("click", function (e) {
        e.preventDefault();
        $(".ui.vertical.menu.open").toggle();
    });    
    $('.ui.search').search({
        apiSettings: {
            url: '/Home/search?id={query}'
        },
        fields: {
            results: 'Result',
            title: 'Title',
            description: 'Description',
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
});