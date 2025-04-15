namespace Students.AvaloniaApp.ViewModels
{
    public interface IExportable
    {
        string? Json { get; }

        void ExportToJson();
    }
}