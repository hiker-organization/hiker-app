using App_Hiker.Model.Review;

namespace App_Hiker.Model.User.Response
{
    public class UserDataResponse
    {
        public string foto_url { get; set; } = String.Empty;

        public string nome_exibicao { get; set; } = String.Empty;

        public string nome_usuario { get; set; } = String.Empty;

        public float reputacao { get; set; } = 0f;

        public string reputacao_normalizada
        {
            get
            {
                return (this.reputacao * 10).ToString("F2") + "%";
            }
        }

        public List<UserGroupReviews> reviews { get; set; } = new List<UserGroupReviews>();
    }
}
