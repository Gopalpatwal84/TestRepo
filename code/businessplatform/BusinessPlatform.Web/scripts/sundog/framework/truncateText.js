(function (sd) {
	'use strict';

	var truncateTextDelay = 500;

	/**
	* Return if text overflows the element
	* @param element the element in question
	* @param text the text to fit in the element
	* @returns bool will the text fit
	*/
	function isTextTooBig(element, text) {
		$(element).text(text);

		while (element && (!element.clientHeight)) {
			element = $(element).parent()[0];
		}

		while (element && (!element.scrollHeight)) {
			element = $(element).parent()[0];
		}

		if (!element) {
			return false;
		}

		if (element.clientHeight < element.scrollHeight - 1) {
			return true;
		}

		return false;
	}

	/**
	* Truncate the text to fit within the element
	* @param element the element in question
	* @param longText the text to fit in the element
	*/
	function truncateText(element, longText) {
		var ellipsis = '\u2026',
			low = 0,
			high = longText.length,
			pos = Math.floor(high / 2),
			textSlice = longText.slice(0, pos),
			leftSideBad = isTextTooBig(element, textSlice + ellipsis),
			rightSideBad = leftSideBad ? false : isTextTooBig(element, textSlice + longText[pos] + ellipsis);

		if (!isTextTooBig(element, longText)) {
			return;
		}

		while (leftSideBad || !rightSideBad) {
			if (leftSideBad) {
				high = pos;
				pos = Math.floor(low + (high - low) / 2);
			} else if (!rightSideBad) {
				low = pos;
				pos = Math.floor(low + (high - low) / 2);
			}
			textSlice = longText.slice(0, pos);

			leftSideBad = isTextTooBig(element, textSlice + ellipsis);
			rightSideBad = leftSideBad ? false : isTextTooBig(element, textSlice + longText[pos] + ellipsis);

			if (high - low <= 2) {
				break;
			}
		}

		$(element)
			.text(textSlice + ellipsis)
			.attr('title', longText)
			.attr('aria-label', longText)
			.data('truncate-full', longText);
	}

	/**
	* Truncate the text to fit within the element
	* @param index the text to fit in the element
	* @param element the element in question
	*/
	function iterateTruncate(index, element) {
		var $element = $(element),
			text = $element.data('truncate-full') || $element.text();
		truncateText(element, text);
	}

	// On update
	$('body').on('update.truncatetext', '.sd-truncateText', function (e) {
		truncateText(e.currentTarget, $(e.currentTarget).text());
	});

	// On resize
	$(window).on('resize', sd.debounce(function () {
		$('.sd-truncateText').each(iterateTruncate);
	}, truncateTextDelay));

	// Custom event listener
    $('body').on('truncate.sundog', function () {
        $('.sd-truncateText').each(iterateTruncate);
    });

	$('.sd-truncateText').each(iterateTruncate);

})(sd);
