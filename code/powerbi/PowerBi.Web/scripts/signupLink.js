(function () {
    var $links = $('.signup-link'),
        $signupRedirectIntegrations = $('.signup-integration #FormOptions_SubmitRedirectUrl'),
        integrationName = '';

    // check for integrations page, set integration
    if ($('.container').hasClass('container-integrations')) {
        integrationName = $('.container').attr('class').split(' ').pop();
    }

    var ru = 'https://app.powerbi.com/groups/me/getdata/services/' + integrationName;

    // signup form integration handler
    $signupRedirectIntegrations.each(function (index, element) {
        var url = element.value + '?ru=' + encodeURIComponent(ru);
        element.value = url;
    });

    //signup button handler
    $links.each(function (index, element) {
        var url = element.href + '?pbi_source=web';
        element.href = url;
    });

})();