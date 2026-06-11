using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;

using App_Hiker.Model.Api;
using App_Hiker.Model.Review.Request;
using App_Hiker.Model.Review.Response;
using App_Hiker.Model.User.Response;

using App_Hiker.Service.Auth;
using App_Hiker.Service.Review;

using App_Hiker.ViewModel.Base;

namespace App_Hiker.ViewModel.Review
{
    public class NewReviewViewModel : BaseViewModel
    {
        private const string GoogleApiKey = "AIzaSyD8SFvXnT_IaBmd55CFkvflabW-HUWdX-Q";

        private readonly string _context;
        private readonly List<FileResult> _arquivosSelecionados = new();

        private CancellationTokenSource? _cts;
        private bool _suppressSearch = false;

        private string _userRealName = "Usuário";
        private string _fotoUrl = "profile.png";
        private string _descricao = string.Empty;
        private string _local = string.Empty;
        private string _tags = string.Empty;
        private int _selectedRating = 1;
        private int _selectedVisibilityIndex = 0;
        private bool _suggestionsVisible = false;
        private string _addImageCountTexto = "+";
        private PlaceSuggestion? _selectedSuggestion;
        private string? _selectedPlaceId;

        public ObservableCollection<PlaceSuggestion> Suggestions { get; } = new();
        public ObservableCollection<ImageSource> Fotos { get; } = new();
        public List<string> VisibilityOptions { get; } = new() { "Público", "Privado" };

        public string UserRealName
        {
            get => _userRealName;
            set => SetProperty(ref _userRealName, value);
        }

        public string FotoUrl
        {
            get => _fotoUrl;
            set => SetProperty(ref _fotoUrl, value);
        }

        public string Descricao
        {
            get => _descricao;
            set => SetProperty(ref _descricao, value);
        }

        public string Local
        {
            get => _local;
            set
            {
                if (SetProperty(ref _local, value) && !_suppressSearch)
                {
                    _ = SearchCitiesAsync(value);
                }
            }
        }

        public string Tags
        {
            get => _tags;
            set => SetProperty(ref _tags, value);
        }

        public int SelectedRating
        {
            get => _selectedRating;
            set => SetProperty(ref _selectedRating, value);
        }

        public int SelectedVisibilityIndex
        {
            get => _selectedVisibilityIndex;
            set => SetProperty(ref _selectedVisibilityIndex, value);
        }

        public bool SuggestionsVisible
        {
            get => _suggestionsVisible;
            set => SetProperty(ref _suggestionsVisible, value);
        }

        public string AddImageCountTexto
        {
            get => _addImageCountTexto;
            set => SetProperty(ref _addImageCountTexto, value);
        }

        public PlaceSuggestion? SelectedSuggestion
        {
            get => _selectedSuggestion;
            set
            {
                if (SetProperty(ref _selectedSuggestion, value) && value != null)
                {
                    OnSuggestionSelected(value);
                }
            }
        }

        private bool IsPrivate => SelectedVisibilityIndex == 1;

        public ICommand SetRatingCommand { get; }
        public ICommand AddImageCommand { get; }
        public ICommand PostCommand { get; }

        public NewReviewViewModel(string context)
        {
            _context = context;

            SetRatingCommand = new Command<string>(OnSetRating);
            AddImageCommand = new Command(async () => await AddImageAsync());
            PostCommand = new Command(async () => await PostAsync());
        }

