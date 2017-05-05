(function(global) {
	'use strict';

	/**
	 * Sets a key value pair in localStorage
	 * @param key The stored key
	 * @param value The stored value
	 */
	function setStorage (key, value) {
		if (global.localStorage && !!key && !!value) {
			global.localStorage.setItem(key, value);
		}
	}

	/**
	 * Gets a value from localStorage
	 * @param key The stored key
	 */
	function getStorage (key) {
		if (global.localStorage) {
			return global.localStorage.getItem(key);
		}
		return null;
	}

	/**
	 * Removes a key from localStorage
	 * @param key The stored key
	 */
	function deleteStorage (key) {
		if (global.localStorage) {
			return global.localStorage.removeItem(key);
		}
	}

	global.sd = global.sd || {};
	global.sd.storage = global.sd.storage || {};
	global.sd.storage.set = setStorage;
	global.sd.storage.get = getStorage;
	global.sd.storage.delete = deleteStorage;
})(window);
