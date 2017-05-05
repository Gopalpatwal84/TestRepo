$(document).ready(function () {
    (function ($, global, localStorage) {
            $('.exit-intent-area').on('mouseover', function () {
                if(localStorage.get("eBookEIP") !== "true"){
                    $(this).trigger('click');
                    localStorage.set("eBookEIP", true);
                } 
            })
    })(jQuery, window, sd.storage);
})


