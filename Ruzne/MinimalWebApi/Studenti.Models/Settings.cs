namespace Studenti.Models
{
    using System.IO;
    using Microsoft.Data.Sqlite;

    public static class Settings
    {
        public static string GetConnectionString(string fileName)
        {
            var folder = Environment.SpecialFolder.MyDocuments;
            string folderPath = Environment.GetFolderPath(folder);
            string filePath = Path.Join(folderPath, fileName);

            SqliteConnectionStringBuilder csb = new SqliteConnectionStringBuilder
            {
                DataSource = filePath
            };

            string connectionString = csb.ConnectionString;

            return connectionString;
        }
    }

}
