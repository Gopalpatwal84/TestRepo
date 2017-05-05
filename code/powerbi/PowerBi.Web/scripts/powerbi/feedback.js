(function ($, global, storage) {
    'use strict';
    
    var VOTE_EXPIRATION = 90,
        $feedbackContainers = $('[data-control="feedback-vote-container"]'),
        today = new Date(),
        voteKey,
        voteDate;

    //If there is no feedback-vote-container in the page we do nothing
    if ($feedbackContainers.length) {
        // First we check if this article was already voted or not
        // If it was already voted we don't load the vote control
        voteKey = $feedbackContainers.data('vote-slug');

        if (!storage.get('vote.' + voteKey)) {
            $feedbackContainers.show();
        } else {
            voteDate = new Date(storage.get('vote.' + voteKey));
            voteDate.setDate(voteDate.getDate() + VOTE_EXPIRATION);

            if (today > voteDate) {
                $feedbackContainers.show();
            }
        }
    }

    function storeFeedbackVote(e, form) {
        if ($(form).data("form-type") === "feedback") {
            storage.set('vote.' + voteKey, new Date());
        }
    }

    $(global).on("form-submitted", storeFeedbackVote);
})(jQuery, window, window.sd.storage);