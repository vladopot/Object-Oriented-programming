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
using System.IO.Packaging;
using System.Runtime.Remoting.Messaging;

namespace project
{
    public partial class MainWindow : Window
    {
        private int Id { get; set; }
        private string DBPath = "Users.db";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CheckUser(object sender, RoutedEventArgs e)
        {
            string userName = userNameInput.Text;
            string userPassword = userPasswordInput.Text;

            if (String.IsNullOrWhiteSpace(userName) || String.IsNullOrWhiteSpace(userPassword)) { return; }

            if (!File.Exists(DBPath))
            {
                this.CreateDB();
            }
            string fullPath = System.IO.Path.GetFullPath(DBPath);
            Console.WriteLine(fullPath);
            bool userChecked = checkPass(userName, userPassword);

            if (userChecked)
            {
                ProfilePage profilePage = new ProfilePage(this.Id);

                MainFrame.Navigate(profilePage);
            }
            else
            {
                MessageBox.Show("Invalid password");
            }

        }

        private bool checkPass(string userName, string userPassword)
        {
            using (var DbConnection = new SQLiteConnection($"Data Source={DBPath}"))
            {
                DbConnection.Open();

                string userSearchString = "SELECT Id, PasswordHash FROM Users WHERE Username = @userNameToCheck";

                using (var command = new SQLiteCommand(userSearchString, DbConnection))
                {
                    command.Parameters.AddWithValue("@userNameToCheck", userName);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader.GetString(1) == userPassword)
                            {
                                this.Id = reader.GetInt32(0);
                                return true;
                            }
                            return false;
                        }
                        else
                        {
                            string addString = "INSERT INTO Users(Username, PasswordHash) VALUES(@name, @pass)";
                            using (var commandToAddString = new SQLiteCommand(addString, DbConnection))
                            {
                                commandToAddString.Parameters.AddWithValue("@name", userName);
                                commandToAddString.Parameters.AddWithValue("@pass", userPassword);
                                try
                                {
                                    commandToAddString.ExecuteNonQuery();
                                    
                                    SQLiteCommand sQLiteCommand = new SQLiteCommand("SELECT last_insert_rowid()", DbConnection);

                                    this.Id = Convert.ToInt32(sQLiteCommand.ExecuteScalar());

                                    return true;
                                }
                                catch
                                {
                                    Console.WriteLine("Error");
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CreateDB()
        {
            using (var db = new SQLiteConnection($"Data Source={DBPath}"))
            {
                db.Open();

                string checkTableExists = "SELECT name FROM sqlite_master WHERE type='table' AND name='Users';";
                using (var command = new SQLiteCommand(checkTableExists, db))
                {
                    var result = command.ExecuteScalar();
                    if (result == null)
                    {
                        string createTable = "CREATE TABLE IF NOT EXISTS Users (Id INTEGER PRIMARY KEY AUTOINCREMENT,Username TEXT NOT NULL UNIQUE,PasswordHash TEXT NOT NULL);";
                        using (var createCommand = new SQLiteCommand(createTable, db))
                        {
                            createCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

    }
}
