(function (debounce) {
    'use strict';

    var baseHeight = {},
        DEFAULT = 'default',
        DELAY = 10;

    function resetHeight(index, element) {
        var $element = $(element);
        baseHeight = {};

        $element.height('auto');
    }

    function getHeight(index, element) {
        var $element = $(element),
            height = $element.height(),
            set = $element.data('set') || DEFAULT;

        if (!baseHeight[set] || height > baseHeight[set]) {
            baseHeight[set] = height;
        }
    }

    function setHeight(index, element) {
        var $element = $(element),
            set = $element.data('set') || DEFAULT;

        $element.height(baseHeight[set]);
    }

    function equalize() {
        var $toEqualize = $('.sd-equalize');
        $toEqualize.each(resetHeight);
        $toEqualize.each(getHeight);
        $toEqualize.each(setHeight);
    }

    function load() {
        equalize();
        setTimeout(equalize, 500);
    }

    // Event listeners
    $(load);
    $(window).on('resize load', debounce(equalize, DELAY));
    $('body').on('equalize.sundog', equalize);
})(sd.debounce);
