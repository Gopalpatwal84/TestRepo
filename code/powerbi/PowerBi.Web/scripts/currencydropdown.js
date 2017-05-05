(function ($, storage, defaultCurrency) {
    'use strict';

    var dropdownSelector = '.currency-dropdown',
        storageKey = 'currency';

    function currencyChanged() {
        var selection = $(this).val();

        storage.set(storageKey, selection);

        $(dropdownSelector).each(function (index, dropdown) {
            $(dropdown).val(selection);
        });
    }

    function firstTime() {
        var storageCurrency = storage.get(storageKey),
            $dropdowns = $(dropdownSelector);

        if ($dropdowns.find('option[value="' + storageCurrency + '"]').length) {
            $dropdowns.val(storageCurrency);
        } else if ($dropdowns.find('option[value="' + defaultCurrency + '"]').length) {
            storage.delete(storageKey);
            $dropdowns.val(defaultCurrency);
        } else {
            $dropdowns.val('USD');
        }
    }

    firstTime();

    // Bind the change event after document ready and after first time initializtion
    // That way we don't save settings the user didn't choose
    $(function () {
        $('body').on('change', dropdownSelector, currencyChanged);
    });
})(jQuery, window.sd.storage, window.defaultCurrency);