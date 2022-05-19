export function durationVideo(given_seconds) {
    if (given_seconds == 0) return "";
    var hours = Math.floor(given_seconds / 3600);
    var minutes = Math.floor((given_seconds - hours * 3600) / 60);
    var seconds = given_seconds - hours * 3600 - minutes * 60;
    var lst = [];
    if (hours > 0) lst.push(hours);
    lst.push(minutes);
    lst.push(seconds); 
    var timeString = lst.map((o) => o.toString().padStart(2, "0")).join(":"); 
    return timeString;
}

export function getUserLastName(str) {
    if (!str) return "";
    let arr = str.split(' ');
    return arr[arr.length - 1];
}

export function getUsernameIcon(str, number = 2) {
    if (!str || str.length <= 0) return "";
    return str
        .split(" ")
        .filter((s) => s !== "")
        .map((s) => s.charAt(0) + "")
        .slice(0, number)
        .join("");
}

export function numberWithCommas(number) {
    return number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
}

export function replaceCommas(str) {
    return str.toString().replace(/\./g, "");
}

export function createImageUrl(imagePath) {
    if(!imagePath) return null;
    const {IMAGE_DOMAIN} = process.env;
    return imagePath.match(/^http/) 
        ? imagePath 
        : IMAGE_DOMAIN + "/" + imagePath.replace(/^\/+/, "");
}