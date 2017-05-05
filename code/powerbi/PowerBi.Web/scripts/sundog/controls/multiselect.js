(function ($, global) {
    'use strict';

    function createMultiSelect(element, collection, identifier) {
        var $element = element.siblings('.sd-multiselect').find('.sd-multiselect-options'),
            $label = $('<label />'),
            $labelTag = $('<span />'),
            $option = $('<input />', { 'type': 'checkbox' }),
            _tempOption,
            i = 0;

        for (i; i < collection.length; i++) {
            if ($(collection[i]).text() !== '') {
                _tempOption = $option
                        .clone(true).attr('value', i)
                        .attr('checked', $(collection[i]).css('display') !== 'none');

                // check option if it is still visible
                // if visible and not checked, trigger click to check it
                if ($(collection[i]).css('display') !== 'none' && !_tempOption.is(':checked')) {
                    _tempOption.trigger('click');
                }

                _tempOption.attr('disabled', $(collection[i]).hasClass('show-small'));

                $element.append(
                     $label.clone(true).append(_tempOption).append($labelTag.clone(true).html($(collection[i]).data('multiselect-override') || $(collection[i]).text()))
                );
            }
        }

        multiSelectEvents(element);

        $('.sd-multiselect[data-target="' + $(element).data('target') + '"]').on('change', 'input[type="checkbox"]', function () {
            $(window).trigger('multiselect-update.' + identifier, $('input', $element));
        });

        return $element;
    }

    function multiSelectEvents(element) {
        $('.sd-multiselect[data-target="' + $(element).data('target') + '"]').on('click', 'button', function () {
            var $this = $(this),
                $options = $this.siblings('.sd-multiselect-options');

            $this.toggleClass('active');
            $options.toggle();
        });

        $('body').on('click.close-multiselect', function (e) {
            if ($(e.target).parents('.sd-multiselect').length === 0) {
                $('.sd-multiselect-options').css('display', 'none');
                $('.sd-multiselect button').removeClass('active');
            }
        });
    }

    global.sd = global.sd || {};
    global.sd.multiselect = createMultiSelect;
})(jQuery, window);