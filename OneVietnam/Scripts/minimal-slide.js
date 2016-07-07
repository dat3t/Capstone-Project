$(document).ready(function() {
  $("a.sidebar-toggle").click(function() {
    $('.ui.sidebar').sidebar('toggle');
  });
  $(".ui.embed").embed();
});