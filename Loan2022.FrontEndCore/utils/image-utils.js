export function getImageInfoFromUrl(imagePath) {
    if(!imagePath || !(/\/zoom\/(?<width>\d{1,})_(?<height>\d{1,})/gm).test(imagePath))
        return null;
    
    let regexValue = (/\/zoom\/(?<width>\d{1,})_(?<height>\d{1,})/gm).exec(imagePath).groups;
    
    return {
        width: regexValue.width,
        height: regexValue.height,
        originUrl: imagePath.replace((/\/zoom\/(?<width>\d{1,})_(?<height>\d{1,})/gm), '')
    }
}

export function dataURLtoFile(dataurl, filename) {
 
    var arr = dataurl.split(','),
        mime = arr[0].match(/:(.*?);/)[1],
        bstr = atob(arr[1]), 
        n = bstr.length, 
        u8arr = new Uint8Array(n);
        
    while(n--){
        u8arr[n] = bstr.charCodeAt(n);
    }
    
    return new File([u8arr], filename, {type:mime});
}