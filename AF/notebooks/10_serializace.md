# Materiál 10: Serializace

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

Nejprve si definujme rozhraní a service pro zobrazení dialogu:

```csharp
public interface ISaveDialogService
{
    Task<IStorageFile?> ShowAsync();
}
```


```csharp
public class SaveDialogService(TopLevel topLevel) : ISaveDialogService
{
    private readonly TopLevel topLevel = topLevel;

    public Task<IStorageFile?> ShowAsync()
    {
        var jsonFileType = new FilePickerFileType("JSON File")
        {
            Patterns = ["*.json"],
            MimeTypes = ["application/json"]
        };

        Task<IStorageFile?> file = topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save export file",
            DefaultExtension = "json",
            FileTypeChoices = [jsonFileType]
        });

        return file;
    }
}
```

A potom si nadefinujeme rozhraní a service pro ukládání řetězce do souboru.

```csharp
public interface IFileService
{
    Task SaveAsync(IStorageFile file, string json);
}
```


```csharp
public class FileService : IFileService
{
    public async Task SaveAsync(IStorageFile file, string json)
    {
        await using var stream = await file.OpenWriteAsync();

        using var streamWriter = new StreamWriter(stream);

        await streamWriter.WriteAsync(json);
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
    private StudentViewModel[]? students;

    [ObservableProperty]
    private StudentViewModel? selectedStudent;

    private readonly IStudentService studentService;
    private readonly ISaveDialogService saveDialog;
    private readonly IFileService fileService;

    public MainViewModel(IStudentService studentService, ISaveDialogService saveDialog, IFileService fileService)
    {
        Task.Run(LoadStudentAsync);
        this.studentService = studentService;
        this.saveDialog = saveDialog;
        this.fileService = fileService;
    }

    private async Task LoadStudentAsync()
    {
        Students = await studentService.GetAllStudentsAsync();

        SelectedStudent = Students?.First();
    }

    public async Task Save()
    {
        if (SelectedStudent is not null)
        {
            await studentService.UpdateStudentAsync(SelectedStudent);
        }
    }

    public async Task ExportToJson()
    {
        IStorageFile? storageFile = await saveDialog.ShowAsync();

        if (storageFile is not null)
        {
            string json = JsonSerializer.Serialize(Students);

            await fileService.SaveAsync(storageFile, json);
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
        BaseAddress = new Uri("https://localhost:7042")
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

        StudentService studentService = new StudentService(sharedClient);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
            
            TopLevel topLevel = TopLevel.GetTopLevel(desktop.MainWindow) ?? throw new NullReferenceException();

            desktop.MainWindow.DataContext = new MainViewModel(studentService, new SaveDialogService(topLevel), new FileService());
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView();

            TopLevel topLevel = TopLevel.GetTopLevel(singleViewPlatform.MainView) ?? throw new NullReferenceException();

            singleViewPlatform.MainView.DataContext = new MainViewModel(studentService, new SaveDialogService(topLevel), new FileService());
        }

        base.OnFrameworkInitializationCompleted();
    }
}
```
