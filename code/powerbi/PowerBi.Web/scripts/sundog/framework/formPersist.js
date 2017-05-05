(function ($, global, storage, qs2object) {
    'use strict';

    var localStorageKey = 'form',
        ignoredFields = ['__RequestVerificationToken', 'Comments', 'ContactConsent', 'FormOptions.Slug', 'FormOptions.SubmitRedirectUrl', 'FormOptions.SubmitView'];

    if (sd._variables && sd._variables.ignoredFields) {
        ignoredFields = ignoredFields.concat(sd._variables.ignoredFields);
    }

    /**
     * Save form data to localStorage
     */
    function formSave(e) {
        var form = $(e.currentTarget),
            existingFormData = JSON.parse(storage.get(localStorageKey)),
            formData = qs2object(form.serialize()),
            i;

        for (i = 0; i < ignoredFields.length; i++) {
            delete formData[ignoredFields[i]];
        }

        if (existingFormData) {
            $.extend(true, existingFormData, formData);
        } else {
            existingFormData = formData;
        }

        storage.set(localStorageKey, JSON.stringify(existingFormData));
    }

    /**
     * Set form data from localStorage
     */
    function formLoad() {
        var formData = JSON.parse(storage.get(localStorageKey)),
            storedField,
            $field,
            value;

        for (storedField in formData) {
            if (formData.hasOwnProperty(storedField)) {
                value = decodeURIComponent(formData[storedField]);
                $field = $('form [name="' + storedField + '"]');
                $field.val(value);
                $field.trigger('change');
            }
        }
    }

    if (storage && $('form').length) {
        $('body').on('submit', 'form', formSave);
        $(formLoad);
    }
})(jQuery, window, window.sd.storage, window.sd.qs2object);

