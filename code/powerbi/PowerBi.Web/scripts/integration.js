(function ($) {
    $('#connect-wrapper').on('click', '#show-all', function (e) {
        e.preventDefault();
        $('.integration-all').show();
        $('.integration-featured').hide();
    });

    $('#connect-wrapper').on('click', '#show-featured', function (e) {
        e.preventDefault();
        $('.integration-all').hide();
        $('.integration-featured').show();
    });
})(jQuery);