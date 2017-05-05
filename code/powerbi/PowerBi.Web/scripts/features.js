(function ($, global) {

    var startingOffset = 1350,
        spaceBetweenAnims = 400,
        animDuration = 150,
        setTourSectionAutoCenter;

    function scrollToSection(multiplier) {
        var top = startingOffset + (spaceBetweenAnims * multiplier);

        $("html, body").animate({ scrollTop: top }, 500);
    }

    function scrollToTourSection() {
        $("html, body").animate({ scrollTop: $("#featuresTopSection").outerHeight() }, 1000);
    }

    $('body').on('click','.features-content h3', function () {
        scrollToSection($(this).data("index"))
    })

    $('#scrollTour').on('click', function (e) {
        e.preventDefault();
        scrollToTourSection();
    })


global.featuresFunction = function ()
{
    
        // height of all elements combined above features section
    var headerHeight = $("#featuresTopSection").outerHeight(),
        // gets height of features section, even with hidden elements
        tourSectionHeight = $("#featuresSectionFixed").get(0).scrollHeight,

        // sets where features section begins
        tourSectionStartingTop = headerHeight + 164,
        // height of titles plus some margin
        textHeadlineHeight = $("#textTitle1").height() + 12,

        // gets title heights, respectively
        title_Heights = [
            $("#textTitle1").get(0).scrollHeight,
            $("#textTitle2").get(0).scrollHeight,
            $("#textTitle3").get(0).scrollHeight,
            $("#textTitle4").get(0).scrollHeight,
            $("#textTitle5").get(0).scrollHeight,
            $("#textTitle6").get(0).scrollHeight,
            $("#textTitle7").get(0).scrollHeight,
            $("#textTitle8").get(0).scrollHeight,
            $("#textTitle9").get(0).scrollHeight
        ],

        // gets description heights, respectively
        textDesc1_Height = $("#textDesc1").get(0).scrollHeight,
        textDesc2_Height = $("#textDesc2").get(0).scrollHeight,
        textDesc3_Height = $("#textDesc3").get(0).scrollHeight,
        textDesc4_Height = $("#textDesc4").get(0).scrollHeight,
        textDesc5_Height = $("#textDesc5").get(0).scrollHeight,
        textDesc6_Height = $("#textDesc6").get(0).scrollHeight,
        textDesc7_Height = $("#textDesc7").get(0).scrollHeight,
        textDesc8_Height = $("#textDesc8").get(0).scrollHeight,
        textDesc9_Height = $("#textDesc9").get(0).scrollHeight,

        numSections = 9,
        //total height including hidden elements
        scrollingHeight = ((numSections - 1) * (spaceBetweenAnims + animDuration) - 100) + headerHeight + 300,
        //not totally sure why this is set like this
        maxTop = scrollingHeight - tourSectionHeight - 300;

    //set initial properties
    $("#textDesc1").height(textDesc1_Height);
    //sets height for full section if nothing were hidden
    $("#featuresScrollingSection").height(scrollingHeight);

    $("#featuresImage1").attr("style", "opacity: 1;");
    $("#textTitle1").attr("style", "color: #F2C812;");

    function setContentTop() {
        if ($(global).width() >= 992) {
            if ($(global).scrollTop() + ($(global).height() / 2) >= tourSectionStartingTop + (tourSectionHeight / 2)) {
                var top = $(global).scrollTop() + ($(global).height() / 2) - (tourSectionHeight / 2);

                if (top < maxTop && !setTourSectionAutoCenter) {
                    // sets fixed height for header
                    var fixedTop = ($(global).height() / 2) - (tourSectionHeight / 2);
                    $("#featuresTopSection").attr("style", "position: fixed; top: " + (-1 * (headerHeight + 64 - fixedTop)) + "px; width: 100%;");//header
                    $("#featuresSectionFixed").attr("style", "position: fixed; top: " + fixedTop + "px; bottom: 0px; margin: auto"); //content
                    $("#featuresTopSection ~ footer").attr("style", "position: fixed; top: " + (fixedTop + tourSectionHeight + 100) + "px; width: 100%;"); //footer
                    // console.log('tour section height',tourSectionHeight)

                    setTourSectionAutoCenter = true;
                }
                else if (top >= maxTop) {
                    setTourSectionAutoCenter = false;

                    $("#featuresTopSection").attr("style", "position: absolute; top: " + (-1 * (headerHeight + 64 - maxTop)) + "px; width:100%;");//header
                    $("#featuresSectionFixed").attr("style", "position: absolute; top: " + maxTop + "px; bottom: auto; margin: 0");//content
                    $("#featuresTopSection ~ footer").attr("style", "position: absolute; top: " + (maxTop + tourSectionHeight + 100) + "px; width: 100%;"); //footer

                    // console.log('got here');
                }
            }
            else {
                setTourSectionAutoCenter = false;
                $("#featuresTopSection").attr("style", "position: static; top: auto; width:auto;");//header
                $("#featuresSectionFixed").attr("style", "position: absolute; top: " + tourSectionStartingTop + "px; bottom: auto; margin: 0");//content
                $("#featuresTopSection ~ footer").attr("style", "position: absolute; top: " + (tourSectionStartingTop + tourSectionHeight + 100) + "px; width: 100%;"); //footer
            }
        }
        else {
            $("#featuresTopSection").attr("style", "position: static; top: auto; width:auto;");
            $("#featuresTopSection ~ footer").attr("style", "position: static; top: auto; width:auto;");
        }
    }

    function calculateTrackSliderTopForPos(titleAboveNum) {

        var top = 0, i;

        for (i = 1; i <= titleAboveNum; i++) {
            top += title_Heights[i - 1];
            //top += ((title_Heights[i-1] - 36) / 20 * 2) + 2;

            top += 4;
        }

        //top += 8;

        // console.log("TOP: " + top);

        return top + "px";
    }

    //make content section sticky
    $(global).on("scroll", setContentTop);
    setContentTop();// set on page load also


    //-- Scroll Magic: --//
    //Init Controller
    var controller = new ScrollMagic.Controller();


    /*** 1. SHIFT FROM FIRST TEXT TO SECOND ****************************/

    var textShiftScene1_timeline = new TimelineMax();

    textShiftScene1_timeline.add([
        TweenMax.to('#textDesc1', 6, { height: "0px" }),
        TweenMax.to('#textDesc2', 6, { height: textDesc2_Height + "px" }),
        TweenMax.to('#trackSlider', 6, { top: calculateTrackSliderTopForPos(1) }),
        TweenMax.to('#featuresImage1', 6, { opacity: "0" }),
        TweenMax.to('#featuresImage2', 6, { opacity: "1" }),
        TweenMax.to('#textTitle1', 6, { color: "#000000" }),
        TweenMax.to('#textTitle2', 6, { color: "#F2C812" })
    ]);

    var textShiftScene1 = new ScrollMagic.Scene({
        duration: animDuration,
        offset: startingOffset
    })
    .setTween(textShiftScene1_timeline)
    .addTo(controller);
    //.addIndicators();

    /***************************************************************/

    /*** 2. SHIFT FROM SECOND TEXT TO THIRD ****************************/

    var textShiftScene2_timeline = new TimelineMax();

    textShiftScene2_timeline.add([
        TweenMax.to('#textDesc2', 6, { height: "0px" }),
        TweenMax.to('#textDesc3', 6, { height: textDesc3_Height + "px" }),
        TweenMax.to('#trackSlider', 6, { top: calculateTrackSliderTopForPos(2) }),
        TweenMax.to('#featuresImage2', 6, { opacity: "0" }),
        TweenMax.to('#featuresImage3', 6, { opacity: "1" }),
        TweenMax.to('#textTitle2', 6, { color: "#000000" }),
        TweenMax.to('#textTitle3', 6, { color: "#F2C812" })
    ]);

    var textShiftScene2 = new ScrollMagic.Scene({
        duration: animDuration,
        offset: startingOffset + spaceBetweenAnims
    })
    .setTween(textShiftScene2_timeline)
    .addTo(controller);
    //.addIndicators();

    /***************************************************************/

    /*** 3. SHIFT FROM THIRD TEXT TO FOURTH ****************************/

    var questionToAsk = "What were last year's sales by product?",
        animationReset = true;

    var AskQuestions_timeline = new TimelineMax();

    AskQuestions_timeline.add([
        TweenMax.to('#askQuestionsInputSection', 6, { top: "25px", height: "413px" }),
    ]);
    AskQuestions_timeline.duration(1.4);
    AskQuestions_timeline.pause();

    function resetAskQuestionsAnimation() {
        $("#askQuestionsInputBox").text("");
        $("#askQuestionsInputSection").height("44px;");
        AskQuestions_timeline.restart();
        AskQuestions_timeline.pause();

        animationReset = true;
    }

    function startAskQuestionsAnimation() {

        animationReset = false;

        var currentText = $("#askQuestionsInputBox").text();

        if (currentText.length < questionToAsk.length) {
            $("#askQuestionsInputBox").text(currentText + questionToAsk[currentText.length]);

            setTimeout(startAskQuestionsAnimation, 30);
        }
        else {
            AskQuestions_timeline.play();
        }
    };


    var textShiftScene3_timeline = new TimelineMax();

    textShiftScene3_timeline.add([
        TweenMax.to('#textDesc3', 6, { height: "0px" }),
        TweenMax.to('#textDesc4', 6, { height: textDesc4_Height + "px" }),
        TweenMax.to('#trackSlider', 6, { top: calculateTrackSliderTopForPos(3) }),
        TweenMax.to('#featuresImage3', 6, { opacity: "0" }),
        TweenMax.to('#featuresImage4', 6, { opacity: "1" }),
        TweenMax.to('#textTitle3', 6, { color: "#000000" }),
        TweenMax.to('#textTitle4', 6, { color: "#F2C812" })
    ]);

    var textShiftScene3 = new ScrollMagic.Scene({
        duration: animDuration,
        offset: startingOffset + (spaceBetweenAnims * 2)
    })
    .setTween(textShiftScene3_timeline)
    .addTo(controller);
    //.addIndicators();

    textShiftScene3.on('start', function (event) {
        resetAskQuestionsAnimation();
    });


    textShiftScene3.on('end', function (event) {
        if (true) {
            resetAskQuestionsAnimation();
            startAskQuestionsAnimation();
        }
    });


    /***************************************************************/

    /*** 4. SHIFT FROM FOURTH TEXT TO FIFTH ****************************/

    var textShiftScene4_timeline = new TimelineMax();

    textShiftScene4_timeline.add([
        TweenMax.to('#textDesc4', 6, { height: "0px" }),
        TweenMax.to('#textDesc5', 6, { height: textDesc5_Height + "px" }),
        TweenMax.to('#trackSlider', 6, { top: calculateTrackSliderTopForPos(4) }),
        TweenMax.to('#featuresImage4', 6, { opacity: "0" }),
        TweenMax.to('#featuresImage5', 6, { opacity: "1" }),
        TweenMax.to('#textTitle4', 6, { color: "#000000" }),
        TweenMax.to('#textTitle5', 6, { color: "#F2C812" })
    ]);

    var textShiftScene4 = new ScrollMagic.Scene({
        duration: animDuration,
        offset: startingOffset + (spaceBetweenAnims * 3)
    })
    .setTween(textShiftScene4_timeline)
    .addTo(controller);
    //.addIndicators();

    textShiftScene4.on('end', function (event) {
        resetAskQuestionsAnimation();
    });

    /***************************************************************/

    /*** 5. SHIFT FROM FIFTH TEXT TO SIXTH ****************************/

    var textShiftScene5_timeline = new TimelineMax();

    textShiftScene5_timeline.add([
        TweenMax.to('#textDesc5', 6, { height: "0px" }),
        TweenMax.to('#textDesc6', 6, { height: textDesc6_Height + "px" }),
        TweenMax.to('#trackSlider', 6, { top: calculateTrackSliderTopForPos(5) }),
        TweenMax.to('#featuresImage5', 6, { opacity: "0" }),
        TweenMax.to('#featuresImage6', 6, { opacity: "1" }),
        TweenMax.to('#textTitle5', 6, { color: "#000000" }),
        TweenMax.to('#textTitle6', 6, { color: "#F2C812" })
    ]);

    var textShiftScene5 = new ScrollMagic.Scene({
        duration: animDuration,
        offset: startingOffset + (spaceBetweenAnims * 4)
    })
    .setTween(textShiftScene5_timeline)
    .addTo(controller);
    //.addIndicators();

    /***************************************************************/

    /*** 6. SHIFT FROM SIXTH TEXT TO SEVENTH ****************************/

    var textShiftScene6_timeline = new TimelineMax();

    textShiftScene6_timeline.add([
        TweenMax.to('#textDesc6', 6, { height: "0px" }),
        TweenMax.to('#textDesc7', 6, { height: textDesc7_Height + "px" }),
        TweenMax.to('#trackSlider', 6, { top: calculateTrackSliderTopForPos(6) }),
        TweenMax.to('#featuresImage6', 6, { opacity: "0" }),
        TweenMax.to('#featuresImage7', 6, { opacity: "1" }),
        TweenMax.to('#textTitle6', 6, { color: "#000000" }),
        TweenMax.to('#textTitle7', 6, { color: "#F2C812" })
    ]);

    var textShiftScene6 = new ScrollMagic.Scene({
        duration: animDuration,
        offset: startingOffset + (spaceBetweenAnims * 5)
    })
    .setTween(textShiftScene6_timeline)
    .addTo(controller);
    //.addIndicators();

    /***************************************************************/

    /*** 7. SHIFT FROM SEVENTH TEXT TO EIGHTH ****************************/

    var textShiftScene7_timeline = new TimelineMax();

    textShiftScene7_timeline.add([
        TweenMax.to('#textDesc7', 6, { height: "0px" }),
        TweenMax.to('#textDesc8', 6, { height: textDesc8_Height + "px" }),
        TweenMax.to('#trackSlider', 6, { top: calculateTrackSliderTopForPos(7) }),
        TweenMax.to('#featuresImage7', 6, { opacity: "0" }),
        TweenMax.to('#featuresImage8', 6, { opacity: "1" }),
        TweenMax.to('#textTitle7', 6, { color: "#000000" }),
        TweenMax.to('#textTitle8', 6, { color: "#F2C812" })
    ]);

    var textShiftScene7 = new ScrollMagic.Scene({
        duration: animDuration,
        offset: startingOffset + (spaceBetweenAnims * 6)
    })
    .setTween(textShiftScene7_timeline)
    .addTo(controller);
    //.addIndicators();

    /***************************************************************/

    /*** 8. SHIFT FROM EIGHTH TEXT TO NINTH ****************************/

    var textShiftScene8_timeline = new TimelineMax();
    textShiftScene8_timeline.add([
        TweenMax.to('#textDesc8', 6, { height: "0px" }),
        TweenMax.to('#textDesc9', 6, { height: textDesc9_Height + "px" }),
        TweenMax.to('#trackSlider', 6, { top: calculateTrackSliderTopForPos(8) }),
        TweenMax.to('#featuresImage8', 6, { opacity: "0" }),
        TweenMax.to('#featuresImage9', 6, { opacity: "1" }),
        TweenMax.to('#textTitle8', 6, { color: "#000000" }),
        TweenMax.to('#textTitle9', 6, { color: "#F2C812" })
    ]);

    var textShiftScene8 = new ScrollMagic.Scene({
        duration: animDuration,
        offset: startingOffset + (spaceBetweenAnims * 7)
    })
    .setTween(textShiftScene8_timeline)
    .addTo(controller);
    //.addIndicators();

    /***************************************************************/
};

})(jQuery, window);

