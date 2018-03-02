$(document).ready(function(){
  authorOptions = $(".author").html();
  $(".author").remove();

  $("#number-of-authors").change(function(){
    if($("#number-of-authors").val() < 0) {
      $("#number-of-authors").val(0);
    } else if ($("#number-of-authors").val() > 10) {
      $("#number-of-authors").val(10);
    }
    number = parseInt($("#number-of-authors").val());
    $(".author").remove();
    for (var i = 0; i < number; i++) {
      console.log(authorOptions);
      $("#authors").append(
        '<select class="author" name="author' + i + '">' +
        authorOptions +
        '</select>'
      );
    }
  });
});
