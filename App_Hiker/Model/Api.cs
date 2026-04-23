namespace App_Hiker.Model
{
    public class Api<T>
    {
        // Campos.

        private string _message = String.Empty;

        private T? _data = default(T);

        private int? _status_code = null;

        // Atributos.

        public string message
        {
            get => this._message;
            set => this._message = value;
        }

        public T? data
        {
            get => this._data;
            set => this._data = value;
        }

        public int status_code
        {
            get => this._status_code ?? 500;
            set => this._status_code = value;
        }
    }
}
