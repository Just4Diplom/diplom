$(function() {
    $( ".frame-image" ).draggable();
    $( ".frame-image" ).resizable();
    $('.frame-image').bind('mousewheel', function(e){
        if(e.originalEvent.deltaY < 0) {
            $(this).css("width", "+=16");
            $(this).css("height", "+=16");
        }
        else{
            $(this).css("width", "-=16");
            $(this).css("height", "-=16");
        }
        return false;
    });
});