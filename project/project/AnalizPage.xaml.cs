using System;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace project
{
    public partial class AnalizPage : Page
    {
        private int userId { get; set; }
        private string DBPath = "Users.db";
        public AnalizPage(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            GetDatas();
        }

        private void NavigateTo(object sender, RoutedEventArgs e)
        {
            Button clickedBtn = sender as Button;
            string btnName = clickedBtn.Name;
            switch (btnName)
            {
                case "HistoryBtn":
                    NavigationService.Navigate(new HistoryPage(userId));
                    break;
                case "exitBtn":
                    NavigationService.GoBack();
                    break;
            }
        }

        private void GetDatas()
        {
            using (var db = new SQLiteConnection($"Data Source={DBPath}"))
            {
                db.Open();
                string userInfoSearchString = "SELECT FirstName, LastName, HeightMM, WeightKG, Age, Goal FROM UserInformation WHERE UserId = @userId";
                using (var command = new SQLiteCommand(userInfoSearchString, db))
                {
                    command.Parameters.AddWithValue("@userId", this.userId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            GoalComboBox.Text = reader.GetString(5);
                            HeightTextBox.Text = reader.GetInt32(2).ToString();
                            WeightTextBox.Text = reader.GetDouble(3).ToString();
                            AgeTextBox.Text = reader.GetInt32(4).ToString();
                        }
                    }
                }
            }
        }

        private void CalculateKBJU(object sender, RoutedEventArgs e)
        {
            double weight = double.TryParse(WeightTextBox.Text, out double w) ? w : 0;
            double height = double.TryParse(HeightTextBox.Text, out double h) ? h : 0;
            int age = int.TryParse(AgeTextBox.Text, out int a) ? a : 0;
            string goal = (GoalComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Maintain Weight";

            double bmr = CalculateBMR(weight, height, age);
            double activityFactor = 1.2;
            double calories = bmr * activityFactor;

            switch (goal)
            {
                case "Gain Weight":
                    calories += 800;
                    break;
                case "Lose Weight":
                    calories -= 800;
                    break;
            }

            double proteins = weight * 2;
            double fats = weight * 1;
            double carbs = (calories - (proteins * 4 + fats * 9)) / 4;

            ResultStackPanel.Visibility = Visibility.Visible;
            CaloriesText.Text = $"Calories: {calories:F0} kcal";
            ProteinsText.Text = $"Proteins: {proteins:F1} g";
            FatsText.Text = $"Fats: {fats:F1} g";
            CarbsText.Text = $"Carbs: {carbs:F1} g";

            DrawWeightChart(weight, goal);
        }

        private double CalculateBMR(double weight, double height, int age)
        {
            return 10 * weight + 6.25 * height - 5 * age + 5;
        }

        private void DrawWeightChart(double initialWeight, string goal)
        {
            ChartCanvas.Children.Clear();

            double canvasWidth = ChartCanvas.ActualWidth;
            double canvasHeight = ChartCanvas.ActualHeight;

            var xAxis = new Line
            {
                X1 = 0,
                Y1 = canvasHeight,
                X2 = canvasWidth,
                Y2 = canvasHeight,
                Stroke = Brushes.White,
                StrokeThickness = 2
            };

            var yAxis = new Line
            {
                X1 = 0,
                Y1 = 0,
                X2 = 0,
                Y2 = canvasHeight,
                Stroke = Brushes.White,
                StrokeThickness = 2
            };

            ChartCanvas.Children.Add(xAxis);
            ChartCanvas.Children.Add(yAxis);

            double currentWeight = initialWeight;
            double weightChangePerWeek = goal == "Gain Weight" ? 1.0 : (goal == "Lose Weight" ? -1.0 : 0);
            double xStep = canvasWidth / 12;

            for (int i = 0; i < 12; i++)
            {
                double x = i * xStep;
                double y = canvasHeight - (currentWeight / 150 * canvasHeight);

                var point = new Ellipse
                {
                    Width = 5,
                    Height = 5,
                    Fill = Brushes.Red,
                    Margin = new Thickness(x - 2.5, y - 2.5, 0, 0)
                };

                ChartCanvas.Children.Add(point);

                if (i > 0)
                {
                    var line = new Line
                    {
                        X1 = (i - 1) * xStep,
                        Y1 = canvasHeight - ((currentWeight - weightChangePerWeek) / 150 * canvasHeight),
                        X2 = x,
                        Y2 = y,
                        Stroke = Brushes.Blue,
                        StrokeThickness = 2
                    };

                    ChartCanvas.Children.Add(line);
                }

                currentWeight = Math.Max(0, currentWeight + weightChangePerWeek);
            }
        }
    }
}