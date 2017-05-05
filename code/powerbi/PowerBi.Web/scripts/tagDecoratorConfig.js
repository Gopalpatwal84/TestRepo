(function ($, global) {
    "use strict";

    global.tagDecoratorConfig = [{
        control: "a",
        type: "link",
        interaction: 1
    },
    {
        control: ".arrowLink",
        type: "link",
        interaction: 1
    },
    {
        control: ".button",
        type: "button",
        interaction: 22
    },
    {
        control: ".linkList a",
        type: "link list link",
        interaction: 1
    },
    // Navigation expand/collapse
    {
        control: ".menu a",
        type: "navigation",
        interaction: 14
    }];

})(jQuery, window);