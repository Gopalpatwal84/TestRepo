(function ($) {
    'use strict';

    function checkForActiveTabs () {
        var $tabs = $('.sd-tabs');
        $tabs.each(function () {
            var $this = $(this),
                anchors = $this.find('li > a.active');

            if (!anchors.length) {
                $this.children().first().children('a').trigger('click');
            }
        });
    }

    function tabsControl (e) {
        e.preventDefault();
        var $this = $(this),
            slug = $this.attr('href'),
            $target = $(slug),
            $tabs = $this.parents('.sd-tabs').find('li > a'),
            $containers = $target.parents('.tabs-content').children('div[id]');

        if (!$this.hasClass('active')) {
            $tabs.removeClass('active');
            $containers.removeClass('active');
            $this.addClass('active');
            $target.addClass('active');
        } else {
            if (!$target.hasClass('active')) {
                $containers.removeClass('active');
                $target.addClass('active');
            }
        }
    }

    $('body').on('click', '.sd-tabs > li > a', tabsControl);
    $(document).ready(checkForActiveTabs);
})(jQuery);