using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.IO;
using System.Xml.Linq;

namespace project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int Id { get; set; }
        private string DBPath = "users.db";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CheckUser(object sender, RoutedEventArgs e)
        {
            string userName = userNameInput.Text;
            string userPassword = userPasswordInput.Text;

            if (userName == null || userPassword == null) { return; }

            if (!File.Exists(DBPath))
            {
                this.CreateDB();
            }
            using (var DbConnection = new SQLiteConnection($"Data Source={DBPath};Version=3"))
            {
                DbConnection.Open();

                string userSearchString = "SELECT Id FROM Users WHERE Username=@userNameToCheck";

                using (var command = new SQLiteCommand(userSearchString, DbConnection))
                {
                    command.Parameters.AddWithValue("@userNameToCheck", userName);

                    using (var reader = command.ExecuteReader()) {
                        if (reader.Read())
                        {
                            this.Id = reader.GetInt32(0);
                        }
                        else
                        {
                            string addString = "INSERT INTO Users(Username, PasswordHash, Email) VALUES(@name, @pass)";
                            using (var commandToAddString = new SQLiteCommand(addString, DbConnection))
                            {
                                commandToAddString.Parameters.AddWithValue("@name", userName);
                                commandToAddString.Parameters.AddWithValue("@pass", userPassword);
                                try 
                                {
                                    commandToAddString.ExecuteNonQuery();
                                }
                                catch 
                                {
                                    Console.WriteLine("Error");
                                }
                            }
                        }

                    }
                }
            }
            ProfilePage profilePage = new ProfilePage(this.Id);

            MainFrame.Navigate(profilePage);
        }

        private void CreateDB()
        {
            using (var db = new SQLiteConnection($"Data Source={DBPath};Version=3"))
            {
                db.Open();

                string createTable = @"
                CREATE TABLE Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL UNIQUE,
                    PasswordHash TEXT NOT NULL
                );";

                using (var command = new SQLiteCommand(createTable, db))
                {
                    command.ExecuteNonQuery();
                }

            }
        }
    }
}
