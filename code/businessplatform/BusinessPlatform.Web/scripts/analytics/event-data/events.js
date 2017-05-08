(function (global) {
    /**
      * event object structure
      * {
      *      @string, - describes the event: page-action-element: {
      *          page: @string, - describes which page in which the event occurs
      *          action: @string, - describes in past tense what event occurred (ex. 'clicked', 'visited', etc)
      *          element: @string or @null - describe the element. if null, whill pull in the text of the element that was actioned upon
      *          blacklist: @object - array of objects that, if set to true will stop an event from being fired through a specifc analytics engine (ex. { km: true })
      *          properties: @bool - set to true, will capture the text of the element that was clicked on (default is false)
      *      }
      * }
      **/
    var events = {
        'home-visited-page': {
            page: 'home',
            action: 'visited',
            element: 'page'
        }
    };

    global.onyx = global.onyx || {};
    global.onyx.analytics = global.onyx.analytics || {};

    global.onyx.analytics.events = events;
})(window)