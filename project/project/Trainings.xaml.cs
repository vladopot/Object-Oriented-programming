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

namespace project
{
    /// <summary>
    /// Interaction logic for Trainings.xaml
    /// </summary>
    public partial class Trainings : Page
    {
        public Trainings()
        {
            InitializeComponent();
        }

        private void NavigateTo(object Sender, RoutedEventArgs e)
        {
            Button clickedBtn = Sender as Button;
            string btnName = clickedBtn.Name;
            switch (btnName)
            {
                case "addTrainBtn":
                    Trainings trainings = new Trainings();
                    NavigationService.Navigate(trainings);
                    break;
                case "exitBtn":
                    NavigationService.GoBack();
                    break;
            }
        }
    }
}
