(function (global) {
    function set(param, value) {
        //Mixpanel does not require a 'set' method to send specific data.
        return null;
    }

    function fire(page, action, element, properties) {
        if (global.onyx.DEBUG) {
            console.log('mixpanel.track(' + page + '-' + action + '-' + element + ',' + JSON.stringify(properties) + ')');
        } else {
            global.mixpanel.track(page + '-' + action + '-' + element, properties);
        }

    }

    function identify() {
        var cookie = global.onyx.utilities.cookieReader('userInfo') || false;
            alias = {};

        if (cookie) {
            alias.guid = cookie.guid || null;
            alias.hashed = cookie.hashed || null;

            if (global.onyx.DEBUG) {
                console.log('mixpanel.alias(' + (alias.hashed || alias.guid) + ')');
                console.log('mixpanel.identify(' + (alias.hashed || alias.guid) + ')');
            } else {
                global.mixpanel.alias(alias.hashed || alias.guid);
                global.mixpanel.identify(alias.hashed || alias.guid);
            }

        }
    }

    global.onyx = global.onyx || {};
    global.onyx.analytics = global.onyx.analytics || {};
    global.onyx.analytics.eventSenders = global.onyx.analytics.eventSenders || {};

    global.onyx.analytics.eventSenders.mixpanel = {
        set: set,
        fire: fire,
        identify: identify
    };
})(window);