(function(global) {
    'use strict';

    /*
     * pass in a serialized string in the format ?key1=val1&key2=val2
    */
    var qs2object = function (serializedData) {
        var i,
            item,
            dataString = serializedData.indexOf('?') === 0 ? serializedData.substr(1) : serializedData,
            dataArray = dataString.split('&'),
            output = {};

        if (!!serializedData) {
            for (i = 0; i < dataArray.length; i++) {
                item = dataArray[i].split('=');
                output[item[0]] = item[1];
            }
        }

        return output;
    };

    global.sd = global.sd || {};
    global.sd.qs2object = qs2object;
})(window);