using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
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

namespace project
{
    /// <summary>
    /// Interaction logic for Plans.xaml
    /// </summary>
    public partial class Plans : Page
    {
        private int userId { get; set; }
        private string DBPath = "Users.db";

        private bool addingState = false;
        TextBox BoxFromInPending = null;
        StackPanel StackPanelInPending = null;
        string planName = "";
        public Plans(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            restoreTranings();
        }

        private void restoreTranings()
        {
            using (var db = new SQLiteConnection($"Data Source={DBPath}"))
            {
                db.Open();
                string createUserInfoTable = @"
                    CREATE TABLE IF NOT EXISTS UserTranings (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserId INTEGER NOT NULL,
                        Traning TEXT NOT NULL,
                        TraningTime TEXT,
                        FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
                );";
                using (var comand = new SQLiteCommand(createUserInfoTable, db))
                {
                    comand.ExecuteNonQuery();
                }
                using (var comand = new SQLiteCommand("SELECT * FROM UserTranings WHERE UserId = @userId", db))
                {
                    comand.Parameters.AddWithValue("@userId", this.userId);
                    using (var reader = comand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StackPanel newStackPanel = new StackPanel();
                            newStackPanel.Orientation = Orientation.Horizontal;
                            TextBlock textBlock = new TextBlock();
                            textBlock.Background = Brushes.LightGray;
                            textBlock.FontSize = 20;
                            textBlock.Text = reader["Traning"].ToString();
                            textBlock.Height = 40;
                            textBlock.Width = 310;

                            Button DeleteBtn = new Button();
                            DeleteBtn.Tag = reader["Id"].ToString();
                            DeleteBtn.Width = 65;
                            DeleteBtn.Content = "Delete";
                            DeleteBtn.Click += DeletePlan;

                            newStackPanel.Children.Add(textBlock);
                            TextBlock textBlockFrom = new TextBlock();
                            textBlockFrom.Background = Brushes.LightGray;
                            textBlockFrom.FontSize = 20;
                            textBlockFrom.Width = 140;
                            textBlockFrom.Text = "Tranning date";

                            TextBlock textBoxFrom = new TextBlock();
                            textBoxFrom.Background = Brushes.LightGray;
                            textBoxFrom.FontSize = 20;
                            textBoxFrom.Width = 120;
                            textBoxFrom.Text = reader["TraningTime"].ToString();

                            Button HistoryBtn = new Button();
                            HistoryBtn.Tag = reader["Id"].ToString();
                            HistoryBtn.Width = 65;
                            HistoryBtn.Content = "History";
                            HistoryBtn.Click += AddToHistory;

                            newStackPanel.Children.Add(textBlockFrom);
                            newStackPanel.Children.Add(textBoxFrom);
                            newStackPanel.Children.Add(HistoryBtn);
                            newStackPanel.Children.Add(DeleteBtn);
                            
                            this.StackPanelInPending = newStackPanel;
                            ContentStackPanel.Children.Insert(0, newStackPanel);
                            
                        }
                    }
                }
            }
        }

