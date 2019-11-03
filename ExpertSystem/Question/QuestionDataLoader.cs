using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExpertSystem.Question
{
    /// <summary>
    /// Thread-unsafe singleton class for question data loading.
    /// </summary>
    public sealed class QuestionDataLoader
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        private static QuestionDataLoader _instance = null;

        /// <summary>
        /// Path to JSON with data.
        /// TODO: change it somehow.
        /// </summary>
        private static string _dataPath = @"../../questionData.json";

        /// <summary>
        /// List of question data.
        /// </summary>
        public List<QuestionData> QuestionData { get; }

        /// <summary>
        /// Array of question ids.
        /// </summary>
        public int[] QuestionIds { get; }

        /// <summary>
        /// Private constructor.
        /// Reads and parses the JSON.
        /// </summary>
        private QuestionDataLoader()
        {
            string json = File.ReadAllText(_dataPath);
            QuestionData = JsonConvert.DeserializeObject<List<QuestionData>>(json);
            QuestionIds = (from question in QuestionData
                           select question.Id).ToArray();
        }

        /// <summary>
        /// Returns the singleton instance.
        /// </summary>
        public static QuestionDataLoader Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new QuestionDataLoader();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Returns question data by id.
        /// </summary>
        /// <param name="id">Question id</param>
        /// <returns>Question with corresponding id</returns>
        public QuestionData GetQuestionDataById(int id)
        {
            return (from question in QuestionData
                    where question.Id == id
                    select question).FirstOrDefault();
        }
    }
}
