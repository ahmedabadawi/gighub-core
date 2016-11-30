using GigHub.Web.Core.Models;

namespace GigHub.Web.Core.Repositories
{
    public interface IFollowingRepository
    {
        Following GetFollowing(string userId, string artistId);
    }
}