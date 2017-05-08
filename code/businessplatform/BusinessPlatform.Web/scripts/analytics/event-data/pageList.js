(function (global) {
    /**
      * Key should be page name from events.js (ex. 'home-visited-page', 'home' is the key for the value/regex)
      * Value should be regex, or an array of regex to match multiple urls to single event in events.js
      * Please be sure to keep this list ALPHABETIZED (ascending)
      **/
    var pageList = {
        home: '^\/[a-z]{2}-[a-z]{2}\/$'
    };

    global.onyx = global.onyx || {};
    global.onyx.analytics = global.onyx.analytics || {};

    global.onyx.analytics.pageList = pageList;
})(window)