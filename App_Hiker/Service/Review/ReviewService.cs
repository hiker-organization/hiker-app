using App_Hiker.Model.Api;
using App_Hiker.Model.Review.Request;
using App_Hiker.Model.Review.Response;

using Newtonsoft.Json;

namespace App_Hiker.Service.Review
{
    class ReviewService
    {
        public static async Task<DataResponse<CreateReviewResponse>> Create(CreateReviewRequest payload)
        {
            string request_json = JsonConvert.SerializeObject(payload);

            string response_json = await ApiService.PostData("/review", request_json);

            return JsonConvert.DeserializeObject<DataResponse<CreateReviewResponse>>(response_json) ?? new DataResponse<CreateReviewResponse>();
        }
    }
}
