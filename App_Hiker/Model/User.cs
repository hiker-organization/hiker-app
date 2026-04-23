namespace App_Hiker.Model
{
    public class User
    {
        // Campos.

        private int _id = 0;

        private string _nome_usuario = String.Empty;

        private string _nome_exibicao = String.Empty;

        private string _email = String.Empty;

        private string _senha = String.Empty;

        private string _numero_celular = String.Empty;

        private DateTime? _data_nascimento = null;

        private string _foto_url = String.Empty;

        private double _reputacao = 0.0;

        private string _cargo = String.Empty;

        private DateTime _created_at = DateTime.Now;

        private DateTime _updated_at = DateTime.Now;

        // Atributos.

        public int id
        {
            get => this._id;
            set => this._id = value;
        }

        public string nome_usuario
        {
            get => this._nome_usuario;
            set => this._nome_usuario = value.Replace(" ", "_");
        }

        public string nome_exibicao
        {
            get => this._nome_exibicao;
            set => this._nome_exibicao = value;
        }

        public string email
        {
            get => this._email;
            set => this._email = value;
        }

        public string senha
        {
            get => this._senha;
            set => this._senha = value;
        }

        public string numero_celular
        {
            get => this._numero_celular;
            set => this._numero_celular = value;
        }

        public DateTime? data_nascimento
        {
            get => this._data_nascimento;
            set => this._data_nascimento = value;
        }

        public string foto_url
        {
            get => this._foto_url;
            set => this._foto_url = value;
        }

        public double reputacao
        {
            get => this._reputacao;
            set => this._reputacao = value;
        }

        public string cargo
        {
            get => this._cargo;
            set => this._cargo = value;
        }

        public DateTime createdAt
        {
            get => this._created_at;
            set => this._created_at = value;
        }

        public DateTime updatedAt
        {
            get => this._updated_at;
            set => this._updated_at = value;
        }
    }
}
