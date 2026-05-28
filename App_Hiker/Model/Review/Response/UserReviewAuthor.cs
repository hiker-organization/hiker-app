namespace App_Hiker.Model.Review.Response
{
    public class UserReviewAuthor
    {
        public string nome_exibicao { get; set; } = string.Empty;

        public string? foto_url { get; set; } = null;

        public int reputacao { get; set; } = 0;

        public string nome_usuario { get; set; } = string.Empty;
    }
}
