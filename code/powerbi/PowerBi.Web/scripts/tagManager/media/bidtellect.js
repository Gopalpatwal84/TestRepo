(function (onyx) {
    'use strict';

    var pageUrl = onyx.tagManager.tagConfig.pageUrl,
		scriptUrl = onyx.tagManager.tagConfig.tagUrl;

    onyx.tagManager.media.Bidtellect = [{
        name: 'Signup form submitt',
        pattern: pageUrl.GLOBAL,
        script: scriptUrl.BIDTELLECT,
        pixels: [{
            tag: '13281',
            selector: 'form[action*=submit-signup] button',
            events: 'mousedown'
        }]
    },
    ];
})(onyx);