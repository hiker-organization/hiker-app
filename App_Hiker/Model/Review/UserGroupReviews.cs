namespace App_Hiker.Model.Review
{
    public class UserGroupReviews
    {
        public List<UserGroupReviewsPhoto> fotos { get; set; } = new List<UserGroupReviewsPhoto>();

        public string local { get; set; } = String.Empty;

        public float nota { get; set; } = 0f;

        public string descricao { get; set; } = String.Empty;

        public int qnt_likes { get; set; } = 0;

        public int qnt_dislikes { get; set; } = 0;

        public DateTime createdAt { get; set; } = DateTime.Now;
    }
}
