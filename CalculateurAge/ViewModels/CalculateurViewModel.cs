using System.Collections.ObjectModel;
using CalculateurAge.Services;

namespace CalculateurAge.ViewModels;

public class CalculateurViewModel : BaseViewModel
{
    // Contient l'ÉTAT de l'écran et les ACTIONS possibles.
    // Champs privés : la vraie donnée.
    private string _nom = "";
    private DateTime _dateNaissance = DateTime.Today.AddYears(-20);
    private string _resultat = "";
    private string _message = "";
    private string _infoAnniversaire = "";
    private bool _resultatVisible;

    private readonly ObservableCollection<string> _historique = new();
    private readonly INavigationService _navigationService;

    // Propriétés publiques : ce que le XAML voit.
    public string Nom
    {
        get => _nom;
        set
        {
            if (SetField(ref _nom, value))
                CalculerCommand.Rafraichir();
        }
    }

    public DateTime DateNaissance
    {
        get => _dateNaissance;
        set
        {
            if (SetField(ref _dateNaissance, value))
            {
                // Rafraîchir le bouton si la date change
                CalculerCommand.Rafraichir();
            }
        }
    }

    public string Resultat
    {
        get => _resultat;
        set => SetField(ref _resultat, value);
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

    public bool ResultatVisible
    {
        get => _resultatVisible;
        set => SetField(ref _resultatVisible, value);
    }

    // Historique des calculs
    public ObservableCollection<string> Historique => _historique;

    // Commandes liées aux boutons
    public RelayCommand CalculerCommand { get; }
    public RelayCommand EffacerCommand { get; }
    public RelayCommand VoirResultatCommand { get; }

    public CalculateurViewModel()
    {
        _navigationService = MauiProgram.ServiceProvider.GetService<INavigationService>();

        CalculerCommand = new RelayCommand(
            Calculer,
            () => !string.IsNullOrWhiteSpace(Nom) && DateNaissance.Date <= DateTime.Today);

        EffacerCommand = new RelayCommand(Effacer);
        VoirResultatCommand = new RelayCommand(VoirResultat);
    }

    // La logique métier : aucun contrôle d'interface ici.
    private void Calculer()
    {
        int age = DateTime.Today.Year - DateNaissance.Year;
        if (DateNaissance.Date >
            DateTime.Today.AddYears(-age)) age--;

        // Fonctionnalité 1: Message Majeur/Mineur
        Message = age >= 18 ? "Majeur" : "Mineur";

        Resultat = $"{Nom}, vous avez {age} ans";

        // Fonctionnalité 3: Jours jusqu'au prochain anniversaire
        InfoAnniversaire = TexteAnniversaire(age);

        ResultatVisible = true;

        // Fonctionnalité 4: Historique des calculs
        string entreeHistorique = $"{Nom}: {age} ans ({DateNaissance:dd/MM/yyyy})";
        Historique.Insert(0, entreeHistorique);
    }

    // Fonctionnalité 2: Effacer
    private void Effacer()
    {
        Nom = "";
        DateNaissance = DateTime.Today.AddYears(-20);
        Resultat = "";
        Message = "";
        InfoAnniversaire = "";
        ResultatVisible = false;
    }

    // Fonctionnalité 5: Navigation vers ResultatPage
    private async void VoirResultat()
    {
        if (!ResultatVisible) return;

        var parameters = new Dictionary<string, object>
        {
            { "nom", Nom },
            { "age", DateTime.Today.Year - DateNaissance.Year - (DateNaissance.Date > DateTime.Today.AddYears(-(DateTime.Today.Year - DateNaissance.Year)) ? 1 : 0) },
            { "message", Message },
            { "info", InfoAnniversaire }
        };

        await _navigationService.GoToAsync(Routes.ResultatPageRoute, parameters);
    }

    // Calcul des jours restants avant le prochain anniversaire
    private string TexteAnniversaire(int age)
    {
        DateTime anniversaireActuel = DateNaissance.AddYears(age);

        if (anniversaireActuel == DateTime.Today)
        {
            return "Joyeux anniversaire !";
        }

        DateTime prochainAnniversaire = DateNaissance.AddYears(age + 1);
        int joursRestants = (prochainAnniversaire - DateTime.Today).Days;

        return joursRestants == 1 
            ? "Prochain anniversaire dans 1 jour" 
            : $"Prochain anniversaire dans {joursRestants} jour(s)";
    }
}