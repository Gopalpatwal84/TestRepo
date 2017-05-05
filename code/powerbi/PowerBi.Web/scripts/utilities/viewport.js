(function(global) {
    function viewportHeight() {
        return Math.max(document.documentElement.clientHeight, window.innerHeight || 0);
    }

    function viewportWidth() {
        return Math.max(document.documentElement.clientWidth, window.innerWidth || 0);
    }

    function viewportSize() {
        return viewportWidth() + "x" + viewportHeight();
    }

    global.onyx = global.onyx || {};
    global.onyx.utilities = global.onyx.utilities || {};
    global.onyx.utilities.viewportHeight = viewportHeight;
    global.onyx.utilities.viewportWidth = viewportWidth;
    global.onyx.utilities.viewportSize = viewportSize;
})(window)