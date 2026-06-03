using App_Hiker.Model.Api;
using App_Hiker.Model.Review.Request;
using App_Hiker.Model.Review.Response;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace App_Hiker.Service.Review
{
    class ReviewService
    {
        public static async Task<DataResponse<FeedResponse>> GetFeed()
        {
            string response_json = await ApiService.GetData("/review");
            JObject root = JObject.Parse(response_json);

            List<UserReview> reviews = new List<UserReview>();
            JToken? dataToken = root["data"];

            if (dataToken is JArray dataArray)
            {
                reviews = dataArray.ToObject<List<UserReview>>() ?? new List<UserReview>();
            }
            else if (dataToken is JObject dataObject)
            {
                JToken? reviewsToken = dataObject["reviews"];

                if (reviewsToken is JArray reviewsArray)
                {
                    reviews = reviewsArray.ToObject<List<UserReview>>() ?? new List<UserReview>();
                }
            }

            return new DataResponse<FeedResponse>
            {
                message = root["message"]?.ToString() ?? String.Empty,
                statusCode = root["statusCode"]?.Value<int>() ?? 200,
                data = new FeedResponse
                {
                    reviews = reviews
                }
            };
        }

        public static async Task<DataResponse<FeedResponse>> Search(string term)
        {
            string endpoint = $"/review/search/{Uri.EscapeDataString(term)}";

            string response_json = await ApiService.GetData(endpoint);
            JObject root = JObject.Parse(response_json);

            List<UserReview> reviews = new List<UserReview>();
            JToken? dataToken = root["data"];

            if (dataToken is JArray dataArray)
            {
                reviews = dataArray.ToObject<List<UserReview>>() ?? new List<UserReview>();
            }

            return new DataResponse<FeedResponse>
            {
                message = root["message"]?.ToString() ?? String.Empty,
                statusCode = root["statusCode"]?.Value<int>() ?? 200,
                data = new FeedResponse
                {
                    reviews = reviews
                }
            };
        }

        public static async Task<DataResponse<CreateReviewResponse>> Create(CreateReviewRequest payload)
        {
            string request_json = JsonConvert.SerializeObject(payload);

            string response_json = await ApiService.PostData("/review", request_json);

            return JsonConvert.DeserializeObject<DataResponse<CreateReviewResponse>>(response_json) ?? new DataResponse<CreateReviewResponse>();
        }

        public static async Task<DataResponse<CreateReviewResponse>> CreateWithPhotos(CreateReviewRequest payload, List<FileResult> fotos)
        {
            using MultipartFormDataContent form_data = new MultipartFormDataContent();

            form_data.Add(new StringContent(payload.descricao ?? string.Empty), "descricao");
            form_data.Add(new StringContent(payload.local ?? string.Empty), "local");
            form_data.Add(new StringContent(payload.local_id ?? string.Empty), "local_id");
            form_data.Add(new StringContent(payload.nota.ToString()), "nota");
            form_data.Add(new StringContent(payload.tags ?? string.Empty), "tags");
            form_data.Add(new StringContent(payload.oculto.ToString().ToLower()), "oculto");

            foreach (FileResult foto in fotos)
            {
                Stream stream = await foto.OpenReadAsync();
                StreamContent streamContent = new StreamContent(stream);
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(foto.ContentType);
                form_data.Add(streamContent, "fotos", foto.FileName);
            }

            string response_json = await ApiService.PostMultipart("/review", form_data);

            return JsonConvert.DeserializeObject<DataResponse<CreateReviewResponse>>(response_json) ?? new DataResponse<CreateReviewResponse>();
        }

        public static async Task Like(int reviewId)
        {
            await ApiService.PostData($"/review/{reviewId}/like", "{}");
        }

        public static async Task Dislike(int reviewId)
        {
            await ApiService.PostData($"/review/{reviewId}/dislike", "{}");
        }
    }
}
