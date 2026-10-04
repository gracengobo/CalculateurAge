using CalculateurAge.Services;

namespace CalculateurAge.ViewModels;

public class ResultatViewModel : BaseViewModel
{
    private string _nom = "";
    private int _age;
    private string _message = "";
    private string _infoAnniversaire = "";

    public string Nom
    {
        get => _nom;
        set => SetField(ref _nom, value);
    }

    public int Age
    {
        get => _age;
        set => SetField(ref _age, value);
    }

    public string Message
    {
        get => _message;
        set => SetField(ref _message, value);
    }

    public string InfoAnniversaire
    {
        get => _infoAnniversaire;
        set => SetField(ref _infoAnniversaire, value);
    }

    public RelayCommand RetourCommand { get; }

    private INavigationService _navigationService;

    public ResultatViewModel()
    {
        // Obtenir le service de navigation du container
        _navigationService = MauiProgram.ServiceProvider?.GetService<INavigationService>();

        // Fallback: créer une implémentation directe si ServiceProvider n'est pas disponible
        _navigationService ??= new ShellNavigationService();

        RetourCommand = new RelayCommand(Retour);
    }

    private async void Retour()
    {
        try
        {
            if (_navigationService != null)
                await _navigationService.GoBackAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
        }
    }
}