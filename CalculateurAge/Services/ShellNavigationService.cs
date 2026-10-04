using CalculateurAge.Views;

namespace CalculateurAge.Services;

public class ShellNavigationService : INavigationService
{
    public async Task GoToAsync(string route, IDictionary<string, object> parameters)
    {
        // Construire l'URL avec les paramètres
        string queryString = BuildRoute(route, parameters);
        await Shell.Current.GoToAsync(queryString);
    }

    public async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    private static string BuildRoute(string route, IDictionary<string, object> parameters)
    {
        if (parameters == null || parameters.Count == 0)
            return route;

        var queryParams = string.Join("&", 
            parameters.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value?.ToString() ?? "")}"));

        return $"{route}?{queryParams}";
    }
}