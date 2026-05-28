using System.Collections.Generic;

namespace App_Hiker.Model.Review.Response
{
    public class FeedResponse
    {
        public List<UserReview> reviews { get; set; } = new List<UserReview>();
    }
}
