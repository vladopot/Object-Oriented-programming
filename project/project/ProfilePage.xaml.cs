using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
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
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace project
{
    /// <summary>
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        private int userId {  get; set; }
        private string DBPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Users.db");

        public event PropertyChangedEventHandler PropertyChanged;

        private string Login;

        public ProfilePage(int userId)
        {
            this.userId = userId;
            InitializeComponent();
            LoadProfile();
        }

        private void NavigateTo(object Sender, RoutedEventArgs e)
        {
            Button clickedBtn = Sender as Button;
            string btnName = clickedBtn.Name;
            switch (btnName)
            {
                case "plansBtn":
                    Plans PlansPage = new Plans(this.userId);
                    NavigationService.Navigate(PlansPage);
                    break;
                case "HistoryBtn":
                    HistoryPage historyPage = new HistoryPage(this.userId);
                    NavigationService.Navigate(historyPage);
                    break;
                case "AnalyzBtn":
                    AnalizPage analizPage = new AnalizPage(this.userId);
                    NavigationService.Navigate(analizPage);
                    break;
                case "exitBtn":
                    NavigationService.GoBack();
                    break;
            }   
        }

        private void LoadProfile()
        {
            this.CreateDB();
            using (var db = new SQLiteConnection($"Data Source={DBPath}"))
            {
                db.Open();

                string userSearchString = "SELECT Username FROM Users WHERE Id = @userId";
                using (var command = new SQLiteCommand(userSearchString, db))
                {
                    command.Parameters.AddWithValue("@userId", this.userId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string username = reader.GetString(0);
                            this.Login = username;
                            TextBlock.Text = this.Login;
                        }
                    }
                }

                string userInfoSearchString = "SELECT FirstName, LastName, HeightMM, WeightKG, Age, Goal FROM UserInformation WHERE UserId = @userId";
                using (var command = new SQLiteCommand(userInfoSearchString, db))
                {
                    command.Parameters.AddWithValue("@userId", this.userId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine(this.userId);
                            string firstName = reader.GetString(0);
                            Console.WriteLine(firstName);
                            string lastName = reader.GetString(1);
                            int height = reader.GetInt32(2);
                            double weight = reader.GetDouble(3);
                            int age = reader.GetInt32(4);
                            string goal = reader.GetString(5);
                            firstNameInput.Text = firstName;
                            lastNameInput.Text = lastName;
                            heightInput.Text = height.ToString();
                            weightInput.Text = weight.ToString();
                            ageInput.Text = age.ToString();
                            goalInput.Text = goal;
                        }
                    }
                }
            }
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(DBPath))
            {
                this.CreateDB();
            }

            using (var db = new SQLiteConnection($"Data Source={DBPath}"))
            {
                db.Open();

                int height = string.IsNullOrWhiteSpace(heightInput.Text) || !int.TryParse(heightInput.Text, out height) ? 0 : height;
                double weight = string.IsNullOrWhiteSpace(weightInput.Text) || !double.TryParse(weightInput.Text, out weight) ? 0 : weight;
                int age = string.IsNullOrWhiteSpace(ageInput.Text) || !int.TryParse(ageInput.Text, out age) ? 0 : age;

                string checkString = "SELECT * FROM UserInformation WHERE UserId = @userId";

                using (var checkCommand = new SQLiteCommand(checkString, db))
                {
                    checkCommand.Parameters.AddWithValue("@userId", this.userId);
                    using (var reader = checkCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string updateString = @"UPDATE UserInformation
                                        SET FirstName = @firstName,
                                            LastName = @lastName,
                                            HeightMM = @height,
                                            WeightKG = @weight,
                                            Age = @age,
                                            Goal = @goal
                                        WHERE UserId = @userId;";
                            using (var command = new SQLiteCommand(updateString, db))
                            {
                                command.Parameters.AddWithValue("@firstName", firstNameInput.Text);
                                command.Parameters.AddWithValue("@lastName", lastNameInput.Text);
                                command.Parameters.AddWithValue("@height", height);
                                command.Parameters.AddWithValue("@weight", weight);
                                command.Parameters.AddWithValue("@age", age);
                                command.Parameters.AddWithValue("@goal", goalInput.Text);
                                command.Parameters.AddWithValue("@userId", this.userId);
                                int rowsAffected = command.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Record updated successfully.");
                                }
                                else
                                {
                                    MessageBox.Show("No changes made.");
                                }
                            }
                        }
                        else
                        {
                            string insertQuery = @"
                                                    INSERT INTO UserInformation (UserId, FirstName, LastName, HeightMM, WeightKG, Age, Goal)
                                                    SELECT @userId, @firstName, @lastName, @height, @weight, @age, @goal
                                                    WHERE NOT EXISTS (
                                                        SELECT 1 FROM UserInformation WHERE UserId = @userId
    );
";
                            using (var command = new SQLiteCommand(insertQuery, db))
                            {
                                command.Parameters.AddWithValue("@userId", this.userId);
                                command.Parameters.AddWithValue("@firstName", firstNameInput.Text);
                                command.Parameters.AddWithValue("@lastName", lastNameInput.Text);
                                command.Parameters.AddWithValue("@height", height);
                                command.Parameters.AddWithValue("@weight", weight);
                                command.Parameters.AddWithValue("@age", age);
                                command.Parameters.AddWithValue("@goal", goalInput.Text);
                                int rowsAffected = command.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Record updated successfully.");
                                }
                                else
                                {
                                    MessageBox.Show("No changes made.");
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

                string createUserInfoTable = @"
                    CREATE TABLE IF NOT EXISTS UserInformation (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserId INTEGER NOT NULL,
                        FirstName TEXT NOT NULL,
                        LastName TEXT NOT NULL,
                        HeightMM INTEGER NOT NULL,
                        WeightKG REAL NOT NULL,
                        Age INTEGER NOT NULL,
                        Goal TEXT NOT NULL,
                        FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
                    );";
                using (var createUserInfoCommand = new SQLiteCommand(createUserInfoTable, db))
                {
                    createUserInfoCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
