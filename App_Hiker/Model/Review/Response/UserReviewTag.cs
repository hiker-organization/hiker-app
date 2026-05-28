namespace App_Hiker.Model.Review.Response
{
    public class UserReviewTag
    {
        // Some endpoints return { "tag": { "descritivo": "..." } }
        // Others return flat objects like { "id":1, "id_review":51, "id_tag":7 }
        // Support both shapes by exposing both fields.
        public UserReviewTagValue tag { get; set; } = new UserReviewTagValue();

        public int id { get; set; } = 0;
        public int id_review { get; set; } = 0;
        public int id_tag { get; set; } = 0;

        private string _descritivo = string.Empty;
        public string descritivo
        {
            get
            {
                if (!string.IsNullOrEmpty(_descritivo)) return _descritivo;
                if (!string.IsNullOrEmpty(tag?.descritivo)) return tag.descritivo;
                if (id_tag > 0) return $"#{id_tag}";
                return string.Empty;
            }
            set => _descritivo = value;
        }
    }

    public class UserReviewTagValue
    {
        public string descritivo { get; set; } = String.Empty;
    }
}
