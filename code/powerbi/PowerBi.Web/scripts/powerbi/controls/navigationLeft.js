$(function () {
    if (!!$('.navigationLeft-moreless')) {
        var $open = $('.toggler-open'),
            $close = $('.toggler-close');

        $open.on('click', function () {
            $(this).hide();
            $(this).siblings($close).show();
        });

        $close.on('click', function () {
            $(this).siblings('.toggled.open').hide();
            $(this).hide();
            $(this).prev($open).show();
        });
    }
});
