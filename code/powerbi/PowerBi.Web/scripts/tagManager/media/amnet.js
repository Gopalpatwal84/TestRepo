(function (onyx) {
    'use strict';

    var pageUrl = onyx.tagManager.tagConfig.pageUrl,
		scriptUrl = onyx.tagManager.tagConfig.tagUrl;

    onyx.tagManager.media.Amnet = [{
        name: 'Home page load',
        pattern: pageUrl.HOME,
        script: scriptUrl.AMNET,
        pixels: [{
            tag: '796385&mt_adid=145196&v1=&v2=&v3=&s1=&s2=&s3='
        }]
    },  {
        name: 'Solutions page load',
        pattern: pageUrl.SOLUTIONS,
        script: scriptUrl.AMNET,
        pixels: [{
            tag: '796292&mt_adid=145196&v1=&v2=&v3=&s1=&s2=&s3='
        }]
    }, {
        name: 'pricing page load',
        pattern: pageUrl.PRICING,
        script: scriptUrl.AMNET,
        pixels: [{
            tag: '796292&mt_adid=145196&v1=&v2=&v3=&s1=&s2=&s3='
        }]
    }, {
        name: 'Downloads page load',
        pattern: pageUrl.DOWNLOADS,
        script: scriptUrl.AMNET,
        pixels: [{
            tag: '796292&mt_adid=145196&v1=&v2=&v3=&s1=&s2=&s3='
        }]
    }, {
        name: 'Signup form submitt',
        pattern: pageUrl.GLOBAL,
        script: scriptUrl.AMNET,
        pixels: [{
            tag: '796293&mt_adid=145196&v1=&v2=&v3=&s1=&s2=&s3=',
            selector: 'form[action*=submit-signup] button',
            events: 'mousedown'
        }]
    },
    ];
})(onyx);