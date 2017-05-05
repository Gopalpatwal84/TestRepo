# Onyx Analytics

## Getting Started

### Config File
First, you must create an analyticsConfig.js file that will need to be loaded before all other analytics files. The contents should resemble the following:

```
(function (global) {
    global.onyx = global.onyx || {};
    global.onyx.analytics = global.onyx.analytics || {};
    global.onyx.analytics.config = global.onyx.analytics.config || {};

    global.onyx.analytics.config = {
        mixpanel: {
            key: "!!! YOUR MIXPANEL PROJECT KEY !!!"
        },
        ga: {
            key: "!!! YOUR GOOGLE ANALYTICS PROJECT KEY !!!",
            domain: "!!! YOUR GOOGLE ANALYTICS PROJECT DOMAIN !!!"
        }
    }
})(window)
```

Next, in your script bundler, please use the following convention to ensure no race conditions occur:

```
"~/scripts/utilities/*.js",
"~/scripts/analytics/config/*.js",
"~/scripts/analytics/libs/*.js",
"~/scripts/analytics/event-data/*.js",
"~/scripts/analytics/helpers/*.js",
"~/scripts/analytics/controls/*.js",
```

In the event-data folder, you must create two files:

events.js and pageList.js


### events.js
events.js houses all of your event data. The structure should look like the following:

```
(function(global) {
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
        /**
          *
          * EXAMPLE:
          *      'home-visited-page': {
          *          page: 'home',
          *          action: 'visited',
          *          element: 'page'
          *      }
          *
          **/
    };

    global.onyx = global.onyx || {};
    global.onyx.analytics = global.onyx.analytics || {};

    global.onyx.analytics.events = events;
})(window)
```

### pageList.js
pageList.js houses all of your page view event triggers, managed via regex. The contents of the file should look like the following:

```
(function(global) {
    /**
      * Key should be page name from events.js (ex. 'home-visited-page', 'home' is the key for the value/regex)
      * Value should be regex, or an array of regex to match multiple urls to single event in events.js
      * Please be sure to keep this list ALPHABETIZED (ascending)
      **/
    var pageList = {
        /**
          *
          * EXAMPLE:
          *
          * home: '^/[a-z]{2}-[a-z]{2}/$',
          *
          **/
    };

    global.onyx = global.onyx || {};
    global.onyx.analytics = global.onyx.analytics || {};

    global.onyx.analytics.pageList = pageList;
})(window)
```