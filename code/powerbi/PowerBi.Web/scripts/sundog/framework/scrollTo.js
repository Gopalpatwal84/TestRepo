(function ($) {
	'use strict';

	var $scrollLinks = $('[data-scroll]');

	function scrollTo(e) {
		e.preventDefault();

		var scrollId = '#' + $(e.currentTarget).data('scroll'),
			$scrollElement = $(scrollId),
			$scrollInputs = $scrollElement.find('input');

		$('html, body').stop().animate({
			scrollTop: $scrollElement.offset().top - $('header').height()
		}, 1000, function () {
			if ($scrollInputs.length) {
				$scrollInputs[0].focus();
			}
		});
	}

	$scrollLinks.on('click', scrollTo);
})(jQuery);
