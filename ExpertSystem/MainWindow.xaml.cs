using ExpertSystem.Algorithm;
using ExpertSystem.Helpers;
using ExpertSystem.Questions;
using ExpertSystem.Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;

namespace ExpertSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        private LoggerWindow _loggerWindow = new LoggerWindow();

        private string _dbPath = @"..\..\data\awd-data.sqlite";
        private string _jsonPath = @"..\..\data\questionData.json";

        private QuestionDataLoader _loader;
        private DbHandler _handler;
        private ExpertSystemAlgorithm _system;

        private MainPage _mainPage;

        private Stack<int> _stackId;
        private List<Question> _questions = new List<Question>();

        public MainWindow()
        {
            InitializeComponent();

            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            _loggerWindow.Show();

            _stackId = new Stack<int>();

            Logger.Info($"Initializing QuestionDataLoader ({_jsonPath}).");
            _loader = QuestionDataLoader.Instance;
            _loader.Initialize(new object[] { _jsonPath });

            Logger.Info($"Initializing DbHandler ({_dbPath}).");
            _handler = DbHandler.Instance;
            _handler.Initialize(new object[] { _dbPath });

            _loader.QuestionIds.ToList().ForEach((x) => { _stackId.Push(x); });

            Logger.Info("Adding questions.");
            List<Question> questions = new List<Question>();
            _loader.QuestionIds.ToList().ForEach(i => questions.Add(new Question(i)));
            Logger.Info($"{questions.Count} questions added.");

            Logger.Info("Initializing ExpertSystemAlgorithm.");
            _system = new ExpertSystemAlgorithm(questions);


            _mainPage = new MainPage(_system.AvailableQuestionsCount);
            _mainPage.OnNext += MainPage_Started;
            _mainPage.OnNext += Page_OnNext;

            Navigate(_mainPage);
        }

        private void MainPage_Started()
        {
            _system.QuestionCountMax = _mainPage.SystemSettings.QuestionsToAskCount;
            Logger.Info($"Questions to ask count changed to {_mainPage.SystemSettings.QuestionsToAskCount}.");
        }

        private void Page_OnNext()
        {
            if (_system.HasAvailable)
            {
                Logger.Info("Page_OnNext: There are available questions.");
                Question question = _system.PickNext();

                if (question != null)
                {
                    _questions.Add(question);
                    QuestionPage page = new QuestionPage(question);
                    page.OnNext += Page_OnNext;
                    Navigate(page);
                }
                else
                {
                    NoMoreQuestions();
                }
            }
            else
            {
                NoMoreQuestions();
            }
        }

        private void NoMoreQuestions()
        {
            Logger.Info("Page_OnNext: There are no available questions.");
            Logger.Info("--- Answer summary:");

            _questions.ForEach(q =>
            {
                Logger.Info($"Q{q.QuestionId} ({q.Data.String})");
                q.AnswerIds.ForEach(a =>
                {
                    Logger.Info($"A{a} ({q.Data.Answers.Where(ans => ans.Id == a).FirstOrDefault()?.String})");
                });
            });

            Logger.Info("Showing the results page.");
            ResultsPage page = new ResultsPage(_system);
            Navigate(page);

            _handler.Close();
        }

        private void NavigationWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
