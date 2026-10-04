using CalculateurAge.ViewModels;

namespace CalculateurAge.Views;

public partial class ResultatPage : ContentPage, IQueryAttributable
{
    public ResultatPage()
    {
        InitializeComponent();
        BindingContext = new ResultatViewModel();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query != null && BindingContext is ResultatViewModel vm)
        {
            vm.Nom = query.ContainsKey("nom") ? query["nom"].ToString() : "";

            if (query.ContainsKey("age") && int.TryParse(query["age"].ToString(), out int age))
                vm.Age = age;

            if (query.ContainsKey("message"))
                vm.Message = query["message"].ToString() ?? "";

            if (query.ContainsKey("info"))
                vm.InfoAnniversaire = query["info"].ToString() ?? "";
        }
    }
}