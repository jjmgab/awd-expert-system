using ExpertSystem.Helpers;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ExpertSystem
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public event Action OnNext;

        private Settings _settings;
        public Settings SystemSettings => _settings;

        bool isValid = true;

        private int _availableQuestionsCount;

        public MainPage(int availableQuestionsCount)
        {
            InitializeComponent();
            Logger.Info("MainPage: Initializing.");
            _availableQuestionsCount = availableQuestionsCount;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool isParseable = int.TryParse(textBoxQNumber.Text, out int value);
            bool isWithinRange = isParseable && value < _availableQuestionsCount && value > 0;

            isValid = isWithinRange;

            if (isValid)
            {
                _settings = new Settings(int.Parse(textBoxQNumber.Text));

                (sender as Button).IsEnabled = false;
                OnNext?.Invoke();
            }
            else
            {
                if (!isParseable)
                    Logger.Error("Not parseable");
                else if (!isWithinRange)
                    Logger.Error("Not within range");

                textBoxQNumber.Background = new SolidColorBrush(Color.FromRgb(255, 172, 172));
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            bool isMatch = regex.IsMatch(e.Text);

            e.Handled = regex.IsMatch(e.Text);
        }

        public class Settings
        {
            public int QuestionsToAskCount { get; set; }

            public Settings(int questionsToAskCount)
            {
                QuestionsToAskCount = questionsToAskCount;
            }
        }
    }
}
