(function ($, global, debounce) {
    'use strict';

    function activatemodal(e, modalId) {
        e.preventDefault();
        setScrollbar(getScrollbarWidth());

        var $modal = $('#' + modalId),
            $modalWrapper = $modal.parents('.modal-wrapper-mask');    

        closeclick();

        if ($modal.hasClass('modal-video')) {
            var video = $modal.data('video-embed');
            setupvideo($modal, video);
        }

        $modal.addClass('visible');
        $modalWrapper.addClass('visible');
        $('html').addClass('modal-visible');
        $('body').trigger('equalize.sundog');
        checkModalSize($modal);
        $modal.find('input, select, textarea').filter(':visible').first().focus();
    }

    function checkModalSize($modal) {
        var modalHeight = $modal.height(),
            viewportHeight = global.innerHeight ? global.innerHeight : 0;

        if (modalHeight >= viewportHeight) {
            $modal.addClass('oversized');
        }
    }

    function setupvideo($modal, src) {
        var width = $('.container .row').width(), // full row video modal at time of click
            height = 9 * width / 16,
            minHeightBuffer = 50;

        if (height > $(global).outerHeight() - minHeightBuffer) {
            height = $(global).outerHeight() - minHeightBuffer;
            width = 16 * height / 9;
        }

        $modal.css('height', height + 30) // 30px for the close button
              .css('width', width - (width % 2)) // must be even number to avoid whitespace
            .find('iframe')
                .attr('width', width)
                .attr('height', height)
                .attr('src', src);
    }

    function closeclick() {
        $('.modal').removeClass('visible');
        $('.modal-wrapper-mask').removeClass('visible');
        $('html').removeClass('modal-visible');

        if ($('.modal').hasClass('modal-video')) {
            $('.modal').find('iframe').attr('src', '');
        }
        $('html').css('padding-right', '');
    }

    function controlClicks(e) {
        e.stopPropagation();
    }

    function getScrollbarWidth() {
        return global.innerWidth ? (global.innerWidth - document.documentElement.clientWidth) : 0;
    }

    function setScrollbar(scrollWidth) {
        if (!!scrollWidth) {
            $('html').css('padding-right', scrollWidth);
        }
    }

    $(function() {
        $('.modal').wrap('<div class="modal-wrapper-mask"></div>');
        $('body').on('click', '[data-pops-up]', function(e){
            activatemodal(e, $(this).data('pops-up'));
        });
        $('body').on('click', '.modal-wrapper-mask', closeclick);
        $('body').on('click', '.modal .close', closeclick);
        $('body').on('click', '.modal', controlClicks);
        $('body').on('show.modal', activatemodal);
        $('body').on('hide.modal', closeclick);
        $(global).on('resize', debounce(function () {
            checkModalSize($('.modal.visible'));
        }, 50));
    });
})(jQuery, window, window.sd.debounce);