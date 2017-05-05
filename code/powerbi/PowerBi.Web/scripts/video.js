(function () {
    function setupInlineVideo() {
        $('.videoPlaceholder').on('click', function (e) {
            e.preventDefault();

            var $this = $(this),
                $frame = $this.find('.inlineVideo'),
                $wallpaper = $this.find('.placeholderScreenshot'),
                height = $wallpaper.height(),
                width = $wallpaper.width(),
                page = $this.data('page');

            $frame.attr({
                'height': height,
                'width': width,
                'src': $frame.data('src')
            });

            $this.find('.icon').hide();
            $wallpaper.hide();
            $frame.show();

            window.onyx.analytics.getData(page + '-played-video');
        });
    }

    setupInlineVideo();

    $(document).ready(function () {
        if (window.amp) {
            $('.article-video').each(function () {

                var $video = $(this),
                    id = $video.attr('id'),
                    src = $video.attr('data-src'),
                    amPlayer = amp(id, {
                        nativeControlsForTouch: false,
                        autoplay: false,
                        controls: true,
                        width: '100%',
                        height: 'auto',
                        logo: {
                            enabled: false
                        },
                        poster: ''
                    });

                amPlayer.src([{
                    src: src,
                    type: 'application/vnd.ms-sstr+xml'
                }]);
            });
        }
    });


})();