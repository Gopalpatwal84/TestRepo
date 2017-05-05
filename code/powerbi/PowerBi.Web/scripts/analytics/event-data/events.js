(function(global) {
    /**
     * event object structure
     * {
     *      @string, - describes the event: page-action-element: {
     *          page: @string, - describes which page in which the event occurs
     *          action: @string, - describes in past tense what event occurred (ex. 'clicked', 'visited', etc)
     *          element: @string or @null - describe the element. if null, whill pull in the text of the element that was actioned upon
                blacklist: @object - array of objects that, if set to true will stop an event from being fired through a specifc analytics engine (ex. { km: true })
                properties: @bool - set to true, will capture the text of the element that was clicked on (default is false)
            }
     * }
     **/
    var events = {
        'global-clicked-getstartedcta': {
            page: 'global',
            action: 'clicked',
            element: 'getstartedcta'
        },
        'global-clicked-getcontent': {
            page: 'global',
            action: 'clicked',
            element: 'getcontent',
            properties: true
        },
        'global-clicked-mainmenu': {
            page: 'global',
            action: 'clicked',
            element: 'mainmenu'
        },
        'global-selected-dropdown': {
            page: 'global',
            action: 'selected',
            element: 'dropdown'
        },
        'global-submitted-form': {
            page: 'global',
            action: 'submitted',
            element: 'form'
        },
        'global-empty-form': {
            page: 'global',
            action: 'empty',
            element: 'form'
        },
        'global-blocked-form': {
            page: 'global',
            action: 'blocked',
            element: 'form'
        },
        'global-invalid-form': {
            page: 'global',
            action: 'invalid',
            element: 'form'
        },
        'global-consumer-form': {
            page: 'global',
            action: 'consumer',
            element: 'form'
        },
        'home-visited-page': {
            page: 'home',
            action: 'visited',
            element: 'page'
        },
        'home-clicked-primarycta': {
            page: 'home',
            action: 'clicked',
            element: 'primarycta'
        },
        'community-visited-page': {
            page: 'community',
            action: 'visited',
            element: 'page'
        },
        'comparepowerbi-visited-page': {
            page: 'comparepowerbi',
            action: 'visited',
            element: 'page'
        },
        'comparepowerbi-played-video': {
            page: 'comparepowerbi',
            action: 'played',
            element: 'video'
        },
        'desktop-visited-page': {
            page: 'desktop',
            action: 'visited',
            element: 'page'
        },
        'desktop-filled-phoneinput': {
            page: 'desktop',
            action: 'filled',
            element: 'phoneinput'
        },
        'doc-visited-page': {
            page: 'doc',
            action: 'visited',
            element: 'page'
        },
        'doc-clicked-menu': {
            page: 'doc',
            action: 'clicked',
            element: 'menu'
        },
        'exceldashboardpublisher-clicked-download32bit': {
            page: 'exceldashboardpublisher',
            action: 'clicked',
            element: 'download32bit'
        },
        'exceldashboardpublisher-clicked-download64bit': {
            page: 'exceldashboardpublisher',
            action: 'clicked',
            element: 'download64bit'
        },
        'exceldashboardpublisher-visited-page': {
            page: 'exceldashboardpublisher',
            action: 'visited',
            element: 'page'
        },
        'exceldashboardpublisherpreinstall-visited-page':{
            page: 'exceldashboardpublisherpreinstall',
            action: 'visited',
            element: 'page'
        },
        'exceldashboardpublisherthankyou-visited-page':{
            page: 'exceldashboardpublisherthankyou',
            action: 'visited',
            element: 'page'
        },
        'exceldashboardpublisherinterrupted-visited-page':{
            page: 'exceldashboardpublisherinterrupted',
            action: 'visited',
            element: 'page'
        },
        'features-visited-page': {
            page: 'features',
            action: 'visited',
            element: 'page'
        },
        'features-clicked-primarycta': {
            page: 'features',
            action: 'clicked',
            element: 'primarycta'
        },
        'gateway-visited-page': {
            page: 'gateway',
            action: 'visited',
            element: 'page'
        },
        'getstarted-clicked-getebookcta':{
            page: 'getstarted',
            action: 'clicked',
            element: 'getebookcta'
        },
        'getstarted-mouseover-exitintentarea':{
            page: 'getstarted',
            action: 'mouseover',
            element: 'exitintentarea'
        },
        'getstarted-visited-page':{
            page: 'getstarted',
            action: 'visited',
            element: 'page'
        },
        'guidedlearningdetails-visited-page': {
            page: 'guidedlearningdetails',
            action: 'visited',
            element: 'page'
        },
        'guidedlearningdetails-notify-firsttopiccompleted': {
            page: 'guidedlearningdetails',
            action: 'notify',
            element: 'firsttopiccompleted'
        },
        'guidedlearningdetails-notify-buildingblockscompleted': {
            page: 'guidedlearningdetails',
            action: 'notify',
            element: 'buildingblockscompleted'
        },
        'integrations-visited-page': {
            page: 'integrations',
            action: 'visited',
            element: 'page'
        },
        'integrations-played-video': {
            page: 'integrations',
            action: 'played',
            element: 'video'
        },
        'mobile-visited-page': {
            page: 'mobile',
            action: 'visited',
            element: 'page'
        },
        'partnershowcase-visited-page': {
            page: 'partnershowcase',
            action: 'visited',
            element: 'page'
        },
        'partnershowcase-clicked-learnmorecta': {
            page: 'partnershowcase',
            action: 'clicked',
            element: 'learnmorecta',
            properties: true
        },
        'partnershowcasedetails-visited-page': {
            page: 'partnershowcasedetails',
            action: 'visited',
            element: 'page'
        },
        'partnershowcasedetails-clicked-contactpartnercta': {
            page: 'partnershowcasedetails',
            action: 'clicked',
            element: 'contactpartnercta',
            properties: true
        },
        'partnershowcasedetails-clicked-viewreportcta': {
            page: 'partnershowcasedetails',
            action: 'clicked',
            element: 'viewreportcta',
            properties: true
        },
        'partnershowcasedetails-clicked-watchvideocta': {
            page: 'partnershowcasedetails',
            action: 'clicked',
            element: 'watchvideocta',
            properties: true
        },
        'partners-visited-page': {
            page: 'partners',
            action: 'visited',
            element: 'page'
        },
        'pricing-visited-page': {
            page: 'pricing',
            action: 'visited',
            element: 'page'
        },
        'pricing-clicked-optionscta': {
            page: 'pricing',
            action: 'clicked',
            element: 'optionscta'
        },
        'roles-visited-page': {
            page: 'roles',
            action: 'visited',
            element: 'page'
        },
        'solutions-visited-page': {
            page: 'solutions',
            action: 'visited',
            element: 'page'
        },
        'support-clicked-community':{
            page: 'support',
            action: 'clicked',
            element: 'community'
        },
        'support-clicked-reportanissue': {
            page: 'support',
            action: 'clicked',
            element: 'reportanissue'
        },
        'support-visited-page': {
            page: 'support',
            action: 'visited',
            element: 'page'
        },
        'support-clicked-createticket': {
            page: 'support',
            action: 'clicked',
            element: 'createticket'
        },
        'supportfree-visited-page': {
            page: 'support-free',
            action: 'visited',
            element: 'page'
        },
        'supportfree-clicked-purchase': {
            page: 'support-free',
            action: 'clicked',
            element: 'purchase'
        },
        'supportfree-clicked-contactus': {
            page: 'support-free',
            action: 'clicked',
            element: 'contactus'
        },
        'supportpro-visited-page': {
            page: 'support-pro',
            action: 'visited',
            element: 'page'
        },
        'supportpro-searched-searchbox': {
            page: 'support-pro',
            action: 'clicked',
            element: 'search'
        },
        'supportpro-clicked-adminportal': {
            page: 'support-pro',
            action: 'clicked',
            element: 'adminportal'
        },
        'supportpro-clicked-createticket': {
            page: 'support-pro',
            action: 'clicked',
            element: 'createticket'
        },
        'supportproticket-visited-page': {
            page: 'support-pro-ticket',
            action: 'visited',
            element: 'page'
        },
        'supportproticket-clicked-submitticket': {
            page: 'support-pro-ticket',
            action: 'clicked',
            element: 'submitticket'
        },
        'tour-visited-page': {
            page: 'tour',
            action: 'visited',
            element: 'page'
        },
        'tour-played-video': {
            page: 'tour',
            action: 'played',
            element: 'video'
        },
        'whatispowerbi-visited-page': {
            page: 'whatispowerbi',
            action: 'visited',
            element: 'page'
        },
        'whatispowerbi-clicked-video': {
            page: 'whatispowerbi',
            action: 'clicked',
            element: 'video'
        },
        'whatispowerbi-played-video': {
            page: 'whatispowerbi',
            action: 'played',
            element: 'video'
        },
        'getstarted-clicked-desktopdownload': {
            page: 'getstarted',
            action: 'clicked',
            element: 'desktopdownload'
        },
        'getstarted-clicked-servicesignup': {
            page: 'getstarted',
            action: 'clicked',
            element: 'servicesignup'
        },
        'articles-clicked-category': {
            page: 'articles',
            action: 'clicked',
            element: 'category'
        },
        'articles-clicked-link': {
            page: 'articles',
            action: 'clicked',
            element: 'link'
        }
    };

    global.onyx = global.onyx || {};
    global.onyx.analytics = global.onyx.analytics || {};

    global.onyx.analytics.events = events;
})(window)