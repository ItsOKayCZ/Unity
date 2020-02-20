function move(direction, b){
    var http = new XMLHttpRequest();
    http.open("GET", `${location.origin}/?direction=${direction}&b=${b}`);
    http.send();
}

var el = document.getElementsByTagName("button");
function requestFullscreen(){
    document.getElementsByTagName("body")[0].requestFullscreen();
}

el[0].addEventListener("touchstart", requestFullscreen);
el[0].addEventListener("mousedown", requestFullscreen);
el[1].addEventListener("touchstart", requestFullscreen);
el[1].addEventListener("mousedown", requestFullscreen);