using Microsoft.AspNetCore.Components;
using Microsoft.Data.Sqlite;

namespace BlazorApp1.Data
{

    public class Interaction
    {
        public static int sessionId { get; set; }
        public static void CreateNewEntryJob(string title, string teamLeader)
        {
            using (var connection = new SqliteConnection($"Data Source=Mars.db"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = connection;

                command.CommandText = $"INSERT INTO jobs (title, teamLeader) VALUES ('{title}', '{teamLeader}')";
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        
    }
    public class ButtonConfig
    {
        public string Text { get; set; }
        public EventCallback OnClick { get; set; }
        public ButtonConfig(string text)
        {
            Text = text;
        }
    }
    public class Job
    {
        public int Id { get; set; } // Уникальный идентификатор работы
        public string Title { get; set; } = string.Empty; // Название работы
        public string TeamLeader { get; set; } = string.Empty; // Руководитель команды
        public int Father { get; set; } // Идентификатор создателя работы (userId)
    }
   
}
