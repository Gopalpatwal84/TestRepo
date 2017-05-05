(function ($, global) {
    'use strict';

    /**
     * Submitting an AJAX form
     * @param e submit event
     */
     function formSubmit(e) {
        e.preventDefault();

        var $form = $(e.currentTarget),
            formData = $form.serializeArray(),
            formUrl = $form.attr('action'),
            formVerb = $form.attr('method') || 'GET',
            ajaxReplace = $form.closest('.form-replace'),
            formButton = $form.find("button[type=submit][clicked=true][name][value]");

        if (!ajaxReplace.length) {
            ajaxReplace = $form;
        }

        if (!!formButton.length) {
            formData.push({ name: formButton.attr('name'), value: formButton.val() });
        }

        $.ajax(formUrl, {
            data: formData,
            type: formVerb
        }).done(function (data) {
            if (!!data.Redirect) {
                $(window).trigger('form-submitted', $form);
                global.location = data.Redirect;
            } else {
                ajaxReplace.html(data);
                if (!ajaxReplace.find('.field-validation-error').length) {
                    $(window).trigger('form-submitted', $form);
                } else {
                    $(window).trigger('form-invalid-serverside', $form);
                }
            }
        }).fail(function (jqXHR) {
            var error = $('<p />', {
                'class': 'error-text',
                text: $form.data('error')
            });

            if (jqXHR.status === 400) {
                ajaxReplace.html(jqXHR.responseText)

                // custom event trigger for WEDCS
                $(window).trigger('form-invalid-serverside', $form);
            } else {
                $form.find('.error-text').remove();
                $form.append(error);
            }
        });
    }

    function setButtonClicked(e) {
        $("form button[type=submit]", $(this).parents("form")).removeAttr("clicked");
        $(this).attr("clicked", "true");
    }

    $('body').on('submit', 'form', formSubmit);
    $('body').on('click', 'form button[type=submit]', setButtonClicked);
})(jQuery, window);
