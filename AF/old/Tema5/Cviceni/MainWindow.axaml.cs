using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Avalonia.FormApp
{
    public class Student
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public Student(string name, string surname)
        {
            Name = name;
            Surname = surname;
        }
    }
    public partial class MainWindow : Window
    {
        public ObservableCollection<Student> Students { get; } = new()
        {
            new Student("Denis", "Smith"),
            new Student("Paul", "Walker"),
            new Student("Lara", "Croft")
        };
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Button_OnClick(object? sender, RoutedEventArgs e)
        {
            string name = textBoxName.Text;
            string surname = textBoxSurname.Text;

            if (string.IsNullOrWhiteSpace(name)) return;
            if (string.IsNullOrWhiteSpace(surname)) return;
            
            Students.Add(new Student(name, surname));
        }
        
        private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is Student selected)
            {
                textBlockSelected.Text = $"Selected: {selected.Name} {selected.Surname}";
            }
        }
    }
}
