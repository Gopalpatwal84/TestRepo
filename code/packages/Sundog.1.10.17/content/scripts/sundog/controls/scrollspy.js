/*
 * Override scrollOffset with sd._variables.scrollOffset
 * Dependency: sd.debounce.js
 */

(function ($, sd) {
	'use strict';

	var $window = $(window),
		$document = $(document);

	function scrollSpy() {
		var $el = $(this),
			$items = $el.find('li a[href]'),
			$parent = $el.parents('.column'),
			scrollOffset = $el.data('scroll-offset') || (sd._variables) && sd._variables.scrollOffset || 0,
			active = 0;

		function onScroll() {
			var scroll = $document.scrollTop() - scrollOffset;

			active = 0;

			$items.each(function (index) {
				var $el = $(this.hash),
					top = Infinity;

				if ($el.length) {
					top = $el.offset().top;
				}

				if (scroll > top) {
					active = index;
				}
			});

			$items.parent().removeClass('active');

			if (active >= 0) {
				$items.eq(active).parent().addClass('active');
			}
		}

		function onResize() {
			$el.width($parent.innerWidth());
		}

		onScroll();
		onResize();

		$window.on('scroll', onScroll);
		$window.on('resize', sd.debounce(onResize, 200));
	}

	$('.scrollspy').each(scrollSpy);
})(jQuery, window.sd || {});
