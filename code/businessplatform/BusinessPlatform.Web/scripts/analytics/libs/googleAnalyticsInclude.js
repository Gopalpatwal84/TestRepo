$(function () {
    (function (n, t, i, r, u, f, e) {
        n.GoogleAnalyticsObject = u, n[u] = n[u] || function () { (n[u].q = n[u].q || []).push(arguments) }, n[u].l = 1 * new Date, f = t.createElement(i), e = t.getElementsByTagName(i)[0], f.async = 1, f.src = r, e.parentNode.insertBefore(f, e)
    })(window, document, "script", "//www.google-analytics.com/analytics.js", "ga"),
    ga("create", window.onyx.analytics.config.ga.key, window.onyx.analytics.config.ga.domain),
    ga("require", "linkid", "linkid.js");
    ga('send', 'pageview');
    ga('send', 'event', 'Region', 'visited', window.requestRegion);
})