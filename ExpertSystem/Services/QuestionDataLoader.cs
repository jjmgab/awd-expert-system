using ExpertSystem.Base;
using ExpertSystem.Question;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExpertSystem.Services
{
    /// <summary>
    /// Thread-unsafe singleton class for question data loading.
    /// 
    /// Class needs to be initialized first with Initialize(object[] args) method.
    /// There should be only one element, path to JSON file.
    /// </summary>
    public sealed class QuestionDataLoader : SingletonBase<QuestionDataLoader>
    {
        /// <summary>
        /// List of question data.
        /// </summary>
        private List<QuestionData> _questionData;
        /// <summary>
        /// List of question data.
        /// </summary>
        public List<QuestionData> QuestionData => _questionData;

        /// <summary>
        /// Array of question ids.
        /// </summary>
        private int[] _questionIds;
        /// <summary>
        /// Array of question ids.
        /// </summary>
        public int[] QuestionIds => _questionIds;

        /// <summary>
        /// Private constructor.
        /// </summary>
        private QuestionDataLoader() { }

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
        /// Initializes the class.
        /// </summary>
        protected override void Init(object[] args)
        {
            if (!(args[0] is string))
                throw new InvalidDataException("First argument is not a string.");

            if (!File.Exists(args[0] as string))
                throw new FileNotFoundException();

            string json = File.ReadAllText(args[0] as string);
            Instance._questionData = JsonConvert.DeserializeObject<List<QuestionData>>(json);
            Instance._questionIds = (from question in Instance._questionData
                                      select question.Id).ToArray();
        }

        /// <summary>
        /// Resets loader state to post-constructor.
        /// </summary>
        public override void ResetState()
        {
            base.ResetState();
            Instance._questionIds = null;
            Instance._questionData = null;
        }

        /// <summary>
        /// Returns question data by id.
        /// </summary>
        /// <param name="id">Question id</param>
        /// <returns>Question with corresponding id</returns>
        public QuestionData GetQuestionDataById(int id)
        {
            PedanticCheck();

            return (from question in Instance.QuestionData
                    where question.Id == id
                    select question).FirstOrDefault();
        }
    }
}
