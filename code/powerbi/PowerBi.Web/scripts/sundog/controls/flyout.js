(function ($, global, sd) {
    'use strict';

    var $triggers,
        $flyouts,
        OFFSET = 10,
        clickCoordinateLeft,
        clickCoordinateTop;

    function cloneFlyouts() {
        var $flyout;

        $triggers.each(function() {
            $flyout = $('#' + $(this).attr('aria-describedby'));
            $('body').append($('<div />').append($flyout));
        });
    }

    function addEventListeners() {
        $('body').on('click', '.sd-flyout', function(e){
            e.stopPropagation();
        });

        $('body').on('click', '.diagram-flyout-link', function(e) {
            e.preventDefault();
            e.stopPropagation();
        });

        $('body').on('click.sd-flyout', '[aria-describedby^="flyout"]', function(e) {
            e.stopPropagation();
            e.preventDefault();
            clickCoordinateLeft = e.pageX;
            clickCoordinateTop = e.pageY;
            toggleFlyout($(this));
        });

        $('body').on('click', '.sd-flyout .close', function (e) {
            e.stopPropagation();
            e.preventDefault();
            if ($('[aria-hidden=false]').length > 0) {
                hideAllFlyouts();
            }
        });

        $('body').on('click', function() {
            if ($('[aria-hidden=false]').length > 0) {
                hideAllFlyouts();
            }
        });

        $(global).on('resize', sd.debounce(hideAllFlyouts, 50));
    }

    function hideAllFlyouts() {
        $('[aria-hidden=false]').attr('aria-hidden', 'true');
    }

    function toggleFlyout($trigger) {
        var $flyout = $('.sd-flyout#' + $trigger.attr('aria-describedby'));
        hideAllFlyouts();

        if ($flyout.attr('aria-hidden') === 'true') {
            positionFlyout($trigger, $flyout, clickCoordinateLeft, clickCoordinateTop);
            $flyout.attr('aria-hidden', 'false');
        } else {
            $flyout.attr('aria-hidden', 'true');
        }
    }

    function positionFlyout($trigger, $flyout, leftCoordinate, topCoordinate) {
        var el = document.getElementById($trigger.attr('id')),
            triggerBox = !!el ? el.getBoundingClientRect() : null,
            triggerOffset = $trigger.offset(),
            flyoutY = triggerOffset.top + ($trigger.outerHeight() / 2),
            flyoutX;

        $flyout.removeAttr('style');

        if (global.outerWidth <= window.sd.breakpoints.MEDIUM) {
            $flyout.css('margin', '4%');
            flyoutY = topCoordinate ? topCoordinate : flyoutY;
        } else if ($flyout.hasClass('sd-flyout-corner')) { // handle corner positioning
            if (typeof leftCoordinate === 'undefined' && typeof topCoordinate === 'undefined') {
                flyoutX = triggerOffset.left + ((triggerBox.width || $trigger.outerWidth()) / 2);
                flyoutY = triggerOffset.top + ((triggerBox.height || $trigger.outerHeight()) / 2);
            } else {
                flyoutX = checkLeftPosition(leftCoordinate);
                flyoutY = topCoordinate ? topCoordinate : flyoutY;
            }
        } else if ($flyout.hasClass('sd-flyout-left')) { // handle left positioning
            flyoutX = triggerOffset.left - OFFSET;
        } else { // default right positioning
            flyoutX = triggerOffset.left + $trigger.outerWidth() + OFFSET;
        }

        $flyout.css({
            'left': flyoutX,
            'top': flyoutY
        });
    }

    function checkLeftPosition(leftPosition) {
        return leftPosition < 15 ? 15 : leftPosition;
    }

    $(function () {
        $triggers = $('[aria-describedby^="flyout"]');
        $flyouts = $('.sd-flyout');

        if ($triggers.length > 0 && $flyouts.length > 0) {
            cloneFlyouts();
            addEventListeners();
        }
    });
})(jQuery, window, sd);
