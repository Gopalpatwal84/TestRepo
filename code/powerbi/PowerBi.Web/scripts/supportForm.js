(function ($, global) {
    'use strict';

    function refreshCategories(value) {
        $('form[data-form-type=support-pro-ticket] #Category > option[value!=""]').wrap('<span></span>');
        $('form[data-form-type=support-pro-ticket] #Category span option[value^="' + value + '."]').unwrap();
        $('form[data-form-type=support-pro-ticket] #Category').val('');
    }

    function renderQuestions(value) {
        var $form = $('form[data-form-type=support-pro-ticket]'),
            $questionsBlock = $form.find('#ticket-questions'),
            url = '/Form/Support/Questions';

        if (value) {
            url += '?problemType=' + value;
        }

        $form.find('button[type=submit]').prop('disabled', true);
        $.ajax(url, {
            type: 'get'
        }).done(function (data) {
            $questionsBlock.html(data);

            $form.on('change', 'select.withOther', function () {
                if ($(this).find('option:last').is(':selected')) {
                    $(this).nextAll('.otherValue').show().prop('disabled', false);
                } else {
                    $(this).nextAll('.otherValue').hide().prop('disabled', true);
                }
            });

            $form.removeData('validator').removeData('unobtrusiveValidation');
            $.validator.unobtrusive.parse($form);

            $form.find('button[type=submit]').prop('disabled', false);
        }).fail(function (jqXHR) {
            var error = $('<p />', {
                'class': 'error-text',
                text: $questionsBlock.data('error')
            });
            $questionsBlock.html(error);
        });
    }

    refreshCategories('');
    $('body').on('change', 'form[data-form-type=support-pro-ticket] #ProblemType', function () {
        refreshCategories(this.value);
        renderQuestions(this.value);
    });

})(jQuery, window);