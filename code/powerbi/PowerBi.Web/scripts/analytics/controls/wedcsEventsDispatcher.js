$(function () {
    var $window = $(window),
        $currentForm,
        formData;


    $('.form-navigation button[type="submit"]').on('click', function () {
        $currentForm = $($(this).parents('form'));
        formData = $currentForm.find('[name="Email"]').val();
    });

    $window.on('form-invalid-serverside', function () {
        if (MscomCustomEvent) {
            MscomCustomEvent.apply($currentForm, ['wcs.cid=power-bi-signup-consumer-email']);
        }
    });

    $window.on('form-invalid-clientside', function () {
        if (MscomCustomEvent) {
            if (formData.substring(formData.length - 4) === '.gov' || formData.substring(formData.length - 4) === '.mil') {
                MscomCustomEvent.apply($currentForm, ['wcs.cid=power-bi-signup-blocked-email']);
            } else if (formData !== '') {
                MscomCustomEvent.apply($currentForm, ['wcs.cid=power-bi-signup-invalid-email']);
            } else {
                MscomCustomEvent.apply($currentForm, ['wcs.cid=power-bi-signup-empty-email']);
            }
        }
    });

    $window.on('form-submitted', function (event, form) {
        if (MscomCustomEvent) {
            var formtype = $(form).data('form-type') || 'form';
            MscomCustomEvent.apply($(form).find('button#power-bi-signup-click'), ['wcs.cid=power-bi-' + formtype + '-click']);
            var email = $(form).find('[name="Email"]').val();

            if (email && formtype === 'signup') {
                if (email.indexOf('@microsoft.com') > -1) {
                    MscomCustomEvent.apply(form, ['wcs.cid=power-bi-signup-navigate-internal']);
                } else {
                    MscomCustomEvent.apply(form, ['wcs.cid=power-bi-signup-navigate-external']);
                }
            }
        }
    });

    $('body').on('click.wedcs-event', '#free-sign-up', function () {
        if (MscomCustomEvent) {
            MscomCustomEvent.apply($(this), ['wcs.cid=power-bi-signup-navigate-external']);
        }
    });

    $('.docsFeedback').on('click', 'button[value="submit-yes"]', function () {
        MscomCustomEvent.apply($(this), ['wcs.cid=power-bi-doc-feedback-helpful-yes']);
    });
    $('.docsFeedback').on('click', 'button[value="submit-no"]', function () {
        MscomCustomEvent.apply($(this), ['wcs.cid=power-bi-doc-feedback-helpful-no']);
    });

    $('.guidedLearningFeedback').on('click', 'button[value="submit-yes"]', function () {
        MscomCustomEvent.apply($(this), ['wcs.cid=power-bi-guidedlearning-feedback-helpful-yes']);
    });
    $('.guidedLearningFeedback').on('click', 'button[value="submit-no"]', function () {
        MscomCustomEvent.apply($(this), ['wcs.cid=power-bi-guidedlearning-feedback-helpful-no']);
    });

    $('body').on('click', '.form-feedback button[value="submit-comment"]', function () {
        var cid = $(this).closest('section').hasClass('guidedLearningFeedback') ? 'power-bi-guidedlearning-verbatim-feedback' : 'power-bi-verbatim-feedback',
            comment = $(this).closest('.form-feedback').find('#Comments').val();
        if (comment) {
            MscomCustomEvent.apply($(this), ['wcs.cid=' + cid, 'ms.verbatim=' + comment]);
        }
    });



});