(function ($, global) {
    'use strict';

    var defaultDecimals = 2,
        activeCurrency;

    function updatePrices() {
        var selection = $(this).val();

        activeCurrency = global.rawCurrencyData[selection];

        $('.price-data').each(function () {
            var element = $(this),
                price = element.data('amount'),
                decimals = element.attr('data-decimals') ? parseInt(element.data('decimals'), 10) : defaultDecimals;

            price = price * activeCurrency.Conversion;
            price = localizeNumber(price, decimals);
            price = addGlyph(price);

            element.html(price);
        });
    }

    function localizeNumber(number, decimals) {

        if (number > 10 && decimals > 2) {
            decimals = 2;
        }

        //remove decimals from currencies that generally wouldn't use 'cents'
        if (activeCurrency.Conversion > 50 && decimals > 2) {
            decimals = 2;
        }

        if (number % 1 === 0) {
            decimals = 0;
        }

        var minDecimals = decimals > 0 ? Math.min(decimals, 2) : 0;
        var formatter = global.Intl.NumberFormat(global.currentCulture, {
            maximumFractionDigits: decimals,
            minimumFractionDigits: minDecimals
        });

        return formatter.format(number);
    }

    function addGlyph(price) {
        var leadingGlyph = true;

        if (typeof (activeCurrency.InvertGlyph) != 'undefined' && activeCurrency.InvertGlyph) {
            leadingGlyph = false;
        }

        if (global.currentCulture === 'fr-fr' && activeCurrency.Slug === 'EUR') {
            leadingGlyph = false;
        }

        return (leadingGlyph ? activeCurrency.Glyph + price : price + '&nbsp;' + activeCurrency.Glyph);
    }

    $('body').on('change', '.currency-dropdown', updatePrices);
    $('.currency-dropdown').first().trigger('change');
})(jQuery, window);