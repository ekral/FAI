using CommunityToolkit.Mvvm.ComponentModel;

namespace Studenti.AvaloniaClient.ViewModels
{
    public partial class Student : ObservableObject
    {
        public int StudentId { get; set; }

        [ObservableProperty]
        private string jmeno;

        public required bool Studuje { get; set; }
    }
}
