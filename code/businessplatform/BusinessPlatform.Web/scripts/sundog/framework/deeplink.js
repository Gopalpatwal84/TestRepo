(function (global) {
    'use strict';

    /**
     * Hash change update
     */
    function hashChange() {
        var $this = $(this),
            hash = $this.attr('href');

        // SUNDOG-116 Avoid clashing with tabs
        if(!$this.parents('.sd-tabs').length) {
            scrollToLink(hash);
        }
    }

    /**
     * Hash change update
     */
    function scrollToLink(hash) {
        var $this = $(hash),
            elementOffset = 0,
            headerHeight = 0;

        if (!!$this.length) {
            elementOffset = $this.offset().top;
            headerHeight = $('.navigation').outerHeight(true);

            // scroll to position
            $('html, body').animate({ scrollTop: elementOffset - headerHeight }, 10);

            // strip id and name to ignore default
            $this.attr({ 'id': '', 'name': '' });

            setTimeout(function () {
                restoreFromHash($this, hash);
            }, 0);
        }
    }

    /**
     * Restore the id and name from hash
     */
    function restoreFromHash($el, hash) {
        var id = hash.replace('#', '');

        $el.attr({
            'id': id,
            'name': id
        });

        $(window).trigger('scroll');
    }

    /**
     * If hash is present on load, scroll
     */
    function scrollOnLoad() {
        var hash = global.location.hash;

        if (!!hash) {
            setTimeout(scrollToLink(hash), 50);
        }
    }

    $('body').on('click', 'a[href^="#"]:not([href="#"])', hashChange);
    $(scrollOnLoad);
})(window);
