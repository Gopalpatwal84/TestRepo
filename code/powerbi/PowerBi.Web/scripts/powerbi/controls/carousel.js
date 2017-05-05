(function ($, global) {
    $(global).load(function () {

        //Checking & setting width for active vertical control underline
        var width;
        function checkWidth(selected) {
            if (selected == undefined) {
                selected = $('.owl-controls-vertical a.active .dot-label');
            } else {
                selected = $(selected);
            }

            width = selected.width();
        }

        checkWidth();
        $('.underline').css('width', width);

        $('body').on('click', '.owl-controls-vertical a', function () {
            var thisVerticalControl = $(this).children('.owl-controls-vertical .active .dot-label');
            checkWidth(thisVerticalControl);
            $('.underline').css('width', width);
        });


        //Publishtoweb-specific bug fixes to account for multiple instances of vertical carousel on same page
        //Either needs to be re-factored to support same functionality globally or pulled out once instances of the carousel on the page have been reduced.
        $('body').on('click', '.publishtoweb-subnav .subnavLink a', function () {
            var otherActiveControls = $('.owl-controls-vertical .active:not(.industry-tab.active .owl-controls-vertical .active)');
            otherActiveControls.removeClass('active');

            var thisVerticalControl = $('.industry-tab.active .owl-controls-vertical .active .dot-label');   
            thisVerticalControl = $(thisVerticalControl)

            checkWidth(thisVerticalControl);
            $('.underline').css('width', width);
        });
    })


    //Generally setting height of owl-stage-outer to match content height
    function setOuterHeight() {
        var $outerOwl = $('.owl-carousel-vertical .owl-stage-outer'),
            innerSectionHeight = $outerOwl.find('.owl-item.active section').outerHeight();

        $outerOwl.height(innerSectionHeight);
    }

    setTimeout(setOuterHeight, 300);



    // Publishtoweb-specific carousel initialization & bug fixes - short term fix for launch
    // Re-factor and clean-up in backlog

    //Sets owl-stage-outer container to height of content on click of Testimonials control
    var thisStageOuter;
    $('body').on('click', '.vertical-control-testimonials', function (event) {
        setTimeout(function () {
            var thisStageOuter = $(event.target).closest('.owl-controls-vertical');
            thisStageOuter = thisStageOuter.attr('id');
            thisStageOuter = thisStageOuter.slice(-1); //Grabbing number of specific control instance that was clicked
            thisStageOuter = '#owl-carousel-vertical' + thisStageOuter; //Building selector for corresponding vertical carousel

            var activeItemHeight = $(thisStageOuter + ' .owl-item.active section').outerHeight();
            activeItemHeight = activeItemHeight - 4;
            $(thisStageOuter + ' .owl-stage-outer').height(activeItemHeight);
        }, 200)
    });


    //Storing original height of Stories section for Nonprofits on load
    var originalCarouselHeight;
    setTimeout(function () {
        originalCarouselHeight = $('#owl-carousel-vertical1 > .owl-stage-outer').height();
    }, 300)


    //Initializing carousel for Nonprofits
    var $carousel = $('#owl-carousel-vertical1'),
        $dots = $('#owl-controls-vertical1 a');

    $carousel.owlCarousel({
        autoplay: false,
        items: 1,
        loop: true,
        animateOut: 'slideOutUp',
        animateIn: 'slideInUp',
        autoHeight: false,
        mouseDrag: false,
        touchDrag: false,
        pullDrag: false
    });

    $dots.on('click', function (e) {
        e.preventDefault();
        var $dot = $(this);

        $carousel.trigger('to.owl.carousel', $dot.data('index'));
        $dots.removeClass('active');
        $dot.addClass('active');
    });

    $dots.first().addClass('active');
    $carousel.trigger('to.owl.carousel', $dots.first().data('index'));


    //Ensuring tab behavior on mobile for Nonprofits
    $('#nonprofits-subnav').on('click', function () {
        $('#nonprofits-tab').css('display', 'block');
        $('.tabs-content .industry-tab:not(#nonprofits-tab)').css('display', 'none');

        var $carousel = $('#owl-carousel-vertical1'),
            $dots = $('#owl-controls-vertical1 a');

        $carousel.owlCarousel({
            autoplay: false,
            items: 1,
            loop: true,
            animateOut: 'slideOutUp',
            animateIn: 'slideInUp',
            autoHeight: true,
            mouseDrag: false,
            touchDrag: false,
            pullDrag: false
        });

        $dots.on('click', function (e) {
            e.preventDefault();
            var $dot = $(this);

            $carousel.trigger('to.owl.carousel', $dot.data('index'));
            $dots.removeClass('active');
            $dot.addClass('active');
        });

        $dots.first().addClass('active');
        $carousel.trigger('to.owl.carousel', $dots.first().data('index'));
    })


    //Sets owl-stage-outer container to height of original content on click of Stories control for Nonprofits section
    $('body').on('click', '#nonprofits-stories', function () {
        setTimeout(function () {
            $('#owl-carousel-vertical1 .owl-stage-outer').height(originalCarouselHeight);
        }, 200)
    })



    //Initializing carousel & ensuring tab behavior on mobile for Business
    $('#business-subnav').on('click', function () {
        $('#business-tab').css('display', 'block');
        $('.tabs-content .industry-tab:not(#business-tab)').css('display', 'none');

        var $carousel2 = $('#owl-carousel-vertical2'),
            $dots2 = $('#owl-controls-vertical2 a');

        $carousel2.owlCarousel({
            autoplay: false,
            items: 1,
            loop: true,
            animateOut: 'slideOutUp',
            animateIn: 'slideInUp',
            autoHeight: true,
            mouseDrag: false,
            touchDrag: false,
            pullDrag: false
        });

        $dots2.on('click', function (e) {
            e.preventDefault();
            var $dot2 = $(this);

            $carousel2.trigger('to.owl.carousel', $dot2.data('index'));
            $dots2.removeClass('active');
            $dot2.addClass('active');
        });

        $dots2.first().addClass('active');
        $carousel2.trigger('to.owl.carousel', $dots2.first().data('index'));
    });


    //Initializing carousel & ensuring tab behavior on mobile for Media
    $('#media-subnav').on('click', function () {
        $('#media-tab').css('display', 'block');
        $('.tabs-content .industry-tab:not(#media-tab)').css('display', 'none');

        var $carousel3 = $('#owl-carousel-vertical3'),
            $dots3 = $('#owl-controls-vertical3 a');

        $carousel3.owlCarousel({
            autoplay: false,
            items: 1,
            loop: true,
            animateOut: 'slideOutUp',
            animateIn: 'slideInUp',
            autoHeight: true,
            mouseDrag: false,
            touchDrag: false,
            pullDrag: false
        });

        $dots3.on('click', function (e) {
            e.preventDefault();
            var $dot3 = $(this);

            $carousel3.trigger('to.owl.carousel', $dot3.data('index'));
            $dots3.removeClass('active');
            $dot3.addClass('active');
        });

        $dots3.first().addClass('active');
        $carousel3.trigger('to.owl.carousel', $dots3.first().data('index'));
    });


    //Initializing carousel & ensuring tab behavior on mobile for Public Sector
    $('#public-sector-subnav').on('click', function () {
        $('#public-sector-tab').css('display', 'block');
        $('.tabs-content .industry-tab:not(#public-sector-tab)').css('display', 'none');

        var $carousel4 = $('#owl-carousel-vertical4'),
            $dots4 = $('#owl-controls-vertical4 a');

        $carousel4.owlCarousel({
            autoplay: false,
            items: 1,
            loop: true,
            animateOut: 'slideOutUp',
            animateIn: 'slideInUp',
            autoHeight: true,
            mouseDrag: false,
            touchDrag: false,
            pullDrag: false
        });

        $dots4.on('click', function (e) {
            e.preventDefault();
            var $dot4 = $(this);

            $carousel4.trigger('to.owl.carousel', $dot4.data('index'));
            $dots4.removeClass('active');
            $dot4.addClass('active');
        });

        $dots4.first().addClass('active');
        $carousel4.trigger('to.owl.carousel', $dots4.first().data('index'));
    });

})(jQuery, window);

