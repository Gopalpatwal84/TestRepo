(function($) {
    'use strict';

    $('body').on('keydown', 'a[role="button"]', function (e) {
        if (e.which === 32) {
            e.preventDefault();
            this.click();
        }
    });
})(jQuery);
