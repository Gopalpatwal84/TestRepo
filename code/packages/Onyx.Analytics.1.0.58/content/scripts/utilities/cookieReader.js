(function(global) {
    /**
     * cookieReader takes one parameter:
     * cookeName: @string
     * return: @object/@string - serialized JSON or string
     **/
    function cookieReader(cookieName) {
        var cookies = document.cookie.split('; '),
            cookieData,
            i = 0,
            tempCookie = [];

        if (!cookieName) {
            console.warn('Please define a cookie name');
        }

        for (i; i < cookies.length; i++) {
            tempCookie[0] = cookies[i].slice(0, cookies[i].indexOf('='));
            tempCookie[1] = cookies[i].slice(cookies[i].indexOf('=') + 1, cookies[i].length);

            if (tempCookie[0] === cookieName) {
                try {
                    cookieData = JSON.parse(tempCookie[1]);
                } catch (e) {
                    cookieData = tempCookie[1];
                }
            }
        }

        return cookieData;
    }

    global.onyx = global.onyx || {};
    global.onyx.utilities = global.onyx.utilities || {};
    global.onyx.utilities.cookieReader = cookieReader;
})(window)