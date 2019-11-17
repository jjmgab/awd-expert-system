using ExpertSystem.Question;
using ExpertSystem.Services;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
using UniversalHelpers.ConsoleManager;

namespace ExpertSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _dbPath = @"..\..\data\awd-data.sqlite";
        private string _jsonPath = @"..\..\data\questionData.json";

        private QuestionDataLoader _loader;
        private DbHandler _handler;

        public MainWindow()
        {
            ConsoleManager.Show();
            InitializeComponent();

            _loader = QuestionDataLoader.Instance;
            _loader.Initialize(new object[] { _jsonPath });

            _handler = DbHandler.Instance;
            _handler.Initialize(new object[] { _dbPath });

            Console.WriteLine(_loader.GetQuestionDataById(1)?.String);

            SQLiteDataReader reader = _handler.ExecuteQuery("SELECT * FROM DATA WHERE ID < 15");
            while (reader.Read())
            {
                Console.WriteLine(reader.GetValue(0));
            }

            _handler.Close();
        }
    }
}
