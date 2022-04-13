using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    static class Program
    {
        static Task<string> GetStringSimulationAsync()
        {
            return Task.Run(() =>
            {
                System.Threading.Thread.Sleep(4000);
                return $"Ahoj, ted je {DateTime.Now}";
            });
        }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form form = new Form() { Text = "Muj formular" };

            FlowLayoutPanel panel = new FlowLayoutPanel() { AutoSize = true };
            form.Controls.Add(panel);

            Label label = new Label() { Text = "...", AutoSize = true };
            Button button = new Button() { Text = "Nacti vtip" };

            button.Click += (sender, e) =>
            {
                Task<string> task = GetStringSimulationAsync();
                string text = task.Result;

                label.Text = text;
            };

            panel.Controls.Add(button);
            panel.Controls.Add(label);

            Application.Run(form);
        }
    }
}
