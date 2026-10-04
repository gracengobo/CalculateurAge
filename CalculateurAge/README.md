# 📱 CalculateurAge - Calculateur d'Âge MAUI

Une application **MAUI .NET 10** qui calcule votre âge en années complètes, avec 5 fonctionnalités avancées implémentées en **MVVM pur**.

## 📋 Présentation générale

Cette application démontre les concepts progressifs de développement .NET MAUI :

1. **Phase A** : Code-behind simple (Direct manipulation des contrôles)
2. **Phase B** : Navigation multi-page avec paramètres URL
3. **Phase C** : Refactorisation complète en architecture MVVM
4. **Activité 6** : 5 fonctionnalités supplémentaires en MVVM

---

## 🎯 Les 5 Fonctionnalités Principales

### 1️⃣ **Message Majeur / Mineur**
- Affiche automatiquement le statut de l'utilisateur après le calcul
- **Majeur** si âge ≥ 18 ans, **Mineur** sinon
- Affiché en **gras** et **vert** dans l'interface

### 2️⃣ **Commande Effacer**
- Bouton dédié qui remet tous les champs à zéro
- Propriété `EffacerCommand` : `RelayCommand`
- Réinitialise : nom, date, messages et historique

### 3️⃣ **Jours Restants avant Anniversaire**
- Calcule automatiquement le nombre de jours jusqu'au prochain anniversaire
- Affiche : `"Prochain anniversaire dans X jour(s)"` ou `"Joyeux anniversaire !"`
- Fonctionne même si la date d'anniversaire est passée dans l'année

### 4️⃣ **Historique des Calculs**
- `ObservableCollection<string>` : mise à jour temps réel (sans code supplémentaire)
- Chaque calcul s'ajoute en haut (LIFO)
- Format : `"Nom: age ans (dd/MM/yyyy)"`
- Affiché en `CollectionView`

### 5️⃣ **Navigation par ViewModel**
- Service d'abstraction : `INavigationService`
- Implémentation : `ShellNavigationService` (appelle `Shell.Current.GoToAsync`)
- `ResultatPage` implémente `IQueryAttributable`
- Paramètres passés par **objet**, pas par URL texte
- ✅ **Respect du MVVM** : zéro logique interface dans le ViewModel

---

## 🏗️ Architecture MVVM

### Pattern utilisé

```
View (XAML)
   ↓
{Binding} ← BindingContext
   ↓
ViewModel
   ↓
Model / Services
```

### Composants clés

#### **BaseViewModel** (Classe mère)
```csharp
public class BaseViewModel : INotifyPropertyChanged
{
	protected void OnPropertyChanged([CallerMemberName] string nom = null)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nom));

	protected bool SetField<T>(ref T champ, T valeur,
		[CallerMemberName] string nom = null) { ... }
}
```
- Implémente `INotifyPropertyChanged`
- `SetField()` : affecte + notifie automatiquement
- `[CallerMemberName]` : zéro risque de faute de frappe

