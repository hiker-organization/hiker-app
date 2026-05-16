using App_Hiker.Model.Api;
using App_Hiker.Model.Review.Request;
using App_Hiker.Model.Review.Response;
using App_Hiker.Service.Review;
using System.Text.Json;

namespace App_Hiker.View.Review;


public partial class NewReview : ContentView
{
    private const string GoogleApiKey = "AIzaSyD8SFvXnT_IaBmd55CFkvflabW-HUWdX-Q";
    private CancellationTokenSource? _cts;

    private int _selectedRating = 1;
    private readonly List<Label> _stars = new();
    private readonly List<ImageSource> _imagensSelecionadas = new();

    private bool _isPrivate = false;
    private string? _selectedPlaceId; // armazena o place_id selecionado

    public NewReview()
    {
        InitializeComponent();
        SetupPicker();
        SetupStars();
        SetRating(_selectedRating);
    }

    // ── Picker Público / Privado ──────────────────────────────────────
    private void SetupPicker()
    {
        PickerVisibility.Items.Add("Público");
        PickerVisibility.Items.Add("Privado");
        PickerVisibility.SelectedIndex = 0;
    }

    private void PickerVisibility_Changed(object sender, EventArgs e)
    {
        var selected = PickerVisibility.SelectedItem?.ToString();
        // Use 'selected' conforme precisar ("Público" ou "Privado")

        _isPrivate = string.Equals(selected, "Privado", StringComparison.OrdinalIgnoreCase);
    }

