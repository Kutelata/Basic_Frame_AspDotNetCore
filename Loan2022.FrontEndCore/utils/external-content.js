// this function will work cross-browser for loading scripts asynchronously
export function loadScript(src) {
    return new Promise(function(resolve, reject) {
        let script = document.querySelector(`script[src="${src}"]`);
        if(!script) {
            script = document.createElement('script');
            let ready = false;
            script.type = 'text/javascript';
            script.src = src;
            script.async = true;
            script.onerror = function(err) {
                reject(err, script);
            };
            
            script.onload = script.onreadystatechange = function() {
                // console.log(this.readyState); // uncomment this line to see which ready states are called.
                if (!ready && (!this.readyState || this.readyState == 'complete')) {
                    ready = true;
                    resolve();
                }
            };
            document.body.appendChild(script);
        }
        else{
            resolve();
        }
    });
}