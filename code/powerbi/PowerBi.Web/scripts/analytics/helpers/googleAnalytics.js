(function (global) {
    function set(param, value) {
        if (global.onyx.DEBUG) {
            console.log('ga(set, ' + param + ',' + value + ')');
        } else {
            if (global.ga) {
                global.ga('set', param, value);
            }
        }
    }

    function fire(page, action, element, proprties) {
        if (global.onyx.DEBUG) {
            console.log('ga(send, event, ' + page + ',' + action + ',' + element + ',' + JSON.stringify(proprties) + ')');
        } else {
            if (global.ga) {
                global.ga('send', 'event', page, action, element, proprties);
            }
        }
    }

    function identify() {
        //GA does not have the functionality to assign a custom id, so we return null.
        return null;
    }

    global.onyx = global.onyx || {};
    global.onyx.analytics = global.onyx.analytics || {};
    global.onyx.analytics.eventSenders = global.onyx.analytics.eventSenders || {};

    global.onyx.analytics.eventSenders.googleAnalytics = {
        set: set,
        fire: fire,
        identify: identify
    };
})(window);