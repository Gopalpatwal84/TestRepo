(function ($, global) {
    'use strict';

    function navigateTo() {
        global.location = this.value;
    }

    function onLoad() {
        $('.sd-dropdown-navigation').val(global.location.pathname);
    }

    $(onLoad);
    $('body').on('change', '.sd-dropdown-navigation', navigateTo);
})(jQuery, window);
