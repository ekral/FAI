using CommunityToolkit.Mvvm.ComponentModel;

namespace Students.AvaloniaApp.DTOs
{
    public partial class Student : ObservableObject
    {
        public int StudentId { get; set; }
        [ObservableProperty]
        public string jmeno;
        public required bool Studuje { get; set; }
    }
}
