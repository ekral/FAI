using CommunityToolkit.Mvvm.ComponentModel;

namespace Students.AvaloniaApp.ViewModels
{

    public partial class StudentViewModel : ObservableObject
    {
        public int StudentId { get; set; }

        [ObservableProperty]
        public string jmeno;

        public required bool Studuje { get; set; }
    }
}

