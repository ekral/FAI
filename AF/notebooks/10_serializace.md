# Materiál 10: Avalonia serializace a ukládání do souboru

**autor: Erik Král ekral@utb.cz**

---

## Serializace a deserializace

Dokumentace: [Serialization in .NET](https://learn.microsoft.com/en-us/dotnet/standard/serialization/)

Objekty v paměti nejsou kompatibilní mezi hardwarovými platformami (AMR, x86, x64) a už vůbec mezi různými programovacími jazyky. Serializace je proces převodu objektu v paměti na jiný formát kompatibilní mezi různými platformami. Například text ve formátu JSON, který můžeme uložit do souboru nebo přenést po síti. 

Máme několik možností:

- člověkem čitelný textový formát, nejběžnější je JSON používaný pro REST webové služby založené na HTTP protokolu. Dříve se používal XML formát pro SOAP služby.
- binární formát pro vysoký výkon. Příkladem je protokol gRPC.

Dotnet obsahuje mimo jiné zabudovanou podporu pro serializaci a deserializaci do formátu JSON. Není nutné vkládat žádný nuget balíček.

Máme následující třídu:

```csharp
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

Následující příklad serializuje studenta.


```csharp
Student student = new Student() { Id = 1, Name = "Mikulas" };
string text = JsonSerializer.Serialize(student);
```

Výsledný řetězec v proměnné text bude:
```json
{
    "Id" : 1,
    "Name" : "Mikulas"
}
```

A následující příklad zase z řetězce deserializuje studenta.

```csharp
string text = "{ \"Id\" : 1, \"Name\" : \"Cert\"  }";
Student? student = JsonSerializer.Deserialize<Student>(text);
```

V C#11 z .NET 7 můžeme použít raw string literals a zadávat dvojité uvozovky přímo v textu řetězce bez escape sekvence.
 
 ```csharp
string text = """{ "Id" : 1, "Name" : "Cert"  }""";
Student? student = JsonSerializer.Deserialize<Student>(text);
```

Pro serializaci a deserialici volíme jednoduché objekty obsahují ideálně pouze zabudované typy jazyka. Těmto objektům říkame Data Transfer Object, napříkad StudentDTO. Pro serializace tedy často definujeme speciální třídy.

## Avalonia File Dialog

V následujícím příkladu si ukážeme jak vytvořit Save File Dialog ve frameworku Avalonia. V příkladu využijeme techniky Dependency Injection. Cílem je abychom mohli ViewModel testovat pomocí unit testů.

Nejprve si definujme rozhraní a service pro zobrazení dialogu a uložení řetězce do souboru:

```csharp
public interface ISaveDialogService
{
    Task SaveAsync(string json);
}
```


```csharp
public class SaveDialogService(TopLevel level) : ISaveDialogService
{
    private readonly TopLevel level = level;

    public async Task SaveAsync(string json)
    {
        var jsonFileType = new FilePickerFileType("JSON File")
        {
            Patterns = ["*.json"],
            MimeTypes = ["application/json"]
        };

        var options = new FilePickerSaveOptions
        {
            Title = "Save export file",
            DefaultExtension = "json",
            FileTypeChoices = [jsonFileType]
        };

        IStorageFile? file = await level.StorageProvider.SaveFilePickerAsync(options);

        if (file is not null)
        {
            await using var stream = await file.OpenWriteAsync();
            using var streamWriter = new StreamWriter(stream);
            await streamWriter.WriteAsync(json);
        }
    }
}
```


Pro to, aby byl VieModel testovatelný, tak si vytvoříme service i pro studenty, i když to nesouvisí se serializací a ukládáním do souboru.

```csharp
public interface IStudentService
{
    Task<StudentViewModel[]?> GetAllStudentsAsync();
    Task UpdateStudentAsync(StudentViewModel student);
}
```


```csharp
public class StudentService(HttpClient http) : IStudentService
{
    private readonly HttpClient http = http;

    public Task<StudentViewModel[]?> GetAllStudentsAsync()
    {
        Task<StudentViewModel[]?> students = http.GetFromJsonAsync<StudentViewModel[]>("/students");

        return students;
    }

    public Task UpdateStudentAsync(StudentViewModel student)
    {
        return http.PutAsJsonAsync($"/students/{student.StudentId}", student);
    }
}
```


ViewModel bude vypadat následovně, kde důležitá je metoda `ExportToJson`.

```csharp
public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private Student[]? students;

    [ObservableProperty]
    private Student? selectedStudent;

    private readonly IStudentService studentService;
    private readonly ISaveDialogService saveDialog;

    public MainViewModel(IStudentService studentService, ISaveDialogService saveDialog)
    {
        this.studentService = studentService;
        this.saveDialog = saveDialog;
    }

    public async Task LoadStudentAsync()
    {
        Students = await studentService.GetAllStudentsAsync(); 

        SelectedStudent = Students?.FirstOrDefault();
    }

    public async Task SaveAsync()
    {
        if (SelectedStudent is not null)
        {
            await studentService.UpdateStudentAsync(SelectedStudent);
        }
    }

    public async Task ExportAsync()
    {
        if (Students is not null)
        {
            string json = JsonSerializer.Serialize(Students);

            await saveDialog.SaveAsync(json);
        }
    }
}
```

A nakonec konfigurace v App.xaml.cs je následující:

```csharp
public partial class App : Application
{
    public static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("https://localhost:7266")
    };

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();

            TopLevel topLevel = TopLevel.GetTopLevel(desktop.MainWindow) ?? throw new NullReferenceException();

            MainViewModel vm = new(new StudentService(sharedClient), new SaveDialogService(topLevel));

            desktop.MainWindow.DataContext = vm;

            desktop.MainWindow.Loaded += async (sender, e) => await vm.LoadStudentAsync();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView();

            TopLevel topLevel = TopLevel.GetTopLevel(singleViewPlatform.MainView) ?? throw new NullReferenceException();

            MainViewModel vm = new(new StudentService(sharedClient), new SaveDialogService(topLevel));

            singleViewPlatform.MainView.DataContext = vm;

            singleViewPlatform.MainView.Loaded += async (sender, e) => await vm.LoadStudentAsync();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
```
