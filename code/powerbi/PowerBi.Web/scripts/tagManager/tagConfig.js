(function ($, global) {
    'use strict';

    global.onyx.tagManager.tagConfig = {
        page: global.location.pathname,

        // TAG SCRIPT URLS
        tagUrl: {
            AMNET: '//pixel.mathtag.com/event/img?mt_id=',
            ATLAS: '//ad.atdmt.com/m/img;m=',
            BIDTELLECT: '//bttrack.com/Pixel/Conversion/',
            GOOGLEADS: '//googleads.g.doubleclick.net/pagead/viewthroughconversion/',
            OUTBRAIN: '//traffic.outbrain.com/network/trackpxl?action=view&advid=',
            TWITTER: '//analytics.twitter.com/i/adsct?txn_id=',
            UNIFIEDSOCIAL: '//t.co/i/adsct?txn_id=',
        },

        // PAGE URLS
        pageUrl: {
            GLOBAL: '/[a-z]{2}-[a-z]{2}/',
            HOME: '/[a-z]{2}-[a-z]{2}/$',
            SOLUTIONS: '/[a-z]{2}-[a-z]{2}/solutions/$',
            MOBILE: '/[a-z]{2}-[a-z]{2}/mobile/$',
            PRICING: '/[a-z]{2}-[a-z]{2}/pricing/$',
            DOWNLOADS: '/[a-z]{2}-[a-z]{2}/downloads/$',

            // Specific Urls
            // -- en-us
            ENUS_INTEGRATIONS_SALESFORCE: '/en-us/integrations/salesforce/$',
            ENUS_INTEGRATIONS_ADOBE_ANALYTICS: '/en-us/integrations/adobe-analytics/$',
            ENUS_INTEGRATIONS_GOOGLE_ANALYTICS: '/en-us/integrations/google-analytics/$',
            ENUS_COMPARE_POWERBI_TABLEAU_QLIK: '/en-us/compare-power-bi-tableau-qlik/$',
            ENUS_HOME: '/en-us/$',
            ENUS_GET_STARTED: '/en-us/get-started/$',

            //Market Bundles - en-us, de-de, ja-jp, fr-fr
            BUNDLE_INTEGRATIONS_ADOBE_ANALYTICS: '/(en-us|de-de|ja-jp|fr-fr)/integrations/adobe-analytics/$',
            BUNDLE_INTEGRATIONS_ACCESS: '/(en-us|de-de|ja-jp|fr-fr)/integrations/access/$',
            BUNDLE_EXCEL_DASHBOARD: '/(en-us|de-de|ja-jp|fr-fr)/excel-dashboard-publisher/$',
            BUNDLE_COMPARE_PBI_TABLEAU: '/(en-us|de-de|ja-jp|fr-fr)/compare-power-bi-tableau-qlik/$',
            BUNDLE_DESKTOP: '/(en-us|de-de|ja-jp|fr-fr)/desktop/$',
            BUNDLE_GET_STARTED: '/(en-us|de-de|ja-jp|fr-fr)/get-started/$',
            BUNDLE_HOMEPAGE: '/(en-us|de-de|ja-jp|fr-fr)/$',
            BUNDLE_ENUS_PAGES: '/en-us/(integrations/access|integrations/active-directory|integrations/adobe-analytics|integrations/comscore|integrations/exchange|integrations/github|integrations/google-analytics|integrations/hdinsight|integrations/mailchimp|integrations/mandrill|integrations/marketo|integrations/mysql|integrations/odata|integrations/oracle|integrations/postgresql|integrations/quickbooks-online|integrations/salesforce|integrations/sendgrid|integrations/stripe|integrations/twilio|integrations/visual-studio-online|integrations/webtrends|integrations/zendesk)/$',
        }
    };

    global.onyx = global.onyx || {};
    global.onyx.tagManager = global.onyx.tagManager || {};
    global.onyx.tagManager.media = global.onyx.tagManager.media || {};
})(jQuery, window);
