function getUserName() {
    var id = document.getElementById("UserName").textContent;
    return id;
}

function getUserAbout() {
    var id = document.getElementById("UserName").textContent;
    return id;
}

function getUserTheme() {
    var Theme = document.getElementById("Theme").options[document.getElementById("Theme").selectedIndex].value;
    return Theme;
}

function sendAjax() {

    jQuery.ajax({
        type: "POST",
        url: "/User/RefreshInfo",
        data: JSON.stringify({ id: getUserName(), about: getUserAbout(), theme: getUserTheme() }),
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    });
}