using ExpertSystem.Helpers;
using System;
using System.Windows;

namespace ExpertSystem
{
    /// <summary>
    /// Interaction logic for LoggerWindow.xaml
    /// </summary>
    public partial class LoggerWindow : Window
    {
        public LoggerWindow()
        {
            InitializeComponent();
            Logger.Instance.Logged += new LogEventHandler(logger_Logged);
        }

        private void logger_Logged(object sender, LogEventArgs e)
        {
            textBox.Text += $"[{DateTime.Now}] {e.Type.ToString()}: {e.Message}{Environment.NewLine}";
        }
    }
}
