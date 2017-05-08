(function(global) {
	'use strict';

	var debounce = function (fn, timeout) {
		var id;
		return function() {
			var args = arguments;
			clearTimeout(id);
			id = setTimeout(function() {
				fn.apply(null, args);
			}, timeout);
		};
	};

	global.sd = global.sd || {};
	global.sd.debounce = debounce;
})(window);