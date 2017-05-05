(function ($, ampPluginOptions) {
	'use strict';

	var ESC_KEY = 27;

	function playInSection(e) {
		e.preventDefault();

		var $el = $(e.currentTarget),
			$section = $($el.parents('.section')[0]),
			sectionPos = $section.offset().top,
			src = $el.data('src'),
			slug = $el.data('slug'),
			title = $el.data('title'),
			player = $el.data('player') || 'default',
			apectRatio = 16 / 9,

			height = $section.outerHeight(),
			width = height * apectRatio,
			windowWidth = $(window).outerWidth(),

			amPlayer,
			$video,
			$videoContainer = $('<div />', {
				'class': 'sd-playinsection-container'
			}),
			videoId = 'sd-playinsection-video-' + sectionPos,
			$mask = $('<div />', {
				'class': 'sd-playinsection-mask',
				'tabindex': '-1'
			}),
			$closeBtnText = $el.attr('data-close-text') || 'Close',
			$close = $('<button />', {
				'class': 'sd-playinsection-close',
				'aria-label': $closeBtnText
			}),
			ampOptions = {};

		$el.attr('data-state', 'opened');

		if (width > windowWidth) {
			width = windowWidth;
			height = width / apectRatio;
		}

		if (player === 'default' || player === 'ch9') {
			$video = $('<iframe />', {
				id: videoId,
				'class': 'sd-playinsection-embed',
				allowFullScreen: true,
				frameBorder: 0,
				height: height,
				src: src + '#autoplay',
				width: width
			});

			addVideo();
		} else if (player === 'ams') {
			$video = $('<video />', {
				id: videoId,
				'class': 'azuremediaplayer amp-default-skin amp-big-play-centered sd-playinsection-embed'
			});

			addVideo();

			ampOptions = {
				nativeControlsForTouch: false,
				autoplay: true,
				controls: true,
				width: width,
				height: height,
				logo: {
					enabled: false
				},
				poster: ''
			};

			if (!!ampPluginOptions) {
				ampOptions.plugins = ampPluginOptions({
					slug: slug,
					title: title,
					autoplay: true
				});
			}

			amPlayer = amp(videoId, ampOptions);

			amPlayer.src([{
				src: src,
				type: 'application/vnd.ms-sstr+xml'
			}]);
		}

		function closeVideo(e) {
			e.preventDefault();

			var $closingSection = $(e.currentTarget).parents('.section'),
				$closingVideo = $closingSection.find('.sd-playinsection-embed');

			$closingSection.find('.sd-playinsection-close').remove();
			$closingSection.find('.sd-playinsection-mask').remove();

			// In some browsers removing an iframe loads the src of the iframe
			// another time -- the following helps prevent that
			$closingVideo.attr('src', '');
			setTimeout(function () {
				$closingVideo.remove();
			}, 0);

			if (amPlayer && amPlayer.dispose) {
				amPlayer.dispose();
			}

			$('body').off('keydown.playinsection');
			$el.removeAttr('data-state');
		}

		function addVideo() {
			$section.append($mask)
				.append($close)
				.append($videoContainer.append($video));

			$close.css({
				marginLeft: (width / 2) + 'px'
			});
			$mask.focus();
		}

		$close.on('click', closeVideo);
		$('body').on('keydown.playinsection', function (e) {
			if (e.which === ESC_KEY) {
				$close.trigger('click');
			}
		});
		$mask.on('click', closeVideo);
	}

	$('body').on('click.playinsection', '.sd-playinsection[data-state!="opened"]', playInSection);
})(jQuery, window.sd.ampPluginOptions);