var FollowingService = function() {

    var follow = function(artistId, done, fail) {
        $.post("/api/Followings", { artistId: artistId })
            .done(done)
            .fail(fail);
    }

    var unfollow = function(artistId, done, fail) {
        $.ajax( { 
            url: "/api/Followings/" + artistId,
            method: "DELETE"
            })
            .done(done)
            .fail(fail);
    }

    return {
        follow: follow,
        unfollow: unfollow
    };
}();