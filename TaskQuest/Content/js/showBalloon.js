function showBalloon(alert, classe) {
    if ($(".balloon").length != 0) 
        $(".to-left").css("display", "block");
    else 
        $("<span class='balloon " + classe + "' display='block'>" + alert + "</span>").appendTo("body");
    $(".balloon").delay(4000).fadeOut(2000);
}