export function validEmptyObjProp(obj = {}, onlyMessage = false, messages = {}) {
    if (!obj) return undefined;
    let validProps = {};
    for (let key in obj) {
        if (key) {
            let value = obj[key];
            switch (typeof value) {
                case "string":
                case "number":
                    let valid = false;
                    let message = (!!(messages) && messages[key]) ? messages[key] : 'Vui lòng nhập thông tin';
                    if (!!value) {
                        valid = true;
                    }
                    validProps[key] = onlyMessage
                        ? (valid ? null : message)
                        : {
                            status: valid,
                            message: valid ? null : message,
                        };
                    break;
            }
        }
    }
    return validProps;
}

export function validEmail(email) {
    const emailRegex = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
    return emailRegex.test(email);
}

export function getReCaptchaKey() {
    const siteKey = process.env.GOOGLE_CAPTCHA_SITE_KEY;
    return new Promise((resolve, reject) => {
        try {
            if (grecaptcha) {
                grecaptcha.ready(function () {
                    grecaptcha
                        .execute(siteKey, {action: "submit"})
                        .then(function (token) {
                            resolve(token);
                        });
                });
            }
        } catch (e) {
            reject(e);
        }
    });
}

export function validNumber(phone) {
    const phoneRegex = /^-?([0-9]\d*|(?=\.))(\.\d+)?$/i;
    return phoneRegex.test(phone);
}

export function itemDataExists(data, value) {
    if(data && data.length > 0)
        return data.some(function(item) {
            return item.id === value;
        });

    return false;
}

export function uuidV4() {
    return ([1e7]+-1e3+-4e3+-8e3+-1e11).replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}