namespace App_Hiker.Model.User.Response
{
    public class CreateUser
    {
        public string nome_usuario { get; set; } = String.Empty;

        public string nome_exibicao { get; set; } = String.Empty;

        public string email { get; set; } = String.Empty;

        public string senha { get; set; } = String.Empty;
    }
}
