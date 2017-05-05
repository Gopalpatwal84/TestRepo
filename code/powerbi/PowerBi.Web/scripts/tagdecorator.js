(function ($, global) {
    "use strict";

    $(global.tagDecorator = function () {
        // tagDecoratorConfig defined in tagDecoratorConfig.js
        var config = global.tagDecoratorConfig || {},

            // Number of Contols to decorate
            items = config.length,
            i = 0,

            // Link Lists found on the page
            linkLists = $(".linkList"),

            elements;

        // Phase I: Loop through each Control
        for (i = 0; i < items; i++) {

            // Grab all instances of the control on the page
            elements = $(config[i].control);
            elements.each(function (index, element) {

                var that = $(element),

                    // Area defined by data-area as body, footer, header
                    area = that.parents("[data-tag-area]").data("tag-area") || "",

                    // Group defined by data-group as content, footer, header
                    // unless specified as more specific, ie data-tag-group="hero"
                    group = that.parents("[data-tag-group]").data("tag-group") || "",

                    // Type defined in TagDecoratorConfig
                    type = config[i].type || "",

                    // The element text
                    name = that[0].text ? that[0].text.replace(/^\s+|\s+$/g, '') : "",

                    // The element title
                    title = that[0].title || "",

                    // Interaction defined in TagDecoratorConfig
                    interaction = config[i].interaction || "";

                that.attr("ms.pgarea", area)
                    .attr("ms.cmpgrp", group)
                    .attr("ms.cmptyp", type)
                    .attr("ms.cmpnm", name)
                    .attr("ms.title", title).attr("km.title", title)
                    .attr("ms.interactiontype", interaction);
            });
        }

        // Phase II: Enumerate links in Link Lists, processing index
        linkLists.each(function (index, element) {

            var that = $(element),

                // List heading
                heading = that.prev("h1, h2, h3, h4, h5, h6"),
                headingText = $(heading[0]).text(),

                // Individual list elements
                items = that.children("li");

            items.each(function (i, el) {
                var anchor = $(el).find("a"),
                    component = anchor.attr("ms.cmpnm");

                anchor.attr("ms.index", i)
                    .attr("ms.cmpnm", headingText + " | " + component);
            });
        });
    });
})(jQuery, window);