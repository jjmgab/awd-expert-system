using ExpertSystem.Question;
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

        public MainWindow()
        {
            ConsoleManager.Show();
            InitializeComponent();

            _loader = QuestionDataLoader.Instance;
            _loader.Initialize(_jsonPath);

            Console.WriteLine(_loader.GetQuestionDataById(1)?.String);

            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder()
            {
                DataSource = _dbPath
            };

            using (SQLiteConnection conn = new SQLiteConnection(builder.ConnectionString).OpenAndReturn())
            {
                SQLiteCommand command = new SQLiteCommand("SELECT * FROM DATA WHERE ID<15", conn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(reader.GetValue(0));
                }

                conn.Close();
            }
        }
    }
}