        public async Task LoadUserDataAsync()
        {
            try
            {
                IsBusy = true;

                DataResponse<UserDataResponse> response = new DataResponse<UserDataResponse>();

                if (_context == "AuthUserContext")
                {
                    response = await AuthService.Me();
                }

                if (response.data != null)
                {
                    UserRealName = response.data.nome_exibicao;

                    if (!string.IsNullOrEmpty(response.data.foto_url))
                    {
                        FotoUrl = response.data.foto_url;
                    }
                }
            }
            catch (Exception ex)
            {
                await HandleApiErrorAsync(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnSetRating(string? rawRating)
        {
            if (int.TryParse(rawRating, out int rating))
            {
                SelectedRating = rating;
            }
        }

        private async Task SearchCitiesAsync(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro) || filtro.Length < 3)
            {
                SuggestionsVisible = false;

                return;
            }

            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            try
            {
                List<PlaceSuggestion> cidades = await BuscarCidadesAsync(filtro, _cts.Token);

                Suggestions.Clear();

                foreach (PlaceSuggestion cidade in cidades)
                {
                    Suggestions.Add(cidade);
                }

                SuggestionsVisible = Suggestions.Count > 0;
            }
            catch (OperationCanceledException) { /* ignorar, nova busca em andamento */ }
        }

        private static async Task<List<PlaceSuggestion>> BuscarCidadesAsync(string input, CancellationToken ct)
        {
            string url = $"https://maps.googleapis.com/maps/api/place/autocomplete/json" +
                         $"?input={Uri.EscapeDataString(input)}" +
                         $"&types=(cities)" +
                         $"&language=pt-BR" +
                         $"&key={GoogleApiKey}";

            using HttpClient client = new HttpClient();
            string response = await client.GetStringAsync(url, ct);

            JsonDocument json = JsonDocument.Parse(response);
            JsonElement predictions = json.RootElement.GetProperty("predictions");

            List<PlaceSuggestion> cidades = new List<PlaceSuggestion>();

            foreach (JsonElement item in predictions.EnumerateArray())
            {
                string? descricao = item.GetProperty("description").GetString();
                string? placeId = item.TryGetProperty("place_id", out JsonElement pid) ? pid.GetString() : null;

                if (descricao != null)
                {
                    cidades.Add(new PlaceSuggestion { Description = descricao, PlaceId = placeId });
                }
            }

            return cidades;
        }

        private void OnSuggestionSelected(PlaceSuggestion suggestion)
        {
            _suppressSearch = true;
            Local = suggestion.Description;
            _suppressSearch = false;

            _selectedPlaceId = suggestion.PlaceId;
            SuggestionsVisible = false;
        }

        private async Task AddImageAsync()
        {
            if (_arquivosSelecionados.Count >= 5)
            {
                await DisplayAlert("Limite atingido", "Você pode adicionar no máximo 5 imagens.", "OK");

                return;
            }

            try
            {
                PickOptions options = new PickOptions
                {
                    FileTypes = FilePickerFileType.Images
                };

                IEnumerable<FileResult>? results = await FilePicker.PickMultipleAsync(options);

                if (results == null || !results.Any())
                {
                    return;
                }

                int vagas = 5 - _arquivosSelecionados.Count;
                List<FileResult> lista = results.Take(vagas).ToList();

                if (results.Count() > vagas)
                {
                    await DisplayAlert("Limite", $"Apenas {vagas} imagem(ns) foram adicionadas para não exceder o limite de 5.", "OK");
                }

                foreach (FileResult file in lista)
                {
                    _arquivosSelecionados.Add(file);

                    Fotos.Add(ImageSource.FromFile(file.FullPath));
                }

                AddImageCountTexto = $"+ ({_arquivosSelecionados.Count}/5)";
            }
            catch (PermissionException)
            {
                await DisplayAlert("Permissão negada", "Permita o acesso à galeria nas configurações.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async Task PostAsync()
        {
            try
            {
                IsBusy = true;

                if (SelectedRating < 1 || SelectedRating > 5)
                {
                    await DisplayAlert("Avaliação inválida", "Selecione uma nota de 1 a 5.", "OK");

                    return;
                }

                CreateReviewRequest payload = new CreateReviewRequest
                {
                    descricao = Descricao,
                    local = Local,
                    local_id = string.IsNullOrWhiteSpace(_selectedPlaceId) ? Local : _selectedPlaceId,
                    nota = SelectedRating,
                    oculto = IsPrivate,
                    tags = Tags
                };

                DataResponse<CreateReviewResponse> response = _arquivosSelecionados.Count > 0
                    ? await ReviewService.CreateWithPhotos(payload, _arquivosSelecionados)
                    : await ReviewService.Create(payload);

                if (response.statusCode == 201)
                {
                    await DisplayAlert("Sucesso!", "Sua review foi criada com sucesso.", "OK");
                }
            }
            catch (Exception ex)
            {
                await HandleApiErrorAsync(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

    // Sugestão de cidade exibida no autocomplete (Description aparece via ToString).
    public class PlaceSuggestion
    {
        public string Description { get; set; } = string.Empty;
        public string? PlaceId { get; set; }
        public override string ToString() => Description;
    }
}
