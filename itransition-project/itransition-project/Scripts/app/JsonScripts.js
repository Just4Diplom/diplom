function getComix (){
    var pages = [];
    for(let page of $("#pages").children("comix-manager").toArray()) {
        pages.push(getPage(page));
    }
    return { "pages": pages };
}

function getPage(page) {
    var template = $(page).find("option[selected]").contents()[0].textContent;
    //page.find("#comic_wrapper_" + template)
    var frame_images = [];
    for(let image of $(page).find(".frame-image").toArray()) {
        frame_images.push(getImage(image));
    }
    return {
        "template": template,
        "frame_images": frame_images
    }
}

function getImage(image) {
    var bg = $(image).css("background-image");
    var position = $(image).css("position");
    var top = $(image).css("top");
    var left = $(image).css("left");
    var width = $(image).css("width");
    var height = $(image).css("height");

    var balloons = [];
    for(let balloon of $(image).find(".talkbubble").toArray()) {
        balloons.push(getBalloon(balloon));
    }


    return {
        "background_image": bg,
        "position": position,
        "top": top,
        "left": left,
        "width": width,
        "height": height,
        "balloons": balloons
    }
}

function getBalloon(balloon) {
    var text = $(balloon).find("textarea").val();
    var position = $(balloon).css("position");
    var top = $(balloon).css("top");
    var left = $(balloon).css("left");
    var width = $(balloon).css("width");
    var height = $(balloon).css("height");

    return {
        "text": text,
        "position": position,
        "top": top,
        "left": left,
        "width": width,
        "height": height
    }
}