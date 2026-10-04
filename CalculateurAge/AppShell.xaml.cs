using CalculateurAge.Views;
using CalculateurAge.Services;

namespace CalculateurAge
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            // Déclare la route avec le même nom que Routes.ResultatPageRoute
            // Sans cette ligne, GoToAsync lève une exception "route inconnue".
            Routing.RegisterRoute(Routes.ResultatPageRoute, typeof(ResultatPage));
        }
    }
}
