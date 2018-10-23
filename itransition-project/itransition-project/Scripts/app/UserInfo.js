function DelComment(id) {

        jQuery.ajax({
            type: "POST",
            url: "/User/DelComment",
            data: JSON.stringify({ data: id }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: commentDeleteCallback(id)
        });
}

function commentDeleteCallback(id) {
    $("button[data-id='" + id + "']").closest(".comment-list-container").remove();
}

function DelComix(id) {

    jQuery.ajax({
        type: "POST",
        url: "/User/DelComix",
        data: JSON.stringify({ data: id }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: comixDeleteCallback(id)

    });
}

function comixDeleteCallback(id) {
    $("button[data-id='" + id + "']").closest(".comix-list-container").remove();
}

function getUserName() {
    var id = document.getElementById("UserName").textContent;
    return id;
}