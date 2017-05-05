(function() {
    'use strict';

    $('body').on('click', '.navigationLeft .title', function () {
        $(this).parents('.navigationLeft').toggleClass('show');
    });
})();