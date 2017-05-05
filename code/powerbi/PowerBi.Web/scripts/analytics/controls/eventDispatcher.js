(function($, global, events, eventSenders, utilities, analytics) {
    function setBaseProperties() {
        var region = $('meta[name="ms.region"]').attr('content'),
            version = $('meta[name="ms.sitever"]').attr('content'),
            sitename = $('meta[name="ms.sitename"]').attr('content'),
            culture = $('meta[name="ms.lang"]').attr('content') + '-' + $('meta[name="ms.loc"]').attr('content'),
            url = window.location.pathname.split('/')[window.location.pathname.split('/').length - 2],
            correlationId = utilities.cookieReader('userInfo') ? utilities.cookieReader('userInfo').hashed || utilities.cookieReader('userInfo').guid : '';

        return {
            'region': region,
            'version': version,
            'sitename': sitename,
            'url': url,
            'correlationId': correlationId,
            'viewportheight': utilities.viewportHeight() || "Unknown",
            'viewportwidth': utilities.viewportWidth() || "Unknown"
        }
    }

    function sendEventData(eventData, properties) {
        if (eventData && eventData.blacklist) {
            for (lib in eventSenders) {
                if (!eventData.blacklist[lib]) {
                    eventSenders[lib].fire(eventData.page, eventData.action, eventData.element, properties);
                    eventSenders[lib].set('viewportSize', utilities.viewportSize() || 'Unknown');
                    eventSenders[lib].identify();
                }
            }
            return;
        }

        for (lib in eventSenders) {
            eventSenders[lib].fire(eventData.page, eventData.action, eventData.element, properties);
            eventSenders[lib].set('viewportSize', utilities.viewportSize() || 'Unknown');
            eventSenders[lib].identify();
        }
    }

    function getData(event) {
        var eventList = events,
            eventName = event.constructor === $ ? event.data('event') : event,
            eventData,
            properties = setBaseProperties(),
            lib;

        eventData = eventList[eventName];

        if (eventData && eventData.properties && (event.constructor === $)) {
            properties.target = event.data('event-property') || event.attr('title') || $('h1, h2, h3, h4, h5, h6', event).text() || $('img', event).attr('alt') || event.text() || '';

            if (typeof properties.target === "object") {
                var obj = properties.target;
                if (!obj.target) {
                    throw 'A \'target\' property should be specified when using a JSON property';
                }
                if ( !! properties.target) {
                    delete properties.target;
                }
                $.extend(properties, obj);
            }

            properties.target = properties.target.replace(/[\t\n]+/g, '');
        } else if (event.constructor === String && !!eventData.hasElement) {
            properties.target = event;
            if ($(eventData.hasElement.selector).length > 0) {
                properties[eventData.hasElement.name] = true;
            }
        } else if (event.constructor === String) {
            properties.target = $('[data-dropdown-event="' + event + '"]').data('event-property') || event;
        }

        sendEventData(eventData, properties);
    }

    function getFormData(event, data) {
        var eventList = events,
            eventData,
            properties = setBaseProperties(),
            lib

        eventData = eventList[event];
        
        if (eventData) {
            if (eventData.properties) {
                properties.formName = data['FormOptions.Slug'] || '';
            }

            sendEventData(eventData, properties);
        }
    }

    function getSearchData(event, name, value, searchProps) {
        var eventList = events,
            eventName = name,
            eventData,
            properties = setBaseProperties(),
            lib;

        if (typeof searchProps === "object") {
            $.extend(properties, searchProps);
        }

        eventData = eventList[eventName]

        sendEventData(eventData, properties);
    }

    global.onyx = global.onyx || {};
    global.onyx.analytics = global.onyx.analytics || {}

    global.onyx.analytics.getData = getData;
    global.onyx.analytics.getFormData = getFormData;
    global.onyx.analytics.getSearchData = getSearchData;
})(
    jQuery,
    window,
    $.extend(window.onyx.analytics.defaultEvents, window.onyx.analytics.events),
    window.onyx.analytics.eventSenders,
    window.onyx.utilities,
    window.onyx.analytics
)


// Page view event handler
$(function() {
    var currentPage = window.location.pathname || '',
        page,
        i;

    for (page in window.onyx.analytics.pageList) {
        if (window.onyx.analytics.pageList[page].constructor === Array) {
            for (i = 0; i < window.onyx.analytics.pageList[page].length; i++) {
                if (new RegExp(window.onyx.analytics.pageList[page][i], 'i').test(currentPage)) {
                    window.onyx.analytics.getData(page + '-visited-page');
                    return;
                }
            }
        } else {
            if (new RegExp(window.onyx.analytics.pageList[page], 'i').test(currentPage)) {
                window.onyx.analytics.getData(page + '-visited-page');
                return;
            }
        }
    }
});

// Form submitted event handler

(function (global) {
    $(global).on('form-submitted', function (e, form) {
        var formData = $(form).serializeArray(),
            indexedData = {};

        $.each(formData, function (i, n) {
            indexedData[n['name']] = n['value'];
        });

        global.onyx.analytics.getFormData('global-submitted-form', indexedData);

    });

    $(global).on('form-invalid-clientside', function (e, params) {
        var form = params.form,
            errors = params.errors,
            formData = $(form).serializeArray(),
            indexedData = {},
            isEmpty = true;

        $.each(formData, function (i, n) {
            indexedData[n['name']] = n['value'];

            if (n['name'] != '__RequestVerificationToken' && n['name'].indexOf('FormOptions.') == -1) {
                if (n['value'] != '') {
                    isEmpty = false;
                }
            }
        });

        if (isEmpty) {
            global.onyx.analytics.getFormData('global-empty-form', indexedData);
        } else if (errors['Email'] == 'disallowedtldemail') {
                window.onyx.analytics.getFormData('global-blocked-form', indexedData);
        } else {
            global.onyx.analytics.getFormData('global-invalid-form', indexedData);
        }
    });

    $(global).on('form-invalid-serverside', function (e, form) {
        var formData = $(form).serializeArray(),
            indexedData = {},
            email = '';

        $.each(formData, function (i, n) {
            indexedData[n['name']] = n['value'];

            if (n['name'] == 'Email') {
                email = n['value'];
            }
        });

        if (email != '') {
            window.onyx.analytics.getFormData('global-consumer-form', indexedData);
        }
    });
})(window);

// Event listeners

// This event listener will fire on any HTML element with a data-event="" attribute
$('body').on('mousedown', '[data-event]', function(e) {
    window.onyx.analytics.getData($(this));
});

// This event will fire whenever the global object receives a 'searched' event
$(window).on('searched', function(event, name, value) {
    window.onyx.analytics.getSearchData(event, name, value);
});
