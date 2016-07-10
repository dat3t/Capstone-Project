var content = [
  { title: 'Andorra' },
  { title: 'United Arab Emirates' },
  { title: 'Afghanistan' },
  { title: 'Antigua' },
  { title: 'Anguilla' },
  { title: 'Albania' },
  { title: 'Armenia' },
  { title: 'Netherlands Antilles' },
  { title: 'Angola' },
  { title: 'Argentina' },
  { title: 'American Samoa' },
  { title: 'Austria' },
  { title: 'Australia' },
  { title: 'Aruba' },
  { title: 'Aland Islands' },
  { title: 'Azerbaijan' },
  { title: 'Bosnia' },
  { title: 'Barbados' },
  { title: 'Bangladesh' },
  { title: 'Belgium' },
  { title: 'Burkina Faso' },
  { title: 'Bulgaria' },
  { title: 'Bahrain' },
  { title: 'Burundi' }
  // etc
];
$(document).ready(function () {
    $(".right.menu.open").on("click", function (e) {
        e.preventDefault();
        $(".ui.vertical.menu.open").toggle();
    });

    var
  content = [
    {
        title: 'Horse',
        description: 'An Animal',
    },
    {
        title: 'Cow',
        description: 'Another Animal',
    }
  ]
    ;
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