        private void AddToHistory(object sender, RoutedEventArgs e)
        {
            Button clickedBtn = sender as Button;
            using (var db = new SQLiteConnection($"Data Source={DBPath}"))
            {
                db.Open();
                string createHistoryTable = @"
                    CREATE TABLE IF NOT EXISTS History (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserId INTEGER NOT NULL,
                        Traning TEXT NOT NULL,
                        TraningTime TEXT,
                        FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
                );";
                using (var comand = new SQLiteCommand(createHistoryTable, db))
                {
                    comand.ExecuteNonQuery();
                }
                using (var comand = new SQLiteCommand(@"
                        INSERT INTO History (UserId, Traning, TraningTime)
                        SELECT UserId, Traning, TraningTime
                        FROM UserTranings
                        WHERE Id = @id;", db))
                {
                    comand.Parameters.AddWithValue("id", clickedBtn.Tag);
                    comand.ExecuteNonQuery();
                }
            }
            StackPanel stackPanel = clickedBtn.Parent as StackPanel;
            ContentStackPanel.Children.Remove(stackPanel);
            Console.WriteLine(clickedBtn.Tag);
            using (var db = new SQLiteConnection($"Data Source={DBPath}"))
            {
                db.Open();
                using (var comand = new SQLiteCommand("DELETE FROM UserTranings WHERE Id = @id", db))
                {
                    comand.Parameters.AddWithValue("@id", clickedBtn.Tag);
                    comand.ExecuteNonQuery();
                }
            }
        }

        private void addPlan(object sender, RoutedEventArgs e)
        {
            if (this.addingState)
            {
                return;
            }

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;

            ComboBox comboBox = new ComboBox();
            comboBox.Height = 40;
            comboBox.Width = 580;

            comboBox.FontSize = 20;

            comboBox.Items.Add("Swimming");
            comboBox.Items.Add("Strength Training");
            comboBox.Items.Add("Swimming");
            comboBox.Items.Add("Yoga");
            comboBox.Items.Add("Cycling");
            comboBox.Items.Add("Running");
            comboBox.SelectedIndex = 0;

            Button SaveNameBtn = new Button();
            SaveNameBtn.Width = 65;
            SaveNameBtn.Content = "SaveName";
            SaveNameBtn.Click += SaveName;

            Button CancelBtn = new Button();
            CancelBtn.Width = 65;
            CancelBtn.Content = "Cancel";

            stackPanel.Children.Add(comboBox);
            stackPanel.Children.Add(SaveNameBtn);
            stackPanel.Children.Add(CancelBtn);

            ContentStackPanel.Children.Insert(0, stackPanel);

            this.addingState = true;
        }

        private void SaveName(object sender, RoutedEventArgs e)
        {
            Button clickedBtn = sender as Button;
            StackPanel stackPanel = clickedBtn.Parent as StackPanel;
            ComboBox comboBox = stackPanel.Children[0] as ComboBox;
            string planName = comboBox.SelectedItem.ToString();
            if (string.IsNullOrEmpty(planName))
            {
                return;
            }

            ContentStackPanel.Children.Remove(stackPanel);

            StackPanel newStackPanel = new StackPanel();
            newStackPanel.Orientation = Orientation.Horizontal;

            TextBlock textBlock = new TextBlock();
            textBlock.Background = Brushes.LightGray;
            textBlock.FontSize = 20;
            textBlock.Text = planName;
            textBlock.Height = 40;
            textBlock.Width = 310;

            Button SaveTimeBtn = new Button();
            SaveTimeBtn.Width = 65;
            SaveTimeBtn.Click += saveTime;
            SaveTimeBtn.Content = "SaveTime";

            Button DeleteBtn = new Button();
            DeleteBtn.Width = 65;
            DeleteBtn.Content = "Delete";
            DeleteBtn.Click += DeletePlan;

            newStackPanel.Children.Add(textBlock);
            newStackPanel.Children.Add(SaveTimeBtn);
            newStackPanel.Children.Add(DeleteBtn);

            this.StackPanelInPending = newStackPanel;

            ContentStackPanel.Children.Insert(0, newStackPanel);
            this.planName = planName;
            addTimeRange(newStackPanel);
        }

        private void addTimeRange(StackPanel newStackPanel)
        {
            TextBlock textBlockFrom = new TextBlock();
            textBlockFrom.Background = Brushes.LightGray;
            textBlockFrom.FontSize = 15;
            textBlockFrom.Width = 140;
            textBlockFrom.Text = "Tranning date";

            TextBox textBoxFrom = new TextBox();
            this.BoxFromInPending = textBoxFrom;
            textBoxFrom.FontSize = 20;
            textBoxFrom.Width = 120;
            textBoxFrom.Text = "dd/mm/yyyy";

            newStackPanel.Children.Insert(1, textBlockFrom);
            newStackPanel.Children.Insert(2, textBoxFrom);
        }

        private void saveTime(object sender, RoutedEventArgs e)
        {
            string time = this.BoxFromInPending.Text;
            if (DateTime.TryParseExact(time, "dd/MM/yyyy",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out DateTime parsedDate) &&
                            parsedDate.Date >= DateTime.Today)
            {
                TextBlock textBlockFrom = new TextBlock()
                {
                    Width = 120,
                    FontSize = 20,
                    Text = this.BoxFromInPending.Text,
                    Background = Brushes.LightGray
                };

                this.StackPanelInPending.Children.Remove(this.BoxFromInPending);

                Button saveTimeBtn = this.StackPanelInPending.Children
                    .OfType<Button>()
                    .FirstOrDefault(btn => btn.Content.ToString() == "SaveTime");

                if (saveTimeBtn != null)
                {
                    this.StackPanelInPending.Children.Remove(saveTimeBtn);
                }

                Button HistoryBtn = new Button();
                HistoryBtn.Width = 65;
                HistoryBtn.Content = "History";
                HistoryBtn.Click += AddToHistory;

                this.StackPanelInPending.Children.Insert(2, textBlockFrom);

                using (var db = new SQLiteConnection($"Data Source={DBPath}"))
                {
                    db.Open();

                    string createUserInfoTable = @"
                    CREATE TABLE IF NOT EXISTS UserTranings (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserId INTEGER NOT NULL,
                        Traning TEXT NOT NULL,
                        TraningTime TEXT,
                        FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
                );";

                    using (var comand = new SQLiteCommand(createUserInfoTable, db))
                    {
                        comand.ExecuteNonQuery();
                    }

                    using (var comand = new SQLiteCommand("INSERT INTO UserTranings (UserId, Traning, TraningTime) VALUES (@userId, @traning, @time)", db))
                    {
                        comand.Parameters.AddWithValue("@userId", this.userId);
                        comand.Parameters.AddWithValue("@traning", this.planName);
                        comand.Parameters.AddWithValue("@time", this.BoxFromInPending.Text);
                        comand.ExecuteNonQuery();
                    }

                    using (var comand = new SQLiteCommand("SELECT last_insert_rowid();", db))
                    {
                        HistoryBtn.Tag = (long)comand.ExecuteScalar();
                    }

                    this.StackPanelInPending.Children.Insert(3, HistoryBtn);
                }

                this.addingState = false;
            }
            else
            {
                MessageBox.Show("Invalid date format");
                return;
            }
        }

        private void DeletePlan(object sender, RoutedEventArgs e)
        {
            Button clickedBtn = sender as Button;
            StackPanel stackPanel = clickedBtn.Parent as StackPanel;
            ContentStackPanel.Children.Remove(stackPanel);
            Console.WriteLine(clickedBtn.Tag);
            using (var db = new SQLiteConnection($"Data Source={DBPath}"))
            {
                db.Open();
                using (var comand = new SQLiteCommand("DELETE FROM UserTranings WHERE Id = @id", db))
                {
                    comand.Parameters.AddWithValue("@id", clickedBtn.Tag);
                    comand.ExecuteNonQuery();
                }
            }
        }

        private void NavigateTo(object Sender, RoutedEventArgs e)
        {
            Button clickedBtn = Sender as Button;
            string btnName = clickedBtn.Name;
            switch (btnName)
            {
                case "addTrainBtn":
                    Plans PlansPage = new Plans(this.userId);
                    NavigationService.Navigate(PlansPage);
                    break;
                case "HistoryBtn":
                    HistoryPage historyPage = new HistoryPage(this.userId);
                    NavigationService.Navigate(historyPage);
                    break;
                case "exitBtn":
                    NavigationService.GoBack();
                    break;
            }
        }
    }
}
