function showBalloon(alert, classe) {
    $("<span class='balloon "+classe+"' display='block'>"+alert+"</span>").appendTo("body");
    $(".balloon").delay(4000).fadeOut(2000);
}