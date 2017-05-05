(function ($, debounce, breakpoints) {
    'use strict';

    var $window = $(window),
        menuSlide = 50,
        mobileMenuSlide = 200;

    /**
     * Hide menu
     */
    function hideMenu() {
        $('.menu-pop').hide(menuSlide);
        $('.menu-drop').slideUp(menuSlide);

        $('.menu-drop-trigger, .menu-pop-trigger')
            .removeClass('active');
    }

    /**
     * Expand/collapse the mobile menu
     */
    function toggleMenu(e) {
        e.preventDefault();

        $(this).toggleClass('active');
        $('.menu-main').slideToggle(mobileMenuSlide);
    }

    /**
     * Expand/collapse the menu items
     */
    function dropMenu(e) {
        var $this = $(e.target),
            $toDrop = $this.siblings('.menu-drop');

        $('.menu-drop-trigger').removeClass('active');

        $('.menu-drop').each(function () {
            var $drop = $(this),
                isCurrentDrop = $drop.is($toDrop),
                isCurrentShowing = $drop.css('display') === 'block';

            if (isCurrentDrop && !isCurrentShowing) {
                $drop.slideDown(menuSlide);
                $drop.siblings('.menu-drop-trigger').addClass('active');
            } else {
                $drop.slideUp(menuSlide);
                $drop.siblings('.menu-drop-trigger').removeClass('active');
            }
        });
    }

    function clickDropMenu(e) {
        if ($window.width() < breakpoints.LARGE) {
            e.preventDefault();
            e.stopPropagation();

            dropMenu(e);
        }
    }

    function hoverDropMenu(e) {
        if ($window.width() >= breakpoints.LARGE) {
            e.stopPropagation();

            dropMenu(e);
        }
    }

    /**
     * Pop down the secondary dropdown navigation
     */
    function popMenu(e) {
        e.preventDefault();

        var $this = $(this),
            $toPop = $this.siblings('.menu-pop');

        $('.menu-pop-trigger').removeClass('active');
        $(this).addClass('active');

        $('.menu-pop').each(function () {
            var $pop = $(this),
                isCurrentPop = $pop.is($toPop),
                isCurrentShowing = $pop.css('display') === 'block';

            if (isCurrentPop && !isCurrentShowing) {
                $pop.slideDown(menuSlide);
            } else {
                $pop.slideUp(menuSlide);
            }
        });
    }

    /**
     * Prevent clicks from effecting the menu triggers
     */
    function stopPropagation(e) {
        e.stopPropagation();
    }

    $(function () { 
        var $body = $('body');
        $body.on('click', hideMenu);
        $body.on('click', '.hamburger', toggleMenu);
        $body.on('click', '.menu-drop-trigger', clickDropMenu);
        $body.on('mouseenter', '.menu > li > a:not(.menu-drop-trigger)', hideMenu);
        $body.on('mouseenter', '.menu-drop-trigger', hoverDropMenu);
        $body.on('click', '.menu-pop-trigger', popMenu);
        $body.on('click', '.menu-drop', stopPropagation);
        $body.on('mouseleave', '.menu', hideMenu);
    });

})(jQuery, window.sd.debounce, window.sd.breakpoints);
