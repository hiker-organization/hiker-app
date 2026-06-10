using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace App_Hiker.Model.Review.Response
{
    public class UserReview : INotifyPropertyChanged
    {
        private int _id = 0;
        private bool _oculto = false;
        private List<UserReviewPhoto> _fotos = new List<UserReviewPhoto>();
        private List<UserReviewTag> _tags = new List<UserReviewTag>();
        private UserReviewAuthor _autor = new UserReviewAuthor();
        private string _local = String.Empty;
        private float _nota = 0f;
        private string _descricao = String.Empty;
        private int _qnt_likes = 0;
        private int _qnt_dislikes = 0;
        private DateTime _createdAt = DateTime.Now;
        private bool _liked = false;
        private bool _disliked = false;
        private bool _isOwnReview = false;

        public int id { get => _id; set => SetField(ref _id, value); }

        public bool oculto { get => _oculto; set => SetField(ref _oculto, value); }

        public List<UserReviewPhoto> fotos { get => _fotos; set => SetField(ref _fotos, value); }

        public List<UserReviewTag> tags { get => _tags; set => SetField(ref _tags, value); }

        public UserReviewAuthor autor { get => _autor; set => SetField(ref _autor, value); }

        public string local { get => _local; set => SetField(ref _local, value); }

        public float nota { get => _nota; set => SetField(ref _nota, value); }

        public string descricao { get => _descricao; set => SetField(ref _descricao, value); }

        public int qnt_likes { get => _qnt_likes; set => SetField(ref _qnt_likes, value); }

        public int qnt_dislikes { get => _qnt_dislikes; set => SetField(ref _qnt_dislikes, value); }

        public DateTime createdAt { get => _createdAt; set => SetField(ref _createdAt, value); }

        public bool liked { get => _liked; set => SetField(ref _liked, value); }

        public bool disliked { get => _disliked; set => SetField(ref _disliked, value); }

        public bool IsOwnReview { get => _isOwnReview; set => SetField(ref _isOwnReview, value); }

        public event PropertyChangedEventHandler? PropertyChanged;

        private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
    }
}
