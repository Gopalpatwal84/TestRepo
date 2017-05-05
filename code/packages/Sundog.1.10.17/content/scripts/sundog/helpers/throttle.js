(function(global) {
	'use strict';

	var throttle = function (fn, timeout) {
		var lastCall = 0;
		return function() {
			var now = +new Date();
			if (lastCall + timeout <= now) {
				lastCall = now;
				fn.apply(null, arguments);
			}
		};
	};

	global.sd = global.sd || {};
	global.sd.throttle = throttle;
})(window);