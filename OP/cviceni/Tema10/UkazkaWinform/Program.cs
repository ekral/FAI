using System;
using System.Net.Http;
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

        static async Task<string> GetVtipAsync()
        {
            using(HttpClient client = new HttpClient())
            {
                string text = await client.GetStringAsync("https://geek-jokes.sameerkumar.website/api?format=plain");
                return text;
            }
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

            //button.Click += (sender, e) =>
            //{
            //    Task<string> task = GetStringSimulationAsync();

            //    label.Text = task.Result;
            //};

            //button.Click += async (sender, e) =>
            //{
            //    string text = await GetStringSimulationAsync();

            //    label.Text = text;
            //};

            button.Click += async (sender, e) =>
            {
                string text = await GetVtipAsync();

                label.Text = text;
            };

            panel.Controls.Add(button);
            panel.Controls.Add(label);

            Application.Run(form);
        }
    }
}
