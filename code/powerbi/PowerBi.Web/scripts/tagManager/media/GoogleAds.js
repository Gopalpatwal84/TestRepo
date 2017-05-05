(function (onyx) {
    'use strict';

    var pageUrl = onyx.tagManager.tagConfig.pageUrl,
		scriptUrl = onyx.tagManager.tagConfig.tagUrl;

    onyx.tagManager.media.GoogleAds = [{
        name: 'September 7 2016 Request #945',
        pattern: pageUrl.ENUS_HOME,
        script: scriptUrl.GOOGLEADS,
        pixels: [{
            tag: '980969305/?value=0&amp;guid=ON&amp;script=0'
        }]
    },  {
        name: 'September 7 2016 Request #945',
        pattern: pageUrl.BUNDLE_ENUS_PAGES,
        script: scriptUrl.GOOGLEADS,
        pixels: [{
            tag: '980969305/?value=0&amp;guid=ON&amp;script=0'
        }]
    }];
})(onyx);
