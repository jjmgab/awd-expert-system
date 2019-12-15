using ExpertSystem.Helpers;
using ExpertSystem.Questions;
using ExpertSystem.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace ExpertSystem.Algorithm
{
    public class ExpertSystemAlgorithm
    {
        private List<Question> _availableQuestions;
        public int AvailableQuestionsCount => _availableQuestions.Count;
        public bool HasAvailable => _availableQuestions.Count > 0;

        private List<Question> _answeredQuestions;
        public int AnsweredQuestionsCount => _answeredQuestions.Count;

        public int QuestionCountMax { get; set; } = 5;
        private int _questionCount = 0;

        private readonly string resultColumn = "Q009";

        private readonly string strAll = "*";
        private readonly string strFrom = "FROM DATA";
        private readonly string strWhere = "WHERE 1=1";
        private readonly string strAnd = " AND ";
        private readonly string strGroupBy = "GROUP BY ";
        private readonly string strNotNull = "!=\"\"";
        private readonly string strColon = ";";

        private readonly string strTemplSelect = "SELECT {0}";
        private readonly string strTemplQuestion = "Q{0:000}";
        private readonly string strTemplQuestionAndCount = "COUNT(DISTINCT Q{0:000})";
        private readonly string strTemplSingle = " = {0}";
        private readonly string strTemplMultiple = " IN ({0})";

        public ExpertSystemAlgorithm(List<Question> questions)
        {
            _availableQuestions = questions;
            _answeredQuestions = new List<Question>();

            Logger.Info($"ExpertSystemAlgorithm: Available questions: {_availableQuestions.Count}");
            Logger.Info("ExpertSystemAlgorithm: Removing result data question.");
            _availableQuestions.Remove(_availableQuestions.Where(x => x.Data.String == "[TO_DELETE]").First());
            Logger.Info($"ExpertSystemAlgorithm: Available questions: {_availableQuestions.Count}");
        }

        public Question PickNext()
        {
            Logger.Info("ExpertSystemAlgorithm: Picking next question...");

            if (_questionCount >= QuestionCountMax)
            {
                Logger.Info("That was the last question.");
                return null;
            }

            Question question = GetQuestionWithMostDistinctValues();
            while (GetQueryResultCount(GetResultantQuery()) == 0)
            {
                Logger.Warning($"ExpertSystemAlgorithm: PickNext(): No records in resultant query; ignoring last ({_answeredQuestions.Last().QuestionId}) question.");
                _answeredQuestions.Remove(_answeredQuestions.Last());
                _questionCount--;
            }
            while (question == null && _availableQuestions.Count > 0)
            {
                question = GetQuestionWithMostDistinctValues();
            }

            if (question != null)
            {
                _availableQuestions.Remove(question);
                _answeredQuestions.Add(question);
                _questionCount++;
                return question;
            }

            Logger.Info("There is no more questions");
            return null;
        }

        public List<string> GetFinalResults()
        {
            int count;
            SQLiteDataReader reader;

            do
            {
                count = 0;

                Logger.Info("Generating final query string...");
                string query = GetResultantQuery();
                Logger.Info(Environment.NewLine + query);
                DbHandler dbHandler = DbHandler.Instance;
                Logger.Info("Executing final query...");
                reader = dbHandler.ExecuteQuery(query);

                while (reader.Read())
                {
                    count++;
                }

                if (count < 1)
                {
                    Logger.Warning("There are no rows in result table.");
                    Question questionToReject = _answeredQuestions.Last();
                    Logger.Warning($"Removing last question (id {questionToReject.QuestionId})");

                    _answeredQuestions.Remove(questionToReject);
                }

                reader = dbHandler.ExecuteQuery(query);

            } while (count < 1);
            

            List<string> result = new List<string>();
            while (reader.Read())
            {
                try
                {
                    result.Add(reader.GetString(0));
                }
                catch (Exception)
                {
                    result.Add(reader.GetInt32(0).ToString());
                }
                
            }

            Logger.Info($"Final query result row count: {result.Count}");
            return result;
        }

        private int GetQueryResultCount(string query)
        {
            DbHandler dbHandler = DbHandler.Instance;
            SQLiteDataReader reader = dbHandler.ExecuteQuery(query);

            int count = 0;
            while (reader.Read())
                count++;

            Logger.Info($"ExpertSystemAlgorithm: GetQueryResultCount: {count}.");

            return count;
        }

        private Question GetQuestionWithMostDistinctValues()
        {
            Logger.Info("ExpertSystemAlgorithm: GetQuestionWithMostDistinctValues()");
            List<Tuple<Question, int>> pairList = new List<Tuple<Question, int>>();

            Logger.Info($"ExpertSystemAlgorithm: There are {_availableQuestions.Count} available questions.");
            _availableQuestions.ForEach(q =>
            {
                string query = GetResultantQuery(q);

                DbHandler dbHandler = DbHandler.Instance;
                SQLiteDataReader reader = dbHandler.ExecuteQuery(query);

                reader.Read();
                int number = reader.GetInt32(0);
                pairList.Add(new Tuple<Question, int>(q, number));
            });

            if (pairList.Count > 0)
            {
                Question maxQ = pairList[0].Item1;
                int maxQCount = pairList[0].Item2;
                pairList.ForEach(t =>
                {
                    if (t.Item2 > maxQCount)
                    {
                        maxQ = t.Item1;
                        maxQCount = t.Item2;
                    }
                });

                Logger.Info($"ExpertSystemAlgorithm: Question {maxQ.QuestionId} picked ({maxQCount} distinct).");
                Console.WriteLine($"Max distinct: {maxQCount} in Q{maxQ.QuestionId}");

                if (maxQCount < 1)
                {
                    Logger.Warning("ExpertSystemAlgorithm: No records. Ignoring and removing from available.");
                    _availableQuestions.Remove(maxQ);

                    return null;
                }

                return maxQ;
            }

            Logger.Warning("ExpertSystemAlgorithm: No more available questions.");
            return null;
        }

        private string GetResultantQuery(Question q = null)
        {
            StringBuilder builder = new StringBuilder();
            string strQ = "";

            if (q != null)
            {
                strQ = string.Format(strTemplQuestion, q.Data.Id);
                builder.AppendLine(string.Format(strTemplSelect, string.Format(strTemplQuestionAndCount, q.Data.Id)));
            }
            else
            {
                builder.AppendLine(string.Format(strTemplSelect, resultColumn));
            }
            builder.AppendLine(strFrom);
            builder.AppendLine(strWhere);

            if (q != null)
                builder.AppendLine(strAnd + strQ + strNotNull);
            //else
            //    builder.AppendLine(strAnd + resultColumn + strNotNull);

            for (int i = 0; i < _answeredQuestions.Count; i++)
            {
                builder.Append(strAnd);
                Question answeredQuestion = _answeredQuestions[i];
                string strQuestion = string.Format(strTemplQuestion, answeredQuestion.Data.Id);
                string strAnswer = "";
                int answerIdCount = answeredQuestion.AnswerIds.Count;

                if (answerIdCount > 1)
                {
                    string strIds = "";
                    for (int j = 0; j < answerIdCount; j++)
                    {
                        strIds += answeredQuestion.AnswerIds[j];
                        if (j < answerIdCount - 1)
                            strIds += ", ";
                    }
                    strAnswer = strQuestion + string.Format(strTemplMultiple, strIds);
                }
                else if (answerIdCount == 1)
                {
                    strAnswer = strQuestion + string.Format(strTemplSingle, answeredQuestion.AnswerIds[0]);
                }
                builder.Append(strAnswer);
            }
            builder.AppendLine(strColon);
            return builder.ToString();
        }

        public class FinalQueryResult
        {
            public int Id { get; }
            public int Count { get; }

            public FinalQueryResult(int id, int count)
            {
                Id = id;
                Count = count;
            }
        }
    }
}
