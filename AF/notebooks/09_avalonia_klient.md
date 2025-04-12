# Materiál 09: Avalonia dektopový klient

**autor: Erik Král ekral@utb.cz**

---

## Základní pojmy MVVM

- **View** představuje to co vidí uživatel na obrazovce. Obsahuje elementy uživatelského rozhraní jako například ```TextBlock``` nebo ```Button```.
- **ViewModel** obsahuje property, Commandy (zatím jsme neprobrali) a metody na které binduje View. Pokud se ve ViewModelu změní hodnota nějaké property, tak o této změně ViewModel informuje informuje View pomocí eventu ```PropertyChanged```. ViewModel představuje prostředníka mezi View a Modelem, připravuje data pro zobrazení a reaguje na akce uživatele, ale nepoužívá elementy uživatelského rozhraní.
- **Model** představuje aplikační logiku aplikace. Například složitý výpočet parametrů životního pojištění. Model pracuje z hlediska logiky aplikace a vůbec s nezajímá o uživatelské rozhraní.
- Event **PropertyChanged** používá ViewModel k tomu aby infomoval (notifikoval) View o změnách ve svých propertách. Je součástí rozhraní ```INotifyPropertyChanged```.

Použití MVVM je ve frameworku Avalonia, ale i dalších dobrovolné a nemusíme jej používat. Cílem je mít modulární návrh aplikace, tak abychom mohli jednoduše měnit ViewModel například za testovací verzi a také vyvíjet uživatelské rozhran nezávisle na vývoji ViewModelu.

## MVVM Community Toolkit

V následujícím příkladu budeme používat [MVVM Community Toolkit](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/) který přidáme jako následující nuget balíček:

```
CommunityToolkit.Mvvm
```

## Avalonia 

[Avalonia](https://avaloniaui.net/) je multiplatformní framework pro tvorbu především desktopových aplikací. Je jedním z mnoha frameworků využívající jazyk XAML pro popis uživatelského rozhraní a pattern MVVM.

Dalšími frameworky jsou:

- [WPF](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/?view=netdesktop-9.0)
- [WinUI](https://learn.microsoft.com/en-us/windows/apps/winui/winui3/) 
- [NoesisGUI](https://www.noesisengine.com/)
- [Uno platform](https://platform.uno)

## Desktopový klient pro studenty

Vytvoříme jednoduché uživatelské rozhraní, které v ComboBoxu zobrazí jména všech studentů a od ComboBoxem zobrazí TextBox umožňující editovat jméno studenta, CheckBox editující zda student studuje a tlačítko Save pro uložení změn.

Nejprve si nadefinujeme ViewModel. Atribut `[ObservableProperty]` z MVVM Toolkitu způsobí vygenerování property, například pro field `students` vygeneruje následující property, která pomocí volání metody `SetProperty` vyvolá event `PropertyChanged` pro tuto propertu. `MainViewModel` dědí od ViewModelBase, který dědí od třídy `ObservableObject`.

```c#
public string? Students
{
    get => students;
    set => SetProperty(ref students, value);
}
``

```csharp
public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private StudentViewModel[]? students;

    [ObservableProperty]
    private StudentViewModel? selectedStudent;

    public string Greeting => "Welcome to Avalonia!";


    public MainViewModel()
    {
        Task.Run(LoadStudentAsync);
    }
    private async Task LoadStudentAsync()
    {
        Students = await App.sharedClient.GetFromJsonAsync<StudentViewModel[]>("/students");
        SelectedStudent = Students?.First();
    }

    public async Task Save()
    {
        if (SelectedStudent is not null)
        {
            await App.sharedClient.PutAsJsonAsync($"/students/{SelectedStudent.StudentId}", SelectedStudent);
        }
    }
}
```

StudentViewModel vypadá následovně:

```csharp
public partial class StudentViewModel : ObservableObject
{
    public int StudentId { get; set; }

    [ObservableProperty]
    public string jmeno;

    public required bool Studuje { get; set; }
}
```

A poté View:

```xaml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             xmlns:vm="clr-namespace:Students.AvaloniaApp.ViewModels"
             mc:Ignorable="d" d:DesignWidth="800" d:DesignHeight="450"
             x:Class="Students.AvaloniaApp.Views.MainView"
             x:DataType="vm:MainViewModel">
  <Design.DataContext>
    <vm:MainViewModel />
  </Design.DataContext>
	<StackPanel>
        <TextBlock Text="{Binding Greeting}" HorizontalAlignment="Center" />
		<ComboBox ItemsSource="{Binding Students}" SelectedItem="{Binding SelectedStudent}" HorizontalAlignment="Center">
			<ComboBox.ItemTemplate>
				<DataTemplate>
					<TextBlock Text="{Binding Jmeno}"/>
				</DataTemplate>
			</ComboBox.ItemTemplate>
		</ComboBox>
		<TextBlock Text="{Binding SelectedStudent.StudentId}" HorizontalAlignment="Center"/>
		<TextBox Text="{Binding SelectedStudent.Jmeno}" HorizontalAlignment="Center"/>
		<CheckBox IsChecked="{Binding SelectedStudent.Studuje}" HorizontalAlignment="Center"/>
		<Button Command="{Binding Save}" HorizontalAlignment="Center">Save</Button>			
	</StackPanel>  
</UserControl>
```