#### **RelayCommand** (Événement → Action)
```csharp
public class RelayCommand : ICommand
{
	public bool CanExecute(object p) => _peutExecuter?.Invoke() ?? true;
	public void Execute(object p) => _executer();
}
```
- Transforme une méthode en commande`
- Le bouton se grise tout seul selon `CanExecute()`

#### **Services de Navigation**
```csharp
public interface INavigationService
{
	Task GoToAsync(string route, IDictionary<string, object> parameters);
	Task GoBackAsync();
}
```
- Découple le ViewModel de `Shell`
- Testable et réutilisable

#### **CalculateurViewModel**
- ✅ Aucun `Label`, `Entry`, `Button` ou `DisplayAlert`
- ✅ Propriétés : `Nom`, `DateNaissance`, `Resultat`, `Message`, `InfoAnniversaire`
- ✅ Commandes : `CalculerCommand`, `EffacerCommand`, `VoirResultatCommand`
- ✅ Données : `ObservableCollection<string> Historique`

---

## 📁 Structure du Projet

```
CalculateurAge/
├── ViewModels/
│   ├── BaseViewModel.cs           # Classe mère (INotifyPropertyChanged)
│   ├── CalculateurViewModel.cs    # ViewModel de l'écran principal (+5 fonctionnalités)
│   ├── ResultatViewModel.cs       # ViewModel de la page résultat
│   └── RelayCommand.cs            # Implémentation ICommand
├── Services/
│   ├── INavigationService.cs      # Interface de navigation
│   ├── ShellNavigationService.cs  # Implémentation Shell
│   └── Routes.cs                  # Constantes des routes
├── Views/
│   ├── MainPage.xaml              # Écran principal (5 fonctionnalités)
│   ├── MainPage.xaml.cs           # Code-behind minimal (juste BindingContext)
│   ├── ResultatPage.xaml          # Page résultat détaillée
│   └── ResultatPage.xaml.cs       # IQueryAttributable
├── App.xaml(.cs)                  # App shell
├── AppShell.xaml(.cs)             # Routing central
├── MauiProgram.cs                 # DI Container + Service registration
└── Resources/                     # Fonts, Images, Styles
```

---

## 🚀 Comment Utiliser

### Saisie
1. Entrez votre **nom** (champ texte)
2. Sélectionnez votre **date de naissance** (DatePicker)
3. Le bouton `Calculer` s'active automatiquement

### Calcul
- Clic sur **Calculer**
- L'âge est calculé en années complètes (accounting pour l'anniversaire non encore passé)
- Affichage :
  - **Âge** : "Nom, vous avez X ans"
  - **Statut** : "Majeur" ou "Mineur" en **gras vert**
  - **Anniversaire** : "Prochain anniversaire dans X jour(s)" ou "Joyeux anniversaire !"

### Actions
- **Effacer** : Réinitialise tous les champs
- **Voir le résultat** : Ouvre une page détaillée (pop-up résumé)
- **Historique** : Liste des derniers calculs (auto-scroll)

### Navigation
- La page `ResultatPage` affiche les détails
- Bouton `Retour` ou geste back pour revenir

---

## 🎓 Concepts d'Enseignement

### Pourquoi MVVM?

| Aspect | Code-Behind | MVVM |
|--------|------------|------|
| Logique | Mélangée à l'UI | Isolée dans VM |
| Testabilité | Difficile | Facile (pas d'UI) |
| Réutilisabilité | Faible | Haute |
| Maintenance | Chaotique | Organisée |
| Binding | Pas de binding | Binding complet |

### Trajet d'une donnée en MVVM

```
1. Utilisateur tape "Marie" dans Entry
   ↓
2. Binding TwoWay écrit dans ViewModel.Nom
   ↓
3. Setter appelle SetField() → OnPropertyChanged("Nom")
   ↓
4. PropertyChanged event déclenché
   ↓
5. Tous les contrôles liés à Nom se rafraîchissent
   ↓
6. L'écran se met à jour automatiquement
```

### Les trois modes de binding

```csharp
{Binding Propriete}              // OneWay (défaut Label)
{Binding Propriete, Mode=TwoWay} // Bidirectionnel (défaut Entry)
{Binding Propriete, Mode=OneTime} // Lu une seule fois
```

---

## 🛠️ Historique Git (4 Phases)

```bash
git log --oneline
```

```
296ae0f - Activité 6: Ajout des 5 fonctionnalités supplémentaires
762fdf1 - Phase C: Réécriture complète en MVVM
181229b - Phase B: Ajout de la deuxième page et navigation
686d406 - Phase A: Version code-behind avec calcul d'âge
```

Chaque commit représente une étape pédagogique claire.

---

## 📚 Références

- **Microsoft MAUI Docs** : https://learn.microsoft.com/en-us/dotnet/maui/
- **MVVM Pattern** : https://learn.microsoft.com/en-us/dotnet/architecture/mvvm/
- **.NET 10** : https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10

---

## 📝 Notes Importantes

### ✅ Respect de la Règle MVVM

La règle d'or du TP : **Si un ViewModel contient les mots `Label`, `Entry`, `Button` ou `DisplayAlert`, ce n'est pas du MVVM.**

Vérification dans `CalculateurViewModel.cs` :
- ❌ Zéro référence à des contrôles
- ✅ Que des propriétés et des commandes
- ✅ La logique métier est 100% testable

### 🎬 Vidéo de Démonstration

Pour recorder une vidéo (30 sec max) :
1. Lancez l'app
2. Montrez le calcul simple
3. Montrez "Majeur/Mineur"
4. Montrez "Effacer"
5. Montrez l'historique
6. Montrez "Voir le résultat" (navigation)

---

## 👨‍💻 Auteur

**Grace Ngobo** (gracengobo)
- Email : kwuidagrace@gmail.com
- Repo : https://github.com/gracengobo/CalculateurAge

---

## 📄 Licence

Projet pédagogique - Libre d'utilisation

