using ExpertSystem.Questions;
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
    public partial class MainWindow : NavigationWindow
    {
        private string _dbPath = @"..\..\data\awd-data.sqlite";
        private string _jsonPath = @"..\..\data\questionData.json";

        private QuestionDataLoader _loader;
        private DbHandler _handler;

        private Stack<int> stackId;
        private List<Question> questions = new List<Question>();

        public MainWindow()
        {
            ConsoleManager.Show();
            InitializeComponent();

            stackId = new Stack<int>();

            _loader = QuestionDataLoader.Instance;
            _loader.Initialize(new object[] { _jsonPath });

            //_handler = DbHandler.Instance;
            //_handler.Initialize(new object[] { _dbPath });

            _loader.QuestionIds.ToList().ForEach((x) => { stackId.Push(x); });
            //stackId.Push(2);

            Page_OnNext();

            //SQLiteDataReader reader = _handler.ExecuteQuery("SELECT * FROM DATA WHERE ID < 15");
            //while (reader.Read())
            //{
            //    Console.WriteLine(reader.GetValue(0));
            //}

            //_handler.Close();
        }

        private void Page_OnNext()
        {
            if (stackId.Count > 0)
            {
                Question question = new Question(stackId.Pop());
                questions.Add(question);
                QuestionPage page = new QuestionPage(question);
                page.OnNext += Page_OnNext;
                Navigate(page);
            }
            else
            {
                StringBuilder builder = new StringBuilder();
                questions.ForEach(q =>
                {
                    string ans = "";
                    q.AnswerIds.ForEach(a =>
                    {
                        ans += a.ToString() + " ";
                    });
                    builder.AppendLine($"Q{q.QuestionId}: {ans}");
                });
                MessageBox.Show(builder.ToString());
            }
        }
    }
}
