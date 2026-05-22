namespace App_Hiker.Model.Review.Request
{
    class CreateReviewRequest
    {
        public string descricao { get; set; } = String.Empty;

        public string local_id { get; set; } = String.Empty;

        public string local { get; set; } = String.Empty;

        public int nota { get; set; } = 0;

        public string tags { get; set; } = String.Empty;

        public bool oculto { get; set; } = false;
    }
}
