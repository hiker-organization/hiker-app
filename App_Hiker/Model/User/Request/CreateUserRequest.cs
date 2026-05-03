namespace App_Hiker.Model.User.Request
{
    public class CreateUserRequest
    {
        public string nome_usuario { get; set; } = String.Empty;

        public string nome_exibicao { get; set; } = String.Empty;

        public string email { get; set; } = String.Empty;

        public string senha { get; set; } = String.Empty;

        public string numero_celular { get; set; } = String.Empty;

        public DateTime? data_nascimento { get; set; } = null;
    }
}
