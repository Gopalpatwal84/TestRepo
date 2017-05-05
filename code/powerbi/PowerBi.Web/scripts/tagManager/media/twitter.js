(function (onyx) {
    'use strict';

    var pageUrl = onyx.tagManager.tagConfig.pageUrl,
		scriptUrl = onyx.tagManager.tagConfig.tagUrl;

    onyx.tagManager.media.Twitter = [{
        name: 'Signup form submitt',
        pattern: pageUrl.GLOBAL,
        script: scriptUrl.TWITTER,
        pixels: [{
            tag: 'l6gmb&p_id=Twitter&tw_sale_amount=0&tw_order_quantity=0',
            selector: 'form[action*=submit-signup] button',
            events: 'mousedown'
        }]
    },
    ];
})(onyx);