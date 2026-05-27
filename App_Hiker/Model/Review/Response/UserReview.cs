namespace App_Hiker.Model.Review.Response
{
    public class UserReview
    {
        public int id { get; set; } = 0;

        public bool oculto { get; set; } = false;

        public List<UserReviewPhoto> fotos { get; set; } = new List<UserReviewPhoto>();

        public List<UserReviewTag> tags { get; set; } = new List<UserReviewTag>();

        public string local { get; set; } = String.Empty;

        public float nota { get; set; } = 0f;

        public string descricao { get; set; } = String.Empty;

        public int qnt_likes { get; set; } = 0;

        public int qnt_dislikes { get; set; } = 0;

        public DateTime createdAt { get; set; } = DateTime.Now;
    }
}