    // ── Estrelas interativas ──────────────────────────────────────────
    private void SetupStars()
    {
        _stars.AddRange(new[] { Star1, Star2, Star3, Star4, Star5 });

        for (int i = 0; i < _stars.Count; i++)
        {
            int index = i + 1; // captura o índice correto no closure

            // Toque confirma a nota
            var tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) => SetRating(index);
            _stars[i].GestureRecognizers.Add(tap);

            // Hover (PointerGestureRecognizer — MAUI >= .NET 8)
            var pointer = new PointerGestureRecognizer();
            pointer.PointerEntered += (s, e) => HighlightStars(index);
            pointer.PointerExited += (s, e) => HighlightStars(_selectedRating);
            _stars[i].GestureRecognizers.Add(pointer);
        }
    }

    private void SetRating(int rating)
    {
        _selectedRating = rating;
        HighlightStars(rating);
    }

    private void HighlightStars(int upTo)
    {
        for (int i = 0; i < _stars.Count; i++)
            _stars[i].TextColor = i < upTo
                ? Color.FromArgb("#FFC107")  // amarelo
                : Colors.LightGray;
    }

    private async void CityEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var filtro = e.NewTextValue;

        if (string.IsNullOrWhiteSpace(filtro) || filtro.Length < 3)
        {
            SuggestionsList.IsVisible = false;
            return;
        }

        // Cancela requisição anterior se o usuário ainda está digitando
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        try
        {
            var cidades = await BuscarCidadesAsync(filtro, _cts.Token);
            SuggestionsList.ItemsSource = cidades;
            SuggestionsList.IsVisible = cidades.Any();
        }
        catch (OperationCanceledException) { /* ignorar, nova busca em andamento */ }
    }

    private async Task<List<PlaceSuggestion>> BuscarCidadesAsync(string input, CancellationToken ct)
    {
        var url = $"https://maps.googleapis.com/maps/api/place/autocomplete/json" +
                  $"?input={Uri.EscapeDataString(input)}" +
                  $"&types=(cities)" +
                  $"&language=pt-BR" +
                  $"&key={GoogleApiKey}";

        using var client = new HttpClient();
        var response = await client.GetStringAsync(url, ct);

        var json = JsonDocument.Parse(response);
        var predictions = json.RootElement.GetProperty("predictions");

        var cidades = new List<PlaceSuggestion>();
        foreach (var item in predictions.EnumerateArray())
        {
            var descricao = item.GetProperty("description").GetString();
            var placeId = item.TryGetProperty("place_id", out var pid) ? pid.GetString() : null;
            if (descricao != null)
                cidades.Add(new PlaceSuggestion { Description = descricao, PlaceId = placeId });
        }

        return cidades;
    }

    private void SuggestionsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is PlaceSuggestion suggestion)
        {
            CityEntry.Text = suggestion.Description;
            _selectedPlaceId = suggestion.PlaceId; // guarda o place_id para enviar ao backend
            SuggestionsList.IsVisible = false;
        }
    }

    private async void AddImage_Clicked(object sender, EventArgs e)
    {
        if (_imagensSelecionadas.Count >= 5)
        {
            await ShowAlert("Limite atingido", "Você pode adicionar no máximo 5 imagens.", "OK");
            return;
        }

        try
        {
            var options = new PickOptions
            {
                FileTypes = FilePickerFileType.Images
            };

            var results = await FilePicker.PickMultipleAsync(options);

            if (results == null || !results.Any()) return;

            // Quantas ainda cabem
            int vagas = 5 - _imagensSelecionadas.Count;
            var lista = results.Take(vagas).ToList();

            if (results.Count() > vagas)
                await ShowAlert("Limite", $"Apenas {vagas} imagem(ns) foram adicionadas para não exceder o limite de 5.", "OK");

            foreach (var file in lista)
            {
                var stream = await file.OpenReadAsync();
                var imageSource = ImageSource.FromStream(() => stream);
                _imagensSelecionadas.Add(imageSource);

                ImagesContainer.Children.Add(new Image
                {
                    Source = imageSource,
                    HeightRequest = 100,
                    WidthRequest = 100,
                    Aspect = Aspect.AspectFill
                });
            }

            AddImageButton.Text = $"+ Adicionar Imagem ({_imagensSelecionadas.Count}/5)";
        }
        catch (PermissionException)
        {
            await ShowAlert("Permissão negada", "Permita o acesso à galeria nas configurações.", "OK");
        }
        catch (Exception ex)
        {
            await ShowAlert("Erro", ex.Message, "OK");
        }
    }

    private async Task ShowAlert(string title, string message, string button)
    {
        var page = Application.Current?.Windows.FirstOrDefault()?.Page;
        if (page != null)
            await page.DisplayAlertAsync(title, message, button);
        else
            System.Diagnostics.Debug.WriteLine($"Não foi possível exibir alerta: nenhuma Window ativa. {title} - {message}");
    }

    private async void Button_Clicked_Post(object sender, EventArgs e)
    {
        try
        {
            if (_selectedRating < 1 || _selectedRating > 5)
            {
                await ShowAlert("Avaliação inválida", "Selecione uma nota de 1 a 5.", "OK");
                return;
            }
            CreateReviewRequest payload = new CreateReviewRequest
            {
                descricao = DescriptionEditor.Text,
                local = CityEntry.Text,
                // enviar o place_id quando disponível, caso contrário fallback para o nome
                local_id = string.IsNullOrWhiteSpace(_selectedPlaceId) ? CityEntry.Text : _selectedPlaceId,
                nota = _selectedRating,
                oculto = _isPrivate,
                tags = TagsEntry.Text,
            };

            DataResponse<CreateReviewResponse> response = await ReviewService.Create(payload);
            if (response.statusCode == 201)
            {
                await ShowAlert("Sucesso!", "Sua review foi criada com sucesso.", "OK");
            }
        }
        catch (Exception ex)
        {
            await ShowAlert("Erro ao criar review", ex.Message, "OK");
        }
    }

    // Classe auxiliar para representar sugestão (mostra Description no UI via ToString)
    private class PlaceSuggestion
    {
        public string Description { get; set; } = string.Empty;
        public string? PlaceId { get; set; }
        public override string ToString() => Description;
    }
}