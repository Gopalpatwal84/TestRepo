(function($, global) {
    'use strict';

    var $tables = $('.sd-table'),
        $window = $(window);

    $tables.each(function(index) {
        var $table = $(this),
            $columns = $table.find('th'),
            columnCount = addIterativeClasses('column', $columns),
            $currentColumn,
            $select,
            tempClass;

        addIterativeClasses('column', $('tr > td', $table), columnCount);
        displayShowSmallColumns($table);

        $select = global.sd.multiselect($table.parent(), $columns, 'table-' + index);

        $window.on('multiselect-update.table-' + index, function() {
            var hasEmptyCell = $columns.first().text() === '',
                i = hasEmptyCell ? 1 : 0;

            for (i; i < arguments.length; i++) {
                if (!arguments[i].hasOwnProperty('target')) {
                    $currentColumn = $($columns[i - (hasEmptyCell ? 0 : 1)]);

                    tempClass = $currentColumn.attr('class').match(/(column-)\w+/)[0];

                    if ($(arguments[i]).is(':checked')) {
                        $currentColumn.css('display', 'table-cell');
                        $('.' + tempClass, $table).css('display', 'table-cell');
                    } else {
                        $('.' + tempClass, $table).css('display', 'none');
                    }
                }
            }
        });

        $window.on('resize', global.sd.debounce(function() {
            $select.find('input').each(function() {
                var $this = $(this);

                if ($window.width() >= global.sd.breakpoints.SMALL) {
                    if(!$this.is(':checked')) {
                        $this.trigger('click');
                    }
                } else {
                    if($this.is(':checked')) {
                        $this.trigger('click');
                    }
                }
            });
        }, 200));
    });

    function addIterativeClasses(classPrefix, collection, indexOverride) {
        var itemIndex = 0;

        collection.each(function() {
            var $item = $(this);

            if (indexOverride) {
                itemIndex = itemIndex === indexOverride ? 0 : itemIndex;
            }

            itemIndex++;
            $item.addClass(classPrefix + '-' + itemIndex);
        });

        return itemIndex;
    }

    function displayShowSmallColumns($table) {
        $('.show-small', $table).each(function () {
            var columnClass = $(this).attr('class').match(/column-\w/)[0];
            $('.' + columnClass, $table).css('display', 'table-cell');
        });
    }

})(jQuery, window);
