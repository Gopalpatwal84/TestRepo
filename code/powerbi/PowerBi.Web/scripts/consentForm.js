(function ($, global) {
    function initialize($form) {

        var $countryDropdown = $form.find('#Country'),
            $consentFields = $form.find('.country-consent'),
            $phoneFields = $form.find('#Phone').closest('.form-row'),
            $consent = $consentFields.find('#Consent');

        $consentFields.hide();
        if (!$('#Phone[data-countries~="' + $countryDropdown.val() + '"]').length) {
            $phoneFields.hide();
        }
        $consent.prop('checked', true);

        function countryChange() {
            var activeCountry = $countryDropdown.val();

            $countryDropdown.data('event-property', activeCountry);
            global.onyx.analytics.getData('global-selected-dropdown');

            if (!$('.country-consent[data-countries~="' + activeCountry + '"]').length) {
                $consentFields.hide();
                $consent.prop('checked', true);
            } else {
                $consentFields.show();
                $consent.prop('checked', false);
            }

            if ($('#Phone[data-countries~="' + activeCountry + '"]').length) {
                $phoneFields.show();
            } else {
                $phoneFields.hide();
            }
        }
        $countryDropdown.on('change', countryChange);
    }

    $('.consent-form').each(function (index, element) {
        initialize($(this));
    });
    $(global).on('form-invalid-serverside', function(e, form) { initialize($(form)); });

})(jQuery, window);