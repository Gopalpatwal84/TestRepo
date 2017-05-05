(function ($, global) {
    'use strict';

    function cultureChanged() {
        global.document.location = global.document.location.pathname.replace(/^\/[a-z]{2}-[a-z]{2}\//, '/' + $(this).val() + '/');
    }

    $('body').on('change', '.cultures-dropdown', cultureChanged);
})(jQuery, window);