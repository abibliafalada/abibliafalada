$(document).ready(function() {

    $('#cover').backstretch([
        "assets/img/capa_leitor_biblia.jpg",
        "assets/img/capa_leitor_biblia_alternativo.jpg"
    ], {duration: 6000, fade: 1000});

    coverHeight();
        $(window).bind('resize', coverHeight);

    $('#saiba-mais').click(function(){
      $.scrollTo( '#post-list', 800 );
    })

});

function coverHeight() {
    var height = parseInt($(window).height()) + 'px';
    $("#cover").css('height',height);
}