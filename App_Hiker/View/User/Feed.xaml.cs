using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using App_Hiker.Model.Review.Response;
using App_Hiker.Service.Review;

namespace App_Hiker.View.User;

public partial class Feed : ContentView
{
	private bool _initialized = false;

	public Feed()
	{
		InitializeComponent();
	}

	protected override void OnHandlerChanged()
	{
		base.OnHandlerChanged();

		if (Handler != null && !_initialized)
		{
			_initialized = true;
			_ = LoadFeedAsync();
		}
	}

	private async Task LoadFeedAsync()
	{
		try
		{
			var response = await ReviewService.GetFeed();
			clview_feed.ItemsSource = response.data?.reviews ?? new List<UserReview>();
		}
		catch (Exception ex)
		{
			App.ShowInDebugConsole(ex.Message); // Temporário.
		}
	}

	private async void Reaction_Clicked(object sender, EventArgs e)
	{
		try
		{
			if (sender is not Button button || button.BindingContext is not UserReview review)
			{
				return;
			}

			string reactionType = button.ClassId ?? String.Empty;

			if (reactionType == "like")
			{
				if (review.liked)
				{
					return;
				}

				bool wasDisliked = review.disliked;

				await ReviewService.Like(review.id);

				review.liked = true;
				review.disliked = false;
				review.qnt_likes += 1;

				if (wasDisliked && review.qnt_dislikes > 0)
				{
					review.qnt_dislikes -= 1;
				}
			}
			else if (reactionType == "dislike")
			{
				if (review.disliked)
				{
					return;
				}

				bool wasLiked = review.liked;

				await ReviewService.Dislike(review.id);

				review.disliked = true;
				review.liked = false;
				review.qnt_dislikes += 1;

				if (wasLiked && review.qnt_likes > 0)
				{
					review.qnt_likes -= 1;
				}
			}
		}
		catch (Exception ex)
		{
			App.ShowInDebugConsole(ex.Message); // Temporário.
		}
	}
}