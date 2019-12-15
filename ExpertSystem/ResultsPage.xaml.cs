using ExpertSystem.Algorithm;
using ExpertSystem.Services;
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

namespace ExpertSystem
{
    /// <summary>
    /// Interaction logic for ResultsPage.xaml
    /// </summary>
    public partial class ResultsPage : Page
    {
        private ExpertSystemAlgorithm _expertSystem;
        private List<string> _finalResults;

        public ResultsPage(ExpertSystemAlgorithm expertSystem)
        {
            InitializeComponent();
            _expertSystem = expertSystem;
            _finalResults = _expertSystem.GetFinalResults();

            var stats = _finalResults
                .GroupBy(s => s)
                .Select(x => new
                {
                    Id = x.Key,
                    Count = x.Count()
                })
                .OrderByDescending(x => x.Count);

            int overallCount = stats.Sum(x => x.Count);

            stats.ToList().ForEach(a =>
            {
                string text;

                if (a.Id == "")
                    text = "No mental disorders diagnosed";
                else
                {
                    text = QuestionDataLoader.Instance.QuestionData
                        .Where(x => x.Id == 9).First().Answers
                            .Where(y => y.Id == int.Parse(a.Id)).First().String;
                }

                Label label = new Label
                {
                    Content = $"{text}: {Decimal.Round((decimal)100.0 * a.Count / overallCount, 2)}% ({a.Count} of {overallCount})"
                };

                resultPanel.Children.Add(label);
            });
        }
    }
}
