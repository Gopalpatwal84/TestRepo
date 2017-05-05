(function (onyx) {
    'use strict';

    var pageUrl = onyx.tagManager.tagConfig.pageUrl,
		scriptUrl = onyx.tagManager.tagConfig.tagUrl;

    onyx.tagManager.media.atlas = [{
        name: 'June 13 2016 Request #803 Integrations SalesForce',
        pattern: pageUrl.ENUS_INTEGRATIONS_SALESFORCE,
        script: scriptUrl.ATLAS,
        pixels: [{
            tag: '11087204082189?event=CLD_PowerBi_GBL_Sub_Eml_UseItFree_CON',
            selector: '#power-bi-signup-click',
            events: 'mousedown'
        }, {
            tag: '11087204082189?event=CLD_PowerBI_GBL_Ini_Vid_VideoPlay_CON',
            selector: '.video.inline-video-wrapper',
            events: 'mousedown'
        }]
    }, {
        name: 'June 13 2016 Request #803 Integrations Adobe Analytics',
        pattern: pageUrl.ENUS_INTEGRATIONS_ADOBE_ANALYTICS,
        script: scriptUrl.ATLAS,
        pixels: [{
            tag: '11087204082189?event=CLD_PowerBi_GBL_Sub_Eml_UseItFree_CON',
            selector: '#power-bi-signup-click',
            events: 'mousedown'
        }, {
            tag: '11087204082189?event=CLD_PowerBI_GBL_Ini_Vid_VideoPlay_CON',
            selector: '.video.inline-video-wrapper',
            events: 'mousedown'
        }]
    }, {
        name: 'June 13 2016 Request #803 Integrations Google Analytics',
        pattern: pageUrl.ENUS_INTEGRATIONS_GOOGLE_ANALYTICS,
        script: scriptUrl.ATLAS,
        pixels: [{
            tag: '11087204082189?event=CLD_PowerBi_GBL_Sub_Eml_UseItFree_CON',
            selector: '#power-bi-signup-click',
            events: 'mousedown'
        }, {
            tag: '11087204082189?event=CLD_PowerBI_GBL_Ini_Vid_VideoPlay_CON',
            selector: '.video.inline-video-wrapper',
            events: 'mousedown'
        }]
    }, {
        name: 'June 28 2016 Request #870 Compare PowerBI Tableau Qlik',
        pattern: pageUrl.ENUS_COMPARE_POWERBI_TABLEAU_QLIK,
        script: scriptUrl.ATLAS,
        pixels: [{
            tag: '11087203713375?event=CLD_PowerBI_GBL_Ini_Prd_GetStartedFree_CON',
            selector: '.button:eq(2)',
            events: 'mousedown'
        }]
    }, {
        name: 'August 2 2016 Request #915 PBI Home Page EN-US',
        pattern: pageUrl.ENUS_HOME,
        script: scriptUrl.ATLAS,
        pixels: [{
            tag: '11087203713375?event=CLD_PowerBi_USA_Sub_Eml_UseItFree_CON',
            selector: '.button:eq(1)',
            events: 'mousedown'
        }, {
            tag: '11087203713375?event=CLD_PowerBi_USA_Sub_Eml_UseItFree_CON',
            selector: '.button.button-featured.button-block:eq(0)',
            events: 'mousedown'
        }, {
            tag: '11087203713375?event=CLD_PowerBi_USA_Sub_Eml_UseItFree_CON',
            selector: '.button.button-featured.button-block:eq(1)',
            events: 'mousedown'
        }, {
            tag: '11087203713375?event=CLD_PowerBi_USA_Vue_Vid_inAction_CON',
            selector: '.button:eq(9)',
            events: 'mousedown'
        }, {
            tag: '11087203713375?event=CLD_PowerBi_USA_Vue_Prd_Prodinfo_CON',
            selector: '.button.button-featured:eq(0)',
            events: 'mousedown'
        }, {
            tag: '11087203713375?event=CLD_PowerBi_USA_Vue_Prd_Prodinfo_CON',
            selector: '.button:eq(4)',
            events: 'mousedown'
        }, {
            tag: '11087203713375?event=CLD_PowerBi_USA_Vue_Prd_Prodinfo_CON',
            selector: '.button:eq(5)',
            events: 'mousedown'
        }, {
            tag: '11087203713375?event=CLD_PowerBi_USA_Vue_Prd_Prodinfo_CON',
            selector: '.button:eq(6)',
            events: 'mousedown'
        }]
    }, {
        name: 'Sept 28 2016 Request #997 Home Page',
        pattern: pageUrl.BUNDLE_HOMEPAGE,
        script: scriptUrl.ATLAS,
        pixels: [{
            tag: '11087201299407?event=PMG_Cloud_GBL_Ini_Prd_SiteEngagement_CON',
            selector: '#power-bi-sign-up-free-mainmenu',
            events: 'mousedown'
        }, {
            tag: '11087201299407?event=PMG_Cloud_GBL_Ini_Prd_SiteEngagement_CON',
            selector: '#power-bi-get-started-free-home-header-button',
            events: 'mousedown'
        }, {
            tag: '11087201299407?event=PMG_Cloud_GBL_Ini_Prd_SiteEngagement_CON',
            selector: '.row.column.text-center a',
            events: 'mousedown'
        }, {
            tag: '11087201299407?event=PMG_Cloud_GBL_Ini_Prd_SiteEngagement_CON',
            selector: '.button.button-featured:eq(0)',
            events: 'mousedown'
        }]
    }, {
        name: 'Sept 28 2016 Request #997 Get Started',
        pattern: pageUrl.BUNDLE_GET_STARTED,
        script: scriptUrl.ATLAS,
        pixels: [{
            tag: '11087201299407?event=PMG_Cloud_GBL_Ini_Prd_SiteEngagement_CON',
            selector: '#power-bi-sign-up-free-mainmenu',
            events: 'mousedown'
        }, {
            tag: '11087201299407?event=FY14_Q2_Trial_Layer_Tak_STr_TP_SU_CON',
            selector: '#download-desktop',
            events: 'mousedown'
        }, {
            tag: '11087201299407?event=FY14_Q2_Trial_Layer_Tak_STr_TP_SU_CON',
            selector: '#free-sign-up',
            events: 'mousedown'
        }, {
            tag: '11087201299407?event=PMG_Cloud_GBL_Ini_Prd_SiteEngagement_CON',
            selector: '.custom-olark-container.demo-contact',
            events: 'mousedown'
        }]
    }, {
        name: 'Sept 28 2016 Request #997 Desktop',
        pattern: pageUrl.BUNDLE_DESKTOP,
        script: scriptUrl.ATLAS,
        pixels: [{
            tag: '11087201299407?event=PMG_Cloud_GBL_Ini_Prd_SiteEngagement_CON',
            selector: '#power-bi-sign-up-free-mainmenu',
            events: 'mousedown'
        }, {
            tag: '11087201299407?event=PMG_Cloud_GBL_Ini_Prd_PowerBiTrialLead_CON',
            selector: '#desktop-download',
            events: 'mousedown'
        }, {
            tag: '11087201299407?event=PMG_Cloud_GBL_Ini_Prd_SiteEngagement_CON',
            selector: '.cta-secondary',
            events: 'mousedown'
        }]
    }, {
        name: 'Sept 28 2016 Request #997 Compare PBI Tableau Qlik',
        pattern: pageUrl.BUNDLE_COMPARE_PBI_TABLEAU,
        script: scriptUrl.ATLAS,
        pixels: [{
            tag: '11087201299407?event=PMG_Cloud_GBL_Ini_Prd_SiteEngagement_CON',
            selector: '#power-bi-sign-up-free-mainmenu',
            events: 'mousedown'
        }, {
            tag: '11087201299407?event=PMG_Cloud_GBL_Ini_Prd_SiteEngagement_CON',
            selector: '.placeholderScreenshot',
            events: 'mousedown'
        }, {
            tag: '11087201299407?event=PMG_Cloud_GBL_Ini_Prd_SiteEngagement_CON',
            selector: '.button:eq(1)',
            events: 'mousedown'
        }]
    }, {
        name: 'Sept 28 2016 Request #997 Excel Dashboard Publisher',
        pattern: pageUrl.BUNDLE_EXCEL_DASHBOARD,
        script: scriptUrl.ATLAS,
        pixels: [{
            tag: '11087201299407?event=PMG_Cloud_GBL_Ini_Prd_SiteEngagement_CON',
            selector: '#power-bi-sign-up-free-mainmenu',
            events: 'mousedown'
        }, {
            tag: '11087201299407?event=PMG_Cloud_GBL_Ini_Prd_PowerBiTrialLead_CON',
            selector: '#excelPubDownload32Bit',
            events: 'mousedown'
        }, {
            tag: '11087201299407?event=PMG_Cloud_GBL_Ini_Prd_PowerBiTrialLead_CON',
            selector: '#excelPubDownload64Bit',
            events: 'mousedown'
        }]
    }, {
        name: 'Sept 28 2016 Request #997 Integrations Access',
        pattern: pageUrl.BUNDLE_INTEGRATIONS_ACCESS,
        script: scriptUrl.ATLAS,
        pixels: [{
            tag: '11087201299407?event=PMG_Cloud_GBL_Ini_Prd_SiteEngagement_CON',
            selector: '.button:eq(0)',
            events: 'mousedown'
        }, {
            tag: '11087201299407?event=PMG_Cloud_GBL_Ini_Prd_SiteEngagement_CON',
            selector: '.custom-olark-container.demo-contact',
            events: 'mousedown'
        }]
    }, {
        name: 'Sept 28 2016 Request #997 Integrations Adobe Analytics',
        pattern: pageUrl.BUNDLE_INTEGRATIONS_ADOBE_ANALYTICS,
        script: scriptUrl.ATLAS,
        pixels: [{
            tag: '11087201299407?event=PMG_Cloud_GBL_Ini_Prd_SiteEngagement_CON',
            selector: '#power-bi-signup-click',
            events: 'mousedown'
        }]
    }, {
         name: '11/23/16 Request GSG SO 405471',
         pattern: pageUrl.ENUS_HOME,
         script: scriptUrl.ATLAS,
         pixels: [{
             tag: '11087209098569;cache=?event=Click_on_Get_Started_Free',
             selector: 'a#power-bi-get-started-free-home-header-button.button:eq(1)',
             events: 'mousedown'
         }, {
             tag: '11087209098569;cache=?event=Powerbi_landingpage',
         }]
     }, {
         name: '11/23/16 Request GSG SO 405471',
         pattern: pageUrl.ENUS_GET_STARTED,
         script: scriptUrl.ATLAS,
         pixels: [{
             tag: '11087209098569;cache=?event=Click_on_Download_button',
             selector: 'a#download-desktop',
             events: 'mousedown'
         },{
             tag: '11087209098569;cache=?event=Click_on_Signup_Button',
             selector: 'a#free-sign-up',
             events: 'mousedown'
         }, {
             tag: '11087209098569;cache=?event=powerbi_getstarted_Landingpage',
         }]
     },{
        name: '12/6/2016 - Global Universal Tag - SP Request 1203',
        pattern: pageUrl.GLOBAL,
        script: scriptUrl.ATLAS_AD,
        pixels: [{
          tag: '11087210027353;cache=?msfpc=&amp;mc1=' + ($.cookie('MC1')||'').substring(5,37)
      }]
    }];
})(onyx);
