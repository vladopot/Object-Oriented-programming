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
    /// Interaction logic for HistoryPage.xaml
    /// </summary>
    public partial class HistoryPage : Page
    {
        private int userId { get; set; }
        private string DBPath = "Users.db";
        public HistoryPage(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            restoreHistory();
        }

        private void restoreHistory()
        {
            using (var db = new SQLiteConnection($"Data Source={DBPath}"))
            {
                db.Open();
                string createUserInfoTable = @"
                    CREATE TABLE IF NOT EXISTS History (
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
                using (var comand = new SQLiteCommand("SELECT * FROM History WHERE UserId = @userId", db))
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


                            newStackPanel.Children.Add(textBlockFrom);
                            newStackPanel.Children.Add(textBoxFrom);
                            newStackPanel.Children.Add(DeleteBtn);
                            //newStackPanel.Children.Add(SaveTimeBtn);

                            ContentStackPanel.Children.Insert(0, newStackPanel);

                        }
                    }
                }
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
                using (var comand = new SQLiteCommand("DELETE FROM History WHERE Id = @id", db))
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
    }
}
