namespace App_Hiker.Model.Review.Response
{
    class CreateReviewResponse
    {
        public string local { get; set; } = String.Empty;

        public string descricao { get; set; } = String.Empty;

        public int nota { get; set; } = 0;
    }
}
