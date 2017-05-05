(function ($, global) {
    'use strict';

    global.onyx.tagManager = function () {
        var mediaLibrary = {},
			mediaTag = {},
			head = document.head || document.getElementsByTagName('head')[0];

        /**
		 * Process the script source, including cache busting
		 * @param scriptSource source of script to be cache busted
		 */
        function processTagSource(scriptSource) {
            scriptSource += '{0}random=' + Date.now();
            scriptSource = scriptSource.indexOf('?') === -1 ? scriptSource.replace('{0}', '?') : scriptSource.replace('{0}', '&');
            return scriptSource;
        }

        /**
		 * Traverses the tags in the tag json and inserts a tracking script
		 * @param scriptTag the individual script Tag
		 */
        function insertScript(scriptTag) {
            var scriptUrl = this.script;

            // Atach to event
            if (scriptTag.events && scriptTag.selector) {
                $('body').on(scriptTag.events, scriptTag.selector, function () {
                    addScript(scriptUrl, scriptTag);
                });

                // Fire immediately
            } else {
                addScript(scriptUrl, scriptTag);
            }
        }

        /**
		 * Includes a tracking script
		 * @param scriptUrl the base url of the script
		 * @param scriptTag the tag rule object for respective tag
		 */
        function addScript(scriptUrl, scriptTag) {
            var script = '';

            if (scriptUrl && scriptTag) {
                try {
                    if (!document.createElement || !head.appendChild) {
                        return false;
                    } else {
                        script = document.createElement('script');
                        script.src = processTagSource(scriptUrl + scriptTag.tag);

                        // if a script need to have a specific ID
                        script.id = scriptTag.id || '';

                        // if a script requires a call back after load
                        if (scriptTag.callback && typeof scriptTag.callback === 'function') {
                            script.onload = scriptTag.callback;
                        }

                        head.appendChild(script);
                    }
                }
                catch (ex) { }
            }
        }

        /**
		 * Traverses the pixels in the tag json and inserts a tracking pixel
		 * @param pixelTag the individual pixel Tag
		 */
        function insertPixel(pixelTag) {
            var pixelUrl = this.script;

            // Atach to event
            if (pixelTag.events && pixelTag.selector) {
                $('body').on(pixelTag.events, pixelTag.selector, function () {
                    addPixel(pixelUrl, pixelTag);
                });
            }

                // Fire immediately
            else {
                addPixel(pixelUrl, pixelTag);
            }
        }

        /**
		* Includes a one-pixel tracking image
		* @param pixelUrl the base url of the script
		* @param pixelTag the pixel tag appended to the script url
		*/
        function addPixel(pixelUrl, pixelTag) {
            var image;

            if (pixelUrl && pixelTag) {
                try {
                    image = new Image(1, 1);
                    image.src = processTagSource(pixelUrl + pixelTag.tag);
                }
                catch (ex) { }
            }
        }

        /**
		 * Processes an individual tag type
		 * @param currentTag the full tag context
		 */
        function processTag(currentTag) {
            // Do not fire any tags if the url pattern is undefined
            if (currentTag.pattern && currentTag.script) {
                var pageExp = new RegExp(currentTag.pattern, 'i');

                // Traverse tags if:
                // -- not disabled
                // -- matches current page
                if (!currentTag.disabled && pageExp.test(global.onyx.tagManager.tagConfig.page)) {
                    var timeout = currentTag.timeout || 0;

                    // Trigger Script tags
                    if (currentTag.tags && currentTag.tags.length > 0) {
                        $.each(currentTag.tags, function (property, scriptTag) {
                            setTimeout(function () { insertScript.call(currentTag, scriptTag); }, timeout);
                        });
                    }

                    // Trigger Img/Pixel tags
                    if (currentTag.pixels && currentTag.pixels.length > 0) {
                        $.each(currentTag.pixels, function (property, pixelTag) {
                            insertPixel.call(currentTag, pixelTag);
                        });
                    }
                }
            }
        }

        /**
		 * Parse the script URLs defined in TagConfig.js
		 */
        function debugScriptUrls() {
            var script,
				scripts = [],
				library,
				currentLibrary,
				count,
				url;

            console.groupCollapsed('Script URLs');

            for (script in global.onyx.tagManager.tagConfig.tagUrl) {
                count = 0;
                url = global.onyx.tagManager.tagConfig.tagUrl[script];

                for (library in global.onyx.tagManager.media) {
                    currentLibrary = global.onyx.tagManager.media[library];

                    for (var i = 0; i < currentLibrary.length; i++) {
                        var s = currentLibrary[i];

                        if (s.script && s.script.indexOf(url) > -1) {
                            count++;
                        }
                    }
                }

                scripts.push({
                    Name: script,
                    URL: url,
                    Count: count
                });

                if (count === 0) {
                    console.warn('SCRIPT URL NOT USED: ' + script);
                }
            }

            console.table(scripts);
            console.groupEnd();
        }

        /**
		 * Parse the page URLs defined in TagConfig.js
		 */
        function debugPageUrls(testPages) {
            var page,
				pages = [],
				library,
				currentLibrary,
				count,
				url,
				xhr,
				valid;

            console.groupCollapsed('Page URLs');
            if (testPages) {
                for (page in global.onyx.tagManager.tagConfig.pageUrl) {
                    count = 0;
                    url = global.onyx.tagManager.tagConfig.pageUrl[page];

                    for (library in global.onyx.tagManager.media) {
                        currentLibrary = global.onyx.tagManager.media[library];

                        for (var i = 0; i < currentLibrary.length; i++) {
                            var s = currentLibrary[i];

                            if (s.pattern && s.pattern === url) {
                                count++;
                            }
                        }
                    }

                    valid = false;
                    if (url && url.search(/\[|\]|\(|\)|\{|\}/g) === -1) {
                        xhr = new XMLHttpRequest();
                        xhr.open('HEAD', url.replace('$', '').replace('//', '/'), false);
                        xhr.send(null);

                        if (xhr.status === 200) {
                            valid = true;
                        }
                    } else {
                        console.warn('UNABLE TO VALIDATE PATTERN: ' + url)
                    }

                    pages.push({
                        Name: page,
                        URL: url,
                        Count: count,
                        Valid: valid
                    });

                    if (count === 0) {
                        console.warn('PAGE URL NOT USED: ' + page);
                    }
                }

                console.table(pages);
            } else {
                console.warn('Testing pages skipped. Use ".Debug(true)" to test the page URLs.')
            }
            console.groupEnd();
        }

        /**
		 * Display the page-specific tagging
		 */
        function debugCurrentPage(currentPage) {
            console.groupCollapsed('Current Page');
            console.table(currentPage);
            console.groupEnd();
        }

        /**
		 * Display the tag information in console
         * @param testPages test page URLs
		 */
        function debug(testPages) {
            var library,
				currentLibrary,
				currentPage = [],
				formattedLibrary;

            if (console.table && console.warn && console.group) {
                debugScriptUrls();
                debugPageUrls(testPages);

                // Validate the media tags
                for (library in global.onyx.tagManager.media) {
                    console.groupCollapsed(library);
                    currentLibrary = global.onyx.tagManager.media[library];
                    formattedLibrary = currentLibrary.map(function (current) {
                        var pixelList = null,
							tagsList = null,
							pixels = current.pixels,
							tags = current.tags,
							pattern = current.pattern,
							script = current.script;

                        if (pixels) {
                            pixelList = pixels.map(function (pix) { return pix.tag; }).join(' ');
                        }

                        if (tags) {
                            tagsList = tags.map(function (tag) { return tag.tag; }).join(' ');
                        }

                        if (!pattern) {
                            console.error('MISSING PATTERN: ' + current.name);
                        }

                        if (!script) {
                            console.error('MISSING SCRIPT: ' + current.name);
                        }

                        if (tags && tags.length > 0) {
                            console.warn('EMBEDDING ' + tags.length + ' SCRIPT TAG(S): ' + library + ' on ' + current.pattern);
                        }

                        if (global.onyx.tagManager.tagConfig.page.match(pattern)) {
                            currentPage.push({
                                Library: library,
                                Name: current.name,
                                Pattern: current.pattern,
                                Script: current.script,
                                Pixels: pixelList,
                                Tags: tagsList
                            });
                        }

                        return {
                            Name: current.name,
                            Pattern: current.pattern,
                            Script: current.script,
                            Pixels: pixelList,
                            Tags: tagsList
                        }
                    })

                    console.table(formattedLibrary);
                    console.groupEnd();
                }

                debugCurrentPage(currentPage);
            }
        }

        // Traverse the Tags in the Library
        for (mediaLibrary in global.onyx.tagManager.media) {
            var currentLibrary = global.onyx.tagManager.media[mediaLibrary],
				i;

            // Traverse the Tags within each library
            for (i = 0; i < currentLibrary.length; i++) {
                processTag(currentLibrary[i]);
            }
        }

        return {
            Debug: debug
        }
    };

    $(function () {
        global._tagManager = global._tagManager || new global.onyx.tagManager();
    });

    global.onyx = global.onyx || {};
})(jQuery, window);