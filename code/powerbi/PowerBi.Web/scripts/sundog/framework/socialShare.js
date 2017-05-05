(function ($) {
    'use strict';

    var FACEBOOK_WIDTH = 560,
        FACEBOOK_HEIGHT = 330,
        TWITTER_WIDTH = 560,
        TWITTER_HEIGHT = 260,
        LINKEDIN_WIDTH = 560,
        LINKEDIN_HEIGHT = 450;

    function openFacebookPopup() {
        return !window.open(this.href, 'Facebook', 'width=' + FACEBOOK_WIDTH + ',height=' + FACEBOOK_HEIGHT);
    }

    function openTwitterPopup() {
        return !window.open(this.href, 'Twitter', 'width=' + TWITTER_WIDTH + ',height=' + TWITTER_HEIGHT);
    }

    function openLinkedInPopup() {
        return !window.open(this.href, 'Linkedin', 'width=' + LINKEDIN_WIDTH + ',height=' + LINKEDIN_HEIGHT);
    }

    $('body').on('click', '.social-share.social-facebook', openFacebookPopup);
    $('body').on('click', '.social-share.social-twitter', openTwitterPopup);
    $('body').on('click', '.social-share.social-linkedin', openLinkedInPopup);

})(jQuery);