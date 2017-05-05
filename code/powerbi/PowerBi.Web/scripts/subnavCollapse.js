(function ($, global) {
    // Category Menu Toggle
    $('.subnavMenuToggle').on('click', function (event) {
        var menu = $('.subnavMenuLinks')[0],
            menuScrollHeight = menu.scrollHeight,
            menuHeight = $(menu).height();
        if (menuHeight == 0) {
            $('.subnavMenuLinks').css('height', menuScrollHeight);
            $('.subnavMenuExpandIcon').addClass('expand');
        } else {
            $('.subnavMenuLinks').css('height', '0');
            $('.subnavMenuExpandIcon').removeClass('expand');
        }
    });
})(jQuery, window);