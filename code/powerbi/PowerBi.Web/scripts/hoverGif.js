(function ($) {
    'use strict';

    /**
     * Trigger gif src swap on mouseenter
     */
    function enterGif() {
        var $this = $(this),
            $gifs = $this.find('[data-src-gif]').addBack('[data-src-gif]');

        $gifs.each(function () {
            var $this = $(this),
                src = $this.attr('src'),
                altSrc = $this.data('src-gif');

            if (altSrc) {
                $this.data('src-temp', src);
                $this.attr('src', altSrc);
            }
        });
    }

    /**
     * Trigger gif src swap on mouseleave
     */
    function leaveGif() {
        var $this = $(this),
            $gifs = $this.find('[data-src-gif]').addBack('[data-src-gif]');

        $gifs.each(function () {
            var $this = $(this),
                tempSrc = $this.data('src-temp');

            $this.attr('src', tempSrc);
            $this.removeAttr('src-temp');
        });
    }

    $('body').on('mouseenter', '.hover-gif', enterGif);
    $('body').on('mouseleave', '.hover-gif', leaveGif);

})(jQuery);