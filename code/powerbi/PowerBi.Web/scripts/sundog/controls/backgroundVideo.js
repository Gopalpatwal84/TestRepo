(function($) {
    'use strict';

    $(function() {

        /*
         * Check if video element is supported
         * Remove element to display underlying background
         */
        if (isVideoSupported()) {
            $(window).on('resize.background-video', resizeVideos);
            $('video').on('loadeddata', function() {
                resizeVideos();
                addVideoControls();
            });
        } else {
            $('.background-video-holder').remove();
        }
    });

    function isVideoSupported() {
        return !!document.createElement('video').canPlayType;
    }

    function addVideoControls() {
        var $videoHolders = $('.background-video-holder');

        $videoHolders.each(function() {
            var $videoHolder = $(this),
                $videoControls = $('<div />', { class: 'background-video-controls' });

            $videoControls.append($('<div />', { class: 'background-video-play', html: '<svg viewBox="0 0 29.9 29.9"><path d="M.164,15.209a17.524,17.524,0,0,1,.5-4,14.392,14.392,0,0,1,1.5-3.6,13.307,13.307,0,0,1,2.3-3,17.554,17.554,0,0,1,3-2.4,15.108,15.108,0,0,1,3.7-1.5,13.309,13.309,0,0,1,4-.5,17.524,17.524,0,0,1,4,.5,14.392,14.392,0,0,1,3.6,1.5,11.329,11.329,0,0,1,3,2.4,13.307,13.307,0,0,1,2.3,3,14.392,14.392,0,0,1,1.5,3.6,13.309,13.309,0,0,1,.5,4,17.524,17.524,0,0,1-.5,4,19.426,19.426,0,0,1-1.5,3.6,13.307,13.307,0,0,1-2.3,3,13.307,13.307,0,0,1-3,2.3,14.392,14.392,0,0,1-3.6,1.5,13.309,13.309,0,0,1-4,.5,17.524,17.524,0,0,1-4-.5,19.426,19.426,0,0,1-3.6-1.5,13.307,13.307,0,0,1-3-2.3,13.307,13.307,0,0,1-2.3-3,14.392,14.392,0,0,1-1.5-3.6A17.563,17.563,0,0,1,.164,15.209Zm15-13.1a13.449,13.449,0,0,0-3.5.5,16.924,16.924,0,0,0-3.1,1.3,12.84,12.84,0,0,0-2.6,2,12.055,12.055,0,0,0-2,2.6,14.429,14.429,0,0,0-1.3,3.1,14.272,14.272,0,0,0-.6,3.6,13.449,13.449,0,0,0,.5,3.5,12,12,0,0,0,1.3,3.1,12.84,12.84,0,0,0,2,2.6,12.055,12.055,0,0,0,2.6,2,12,12,0,0,0,3.1,1.3,13.449,13.449,0,0,0,3.5.5,13.449,13.449,0,0,0,3.5-.5,10.7,10.7,0,0,0,3.1-1.3,12.84,12.84,0,0,0,2.6-2,12.055,12.055,0,0,0,2-2.6,14.429,14.429,0,0,0,1.3-3.1,13.449,13.449,0,0,0,.5-3.5,13.449,13.449,0,0,0-.5-3.5,10.7,10.7,0,0,0-1.3-3.1,16.38,16.38,0,0,0-2-2.6,12.055,12.055,0,0,0-2.6-2,12,12,0,0,0-3.1-1.3A9.342,9.342,0,0,0,15.164,2.109Z" transform="translate(-0.164 -0.209)" /><path d="M10.864,10.359l8.5,4.8-8.5,4.8Z" transform="translate(-0.164 -0.209)" /></svg>' }));
            $videoControls.append($('<div />', { class: 'background-video-pause', html: '<svg viewBox="0 0 29.9 29.9"><path d="M.164,15.267a17.524,17.524,0,0,1,.5-4,14.392,14.392,0,0,1,1.5-3.6,13.307,13.307,0,0,1,2.3-3,17.554,17.554,0,0,1,3-2.4,15.108,15.108,0,0,1,3.7-1.5,13.309,13.309,0,0,1,4-.5,17.524,17.524,0,0,1,4,.5,14.392,14.392,0,0,1,3.6,1.5,11.329,11.329,0,0,1,3,2.4,13.307,13.307,0,0,1,2.3,3,14.392,14.392,0,0,1,1.5,3.6,13.309,13.309,0,0,1,.5,4,17.524,17.524,0,0,1-.5,4,19.426,19.426,0,0,1-1.5,3.6,13.307,13.307,0,0,1-2.3,3,13.307,13.307,0,0,1-3,2.3,14.392,14.392,0,0,1-3.6,1.5,13.309,13.309,0,0,1-4,.5,17.524,17.524,0,0,1-4-.5,19.426,19.426,0,0,1-3.6-1.5,13.307,13.307,0,0,1-3-2.3,13.307,13.307,0,0,1-2.3-3,14.392,14.392,0,0,1-1.5-3.6A17.563,17.563,0,0,1,.164,15.267Zm15-13.1a13.449,13.449,0,0,0-3.5.5,16.924,16.924,0,0,0-3.1,1.3,12.84,12.84,0,0,0-2.6,2,12.055,12.055,0,0,0-2,2.6,14.429,14.429,0,0,0-1.3,3.1,14.272,14.272,0,0,0-.6,3.6,13.449,13.449,0,0,0,.5,3.5,12,12,0,0,0,1.3,3.1,12.84,12.84,0,0,0,2,2.6,12.055,12.055,0,0,0,2.6,2,12,12,0,0,0,3.1,1.3,13.449,13.449,0,0,0,3.5.5,13.449,13.449,0,0,0,3.5-.5,10.7,10.7,0,0,0,3.1-1.3,12.84,12.84,0,0,0,2.6-2,12.055,12.055,0,0,0,2-2.6,14.429,14.429,0,0,0,1.3-3.1,13.449,13.449,0,0,0,.5-3.5,13.449,13.449,0,0,0-.5-3.5,10.7,10.7,0,0,0-1.3-3.1,16.38,16.38,0,0,0-2-2.6,12.055,12.055,0,0,0-2.6-2,12,12,0,0,0-3.1-1.3A9.342,9.342,0,0,0,15.164,2.167Z" transform="translate(-0.164 -0.267)" /><rect x="12.854" y="10.181" width="1.458" height="9.538" rx="0.459" ry="0.459" /><rect x="14.954" y="10.181" width="1.458" height="9.538" rx="0.459" ry="0.459" /></svg>' }));

            $videoHolder.append($videoControls);
        });

        $videoHolders.on('click.video-controls', '.background-video-controls', function() {
            var video = $(this).siblings('video').get(0),
                $videoHolder = $(video).parent('.background-video-holder');

            if (video.paused) {
                video.play();
                $videoHolder.removeClass('paused');
            } else {
                video.pause();
                $videoHolder.addClass('paused');
            }
        });
    }

    function resizeVideos() {
        $('.background-video-holder').each(function() {
            var $videoHolder = $(this),
                $videoElement = $videoHolder.find('video'),
                videoRatio = $videoElement.width() / $videoElement.height(),
                videoHolderRatio = $videoHolder.outerWidth(true) / $videoHolder.outerHeight(true);

            if (videoHolderRatio < videoRatio) {
                $videoElement.css({
                    'height': '100%',
                    'width': 'auto'
                });
            } else {
                $videoElement.css({
                    'height': 'auto',
                    'width': '100%'
                });
            }

            $videoHolder.animate({'opacity': 1}, 200);
        });
    }
})(jQuery);
