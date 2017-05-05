(function (jQuery) {

    jQuery.validator.addMethod('needsChecked', function (value) {
        return value;
    }, '');

    jQuery.validator.unobtrusive.adapters.add('needschecked', {}, function (options) {
        options.rules['needsChecked'] = true;
        options.messages['needsChecked'] = options.message;
    });

    jQuery.validator.addMethod('disallowedtldemail', function (value, element, params) {
        var tlds = params.tlds.split(','),
            isValid = true;

        $.each(tlds, function (index, tld) {
            if (value.match(tld + '$')) {
                isValid = false;
            }
        });

        $(element).attr('data-val-disallowedtldemail-isvalid', isValid);
        return isValid;
    }, '');

    jQuery.validator.unobtrusive.adapters.add('disallowedtldemail', ['tlds'], function (options) {
        var params = {
            tlds: options.params.tlds
        };
        options.rules['disallowedtldemail'] = params;
        options.messages['disallowedtldemail'] = options.message;
    });
})(jQuery);