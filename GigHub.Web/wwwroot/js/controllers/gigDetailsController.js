var GigDetailsController = function(followingService) {
    var button;
    
    var init = function(container) {
        $(container).on("click", ".js-toggle-follow", toggleFollowing); 
    };

    var toggleFollowing = function(e) {
        button = $(e.target);
        
        var artistId = button.attr("data-artist-id");
        
        if (button.hasClass("btn-default")) {
            followingService.follow(artistId, done, fail);
        } else {
            followingService.unfollow(artistId, done, fail);
        }
    };

    var done = function() {
        var text = (button.text() == "Follow") ? "Following" : "Follow";
        button
            .toggleClass("btn-info")
            .toggleClass("btn-default")
            .text(text);
    };
    
    var fail = function() {
        alert("Something failed!");
    };

    return {
        init: init
    };
}(FollowingService);