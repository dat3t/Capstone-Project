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
});
