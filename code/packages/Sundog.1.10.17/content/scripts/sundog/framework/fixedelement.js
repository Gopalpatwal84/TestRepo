(function ($) {
	'use strict';

	function fixElement() {
		var $window = $(window),
			$el = $(this),
			$elParent = $el.parents('.fixed-parent'),
			elOffset = $el.data('custom-offset') || $el.offset().top,
			additionalSpacingTop = $el.data('top-spacing') || 90,
			additionalSpacingBottom = $el.data('bottom-spacing') || 90,
			restingPosition = $el.data('resting-position') || '';

		function onScroll() {
			var elHeight = $el.outerHeight(true);

			if ($window.scrollTop() > (elOffset - additionalSpacingTop)) {
				$el
					.css({
						'position': 'fixed',
						'top': additionalSpacingTop + 'px'
					})
					.addClass('fixed')
					.siblings('.wa-table').css('margin-top', (elHeight + 30) + 'px');
			}

			if ($window.scrollTop() < (elOffset - additionalSpacingTop)) {
				$el
					.css({
						position: restingPosition,
						top: ''
					})
					.removeClass('fixed')
					.siblings('.wa-table').css('margin-top', '0px');
			} else if ($window.scrollTop() + additionalSpacingBottom > ($elParent.offset().top + $elParent.outerHeight(true) - elHeight)) {
				$el.css({
					'position': 'absolute',
					'top': $elParent.outerHeight(true) - elHeight + 'px'
				});
			}
		}

		onScroll();

		$window.on('scroll', onScroll);
	}

	$('.fixed-element').each(fixElement);
})(jQuery);

