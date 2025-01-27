using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;

namespace project
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        private System.Windows.Controls.Calendar calendar;
        public DialogWindow(string type, Object elem)
        {
            InitializeComponent();
            inserDialogData(type);
        }

        private void inserDialogData(string type)
        {
            switch (type)
            {
                case "Calendar":
                    Button button = new Button()
                    {
                        Content = "Save"
                    };
                    button.Click += SaveDate;
                    button.Tag = "Сalendar";
                    ContentStackPanel.Children.Add(button);

                    calendar = new System.Windows.Controls.Calendar()
                    {
                        Name = "Сalendar",
                        SelectionMode = CalendarSelectionMode.MultipleRange
                    };

                    ContentStackPanel.Children.Insert(0, calendar);
                    break;
                case "task":
                    break;
                case "subtask":
                    break;
            }
        }

        private void SaveDate(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDate = calendar.SelectedDate;

            if (selectedDate.HasValue)
            {
                MessageBox.Show("Вы выбрали: " + selectedDate.Value.ToString("d"));
            }
            else
            {
                MessageBox.Show("Дата не выбрана.");
            }
            this.Close();
        }
    }
}
