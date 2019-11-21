using ExpertSystem.Questions;
using ExpertSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for QuestionPage.xaml
    /// </summary>
    public partial class QuestionPage : Page
    {
        public event Action OnNext;

        private Question _question;

        private HashSet<string> answerStringSet;

        public QuestionPage(Question question)
        {
            InitializeComponent();
            _question = question;

            this.Title = $"Question {_question.QuestionId}";
            answerStringSet = new HashSet<string>();

            labelQuestion.Content = _question.Data.String;

            foreach (AnswerData answer in _question.Data.Answers)
            {
                ToggleButton control = null;

                if (_question.Data.IsSingleAnswer)
                {
                    control = new RadioButton()
                    {
                        Content = answer.String,
                        GroupName = _question.QuestionId.ToString()
                    };
                    control.Checked += RadioControl_Checked;
                }
                else
                {
                    control = new CheckBox()
                    {
                        Content = answer.String
                    };
                    control.Checked += CheckControl_Checked;
                    control.Unchecked += CheckControl_Unchecked;
                }

                stackAnswers.Children.Add(control);
            }
        }

        private void CheckControl_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox control = sender as CheckBox;
            answerStringSet.Remove(control.Content as string);
        }

        private void CheckControl_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox control = sender as CheckBox;
            answerStringSet.Add(control.Content as string);
        }

        private void RadioControl_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton control = sender as RadioButton;
            answerStringSet.Clear();
            answerStringSet.Add(control.Content as string);
        }

        private void ButtonAccept_Click(object sender, RoutedEventArgs e)
        {
            List<int> answerIds = new List<int>();
            //StringBuilder builder = new StringBuilder();
            answerStringSet.ToList().ForEach(x => {
                //builder.AppendLine(x);
                answerIds.Add(_question.Data.Answers
                    .Where(a => a.String == x)
                    .Select(a => a.Id)
                    .FirstOrDefault());
            });
            _question.AnswerIds.AddRange(answerIds);
            //builder.AppendLine($"Id count: {_question.AnswerIds.Count}");
            //MessageBox.Show(_question.AnswerIds.Count.ToString());

            (sender as Button).IsEnabled = false;
            OnNext?.Invoke();
        }
    }
}
