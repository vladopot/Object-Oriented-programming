using System;
using System.Collections.Generic;
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
        TextBox BoxToInPending = null;
        StackPanel StackPanelInPending = null;
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
                            textBlock.Width = 250;

                            //Button SaveTimeBtn = new Button();
                            //SaveTimeBtn.Width = 65;
                            //SaveTimeBtn.Click += saveTime;
                            //SaveTimeBtn.Content = "SaveTime";
                            Button DeleteBtn = new Button();
                            DeleteBtn.Width = 65;
                            DeleteBtn.Content = "Delete";
                            DeleteBtn.Click += DeletePlan;

                            newStackPanel.Children.Add(textBlock);
                            if (!reader.IsDBNull(reader.GetOrdinal("TraningTime")))
                            {
                                TextBox textBoxFrom = new TextBox();
                                this.BoxFromInPending = textBoxFrom;
                                textBoxFrom.FontSize = 20;
                                textBoxFrom.Width = 120;
                                textBoxFrom.Text = reader["TraningTime"].ToString();
                                newStackPanel.Children.Add(textBoxFrom);
                                newStackPanel.Children.Add(DeleteBtn);
                            }
                            else
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
                                addTimeRange(newStackPanel);
                            }
                            //newStackPanel.Children.Add(SaveTimeBtn);
                            
                            this.StackPanelInPending = newStackPanel;
                            ContentStackPanel.Children.Insert(0, newStackPanel);
                            
                        }
                    }
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

            this.addingState = false;
            ContentStackPanel.Children.Remove(stackPanel);

            StackPanel newStackPanel = new StackPanel();
            newStackPanel.Orientation = Orientation.Horizontal;

            TextBlock textBlock = new TextBlock();
            textBlock.Background = Brushes.LightGray;
            textBlock.FontSize = 20;
            textBlock.Text = planName;
            textBlock.Height = 40;
            textBlock.Width = 250;

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
            addTimeRange(newStackPanel);
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

                using (var comand = new SQLiteCommand("INSERT INTO UserTranings (UserId, Traning) VALUES (@userId, @traning)", db))
                {
                    comand.Parameters.AddWithValue("@userId", this.userId);
                    comand.Parameters.AddWithValue("@traning", planName);
                    comand.ExecuteNonQuery();
                }
            }
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
            TextBlock textBlockFrom = new TextBlock()
            {
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

            this.StackPanelInPending.Children.Insert(2, textBlockFrom);

            using (var connection = new SQLiteConnection($"Data Source={DBPath}"))
            {
                connection.Open();

                string updateString = @"UPDATE UserTranings
                                        SET TraningTime = @date
                                        WHERE UserId = @userId;";

                using (var command = new SQLiteCommand(updateString, connection))
                {
                    command.Parameters.AddWithValue("@userId", this.userId);
                    command.Parameters.AddWithValue("@date", this.BoxFromInPending.Text);
                    command.ExecuteReader();
                }
            }
        }

        private void DeletePlan(object sender, RoutedEventArgs e)
        {
            Button clickedBtn = sender as Button;
            StackPanel stackPanel = clickedBtn.Parent as StackPanel;
            ContentStackPanel.Children.Remove(stackPanel);
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
                case "exitBtn":
                    NavigationService.GoBack();
                    break;
            }
        }
    }
}
