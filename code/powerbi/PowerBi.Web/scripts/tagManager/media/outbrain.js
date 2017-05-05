(function (onyx) {
    'use strict';

    var pageUrl = onyx.tagManager.tagConfig.pageUrl,
		scriptUrl = onyx.tagManager.tagConfig.tagUrl;

    onyx.tagManager.media.Outbrain = [{
        name: 'Signup form submitt',
        pattern: pageUrl.GLOBAL,
        script: scriptUrl.OUTBRAIN,
        pixels: [{
            tag: '32547',
            selector: 'form[action*=submit-signup] button',
            events: 'mousedown'
        }]
    },
    ];
})(onyx);