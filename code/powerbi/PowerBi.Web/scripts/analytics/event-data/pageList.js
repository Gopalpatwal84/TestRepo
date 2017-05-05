(function(global) {
    /**
     * Key should be page name from events.js (ex. 'home-visited-page', 'home' is the key for the value/regex)
     * Value should be regex, or an array of regex to match multiple urls to single event in events.js
     * Please be sure to keep this list ALPHABETIZED (ascending)
     **/
    var pageList = {
        home: '^/[a-z]{2}-[a-z]{2}/$',
        community: '^/[a-z]{2}-[a-z]{2}/community/$',
        comparepowerbi: '^/[a-z]{2}-[a-z]{2}/compare-power-bi-tableau-qlik/$',
        desktop: '^/[a-z]{2}-[a-z]{2}/desktop/$',
        doc: '^/[a-z]{2}-[a-z]{2}/documentation/[a-z0-9\-]+/$',
        exceldashboardpublisher: '^/[a-z]{2}-[a-z]{2}/excel-dashboard-publisher/$',
        exceldashboardpublisherinterrupted: '^/[a-z]{2}-[a-z]{2}/excel-dashboard-publisher-interrupted/$',
        exceldashboardpublisherpreinstall: '^/[a-z]{2}-[a-z]{2}/excel-dashboard-publisher-pre-install/$',
        exceldashboardpublisherthankyou: '^/[a-z]{2}-[a-z]{2}/excel-dashboard-publisher-thank-you/$',
        features: '^/[a-z]{2}-[a-z]{2}/features/$',
        gateway: '^/[a-z]{2}-[a-z]{2}/gateway/$',
        getstarted: '^/[a-z]{2}-[a-z]{2}/get-started/$',
        guidedlearningdetails: '^/[a-z]{2}-[a-z]{2}/guided-learning/[a-z0-9\-]+/$',
        integrations: '^/[a-z]{2}-[a-z]{2}/integrations/[a-z0-9\-]+/$',
        mobile: '^/[a-z]{2}-[a-z]{2}/mobile/$',
        partners: '^/[a-z]{2}-[a-z]{2}/partners/$',
        partnershowcase: '^/[a-z]{2}-[a-z]{2}/partner-showcase/$',
        partnershowcasedetails: '^/[a-z]{2}-[a-z]{2}/partner-showcase/[a-z0-9\-]+/$',
        pricing: '^/[a-z]{2}-[a-z]{2}/pricing/$',
        roles: '^/[a-z]{2}-[a-z]{2}/roles/$',
        solutions: '^/[a-z]{2}-[a-z]{2}/solutions/$',
        support: '^/[a-z]{2}-[a-z]{2}/support/$',
        tour: '^/[a-z]{2}-[a-z]{2}/tour/$',
        whatispowerbi: '^/[a-z]{2}-[a-z]{2}/what-is-power-bi/$',
        supportpro: '^/[a-z]{2}-[a-z]{2}/support/pro/$',
        supportfree: '^/[a-z]{2}-[a-z]{2}/support/free/$',
        supportproticket: '^/[a-z]{2}-[a-z]{2}/support/pro/ticket/$'
    };

    global.onyx = global.onyx || {};
    global.onyx.analytics = global.onyx.analytics || {};

    global.onyx.analytics.pageList = pageList;
})(window)