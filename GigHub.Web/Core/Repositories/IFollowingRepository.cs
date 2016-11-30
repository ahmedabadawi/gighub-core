using GigHub.Web.Core.Models;

namespace GigHub.Web.Core.Repositories
{
    public interface IFollowingRepository
    {
        Following GetFollowing(string userId, string artistId);
        void Add(Following following);
        void Remove(Following following);
    }
}