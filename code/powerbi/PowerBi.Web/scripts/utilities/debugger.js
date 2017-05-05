// Check for debug
$(function() {
    if (window.location.search.indexOf('debug=true') > -1) {
        window.onyx.DEBUG = true;
    } else {
        window.onyx.DEBUG = false;
    }
});