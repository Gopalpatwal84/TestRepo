(function (global) {
    var defaultEvents = {
        'global-submitted-form': {
            page: 'global',
            action: 'submitted',
            element: 'form',
            properties: true
        },
        'global-empty-form': {
            page: 'global',
            action: 'empty',
            element: 'form',
            properties: true
        },
        'global-invalid-form': {
            page: 'global',
            action: 'invalid',
            element: 'form',
            properties: true
        },
        'global-consumer-form': {
            page: 'global',
            action: 'consumer',
            element: 'form',
            properties: true
        },
        'global-blocked-form': {
            page: 'global',
            action: 'blocked',
            element: 'form',
            properties: true
        }
    };

    global.onyx = global.onyx || {};
    global.onyx.analytics = global.onyx.analytics || {};

    global.onyx.analytics.defaultEvents = defaultEvents;
})(window)