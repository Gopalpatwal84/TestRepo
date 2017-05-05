(function ($, sd) {
	'use strict';

	var $window = $(window),
		$images = $('.fade-img'),
		windowHeight = $window.height(),
		percentVisibleToFade = 0.333,
		fadeOffset = 200;

	function onScroll () {
		var windowScrollTop = $window.scrollTop(),
			imageHeight,
			imageOffsetTop;

		$images.each(function () {
			imageHeight = $(this).height();
			imageOffsetTop = $(this).offset().top;

			if (windowScrollTop + windowHeight > imageOffsetTop + (imageHeight * percentVisibleToFade) - fadeOffset) {
				$(this).addClass('fade-in');
			}
		});
	}

	function onResize() {
		windowHeight = $window.height();
	}

	onScroll();

	$window.on('scroll', onScroll);
	$window.on('resize', sd.debounce(onResize, 200));
})(jQuery, sd);
