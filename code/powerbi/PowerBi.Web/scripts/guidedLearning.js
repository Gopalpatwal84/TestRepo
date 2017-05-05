(function ($, storage, global) {

    var slug = $('meta[name=guided-learning-slug]').attr('content'),
        progressStored = JSON.parse(storage.get('GuidedLearningProgress')) || { 'completed': {} };

    function checkCompleted() {
        $('[data-gl-lessons]').each(function () {
            var $this = $(this),
                lessons = $this.data('gl-lessons').split(' '),
                totalLessons = lessons.length,
                completedLessons = 0,
                progress = 0;

            $.each(lessons, function (idx, val) {
                if (progressStored.completed[val]) {
                    completedLessons++;
                }
            });

            progress = parseInt((completedLessons / totalLessons * 100), 10);
            $this.data('gl-progress', progress);

            if (progress === 100) {
                $this.addClass('completed');
            }

            setProgressBar();

        });
    }

    function setProgressBar() {
        var $progress = $('.progress-tracker');

        $progress.each(function () {
            var $this = $(this),
                percentComplete = $this.closest('[data-gl-lessons]').data('gl-progress');
            $this.find('.progress-bar').width(percentComplete + '%');
            $this.find('.percentage').text(percentComplete + '%');
            if (percentComplete === 100) {
                $this.find('.progress-bar').addClass('done');
            }
        });
    }

    function setUser(email) {
        progressStored.user = email;
        storageSync();
        checkCompleted();
    }

    function isFirstTopicComplete() {
        var objLength = 0;
        for (key in progressStored.completed) {
            objLength++;
        }
        return objLength === 1;
    }

    function storageSync() {
        storage.set('GuidedLearningProgress', JSON.stringify(progressStored));
    }

    function checkBuildingBlocks() {
        if (slug === 'powerbi-learning-0-0b-building-blocks-power-bi') {
            var nextUrl = $('.continue').attr('href');
            $('.continue').attr('href', nextUrl + '?building-blocks=1')
        } else if (global.location.search.indexOf('?building-blocks=1') !== -1) {
            $('#buildingBlocksComplete').removeClass('hide');
        }
    }

    function isSignedIn() {
        return !!progressStored.user;
    }

    function closeNotification() {
        $(this).closest('.card-notification').remove();
    }

    function sendAnalytics() {
        if (slug === 'powerbi-learning-0-0b-building-blocks-power-bi') {
            global.onyx.analytics.getData('guidedlearningdetails-notify-buildingblockscompleted');
        }
        if (isFirstTopicComplete()) {
            global.onyx.analytics.getData('guidedlearningdetails-notify-firsttopiccompleted');
        }
    }

    function initializeGLProgressTracking() {
        checkBuildingBlocks();

        $('.button.continue').on('click', function () {

            var lessonCategory = $('.active').data('gl-category');
            progressStored.completed[slug] = true;
            storageSync();
            sendAnalytics();

            // please un-comment out once munchkin is added back in to PBI
            //mktoMunchkinFunction('clickLink',
            //    { href: slug + '?finished=true' }
            //);
        });

        if (isSignedIn()) {
            checkCompleted();
            $('.track-progress').remove();
            $('.progress-tracker').removeClass('hide');
        } else {
            $('.track-progress').on('submit','form', function () {
                var email = $(this).find('#Email').val();
                setUser(email);
                $('.track-progress').addClass('hide');
                $('#signupSuccess').removeClass('hide');
            });
            if (isFirstTopicComplete()) {
                $('#firstTopicComplete').removeClass('hide');
            }
        }
    }

    $('.card-notification').on('click', '.close', closeNotification);
    global.guidedLearning = global.guidedLearning || {};
    global.guidedLearning.init = initializeGLProgressTracking;

})(jQuery, window.sd.storage, window);