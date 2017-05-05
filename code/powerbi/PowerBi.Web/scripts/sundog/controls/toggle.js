(function ($) {
    'use strict';

    function toggle(e) {
        e.preventDefault();

        var $toggler = $(this),
            set = $toggler.data('set'),
            force = $toggler.data('force'),
            $toggleds = $('.toggled[data-set="' + set + '"]'),
            singleOnlyGroup,
            $singleOnlyToggleds;

        if (!$toggleds.length) {
            $toggleds = $toggler.siblings('.toggled');
        }

        singleOnlyGroup = $toggleds.data('single-only');
        $singleOnlyToggleds = $('.toggled[data-single-only="' + singleOnlyGroup + '"]');

        if($singleOnlyToggleds.length){
            setToggle('close', $toggler);
            $singleOnlyToggleds.not($toggleds).each(toggleEach);
        }

        if ($toggler.data('hide-toggler') === true) {
            $toggler.hide();
        }

        setToggle(force, $toggler);
        $toggleds.each(toggleEach);
    }

    function setToggle(dataForce, $toggler) {
        switch (dataForce) {
            case 'open':
                $.fn.sundogToggle = $.fn.addClass;
                $.fn.sundogSlideToggle = $.fn.slideDown;
                $toggler.addClass('open');
                break;

            case 'close':
                $.fn.sundogToggle = $.fn.removeClass;
                $.fn.sundogSlideToggle = $.fn.slideUp;
                $toggler.removeClass('open');
                break;

            default:
                $.fn.sundogToggle = $.fn.toggleClass;
                $.fn.sundogSlideToggle = $.fn.slideToggle;
                $toggler.toggleClass('open');
        }
    }

    function toggleEach() {
        var $this = $(this),
            animation = $this.data('animation');

        switch (animation) {
            case 'slide':
                $this.sundogSlideToggle(250);
                break;

            default:
                $this.sundogToggle('open');
        }
    }

    $(function () {
        $('body').on('click', '.toggler', toggle);
    });
})(jQuery);
