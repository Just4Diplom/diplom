function make_it_draggable() {
    $(".frame-image").draggable({
        //containment: "parent"
    });
    $(".frame-image").resizable();
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

    $("p.speech").draggable(
        { helper: "clone" });
    $("p.speech").resizable();

    $(".frame-image").droppable({
        accept: "#balloons_panel .speech",
        drop: function (event, ui) {
            var clone = ui.draggable.clone();
            clone.draggable({
                containment: "parent"
            });
            clone.resizable();
            clone.css("position", "absolute");
            $(this).append(clone);
        }
    });
};

//$(function () { make_it_draggable() });