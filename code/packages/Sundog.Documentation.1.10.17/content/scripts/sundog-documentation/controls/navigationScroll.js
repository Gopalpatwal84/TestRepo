(function($, global) {
    'use strict';

    var $scrollNavigation = $('.wa-navigationScroll, .navigationScroll'),
        $window = $(global),
        $columnScroll = $('.column-navigationScroll');

    function onScroll () {
        var $parent = $scrollNavigation.parents('.section'),
            $scrollingNavigation = $columnScroll.find('.navigationScroll'),
            parentHeight = $parent.offset().top + $parent.height() + (($parent.outerHeight() - $parent.height()) / 2),
            navHeight = $scrollingNavigation.offset().top + $scrollingNavigation.outerHeight();

        if (navHeight > parentHeight) {
            $scrollingNavigation.addClass('hidden');
        } else {
            $scrollingNavigation.removeClass('hidden');
        }
    }

    $scrollNavigation.addClass('navigationScroll');

    if (!!$scrollNavigation.length) {
        $columnScroll.hide().html($scrollNavigation.clone()).fadeIn(800);
        $window.on('scroll', onScroll);
    }

})(jQuery, window);
