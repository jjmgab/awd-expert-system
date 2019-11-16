using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

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
        /// Flag indicating if the loader was already initialized.
        /// </summary>
        private bool _isInitialized = false;
        /// <summary>
        /// Flag indicating if the loader was already initialized.
        /// </summary>
        public bool IsInitialized => _isInitialized;

        /// <summary>
        /// Private constructor.
        /// </summary>
        private QuestionDataLoader() { }

        /// <summary>
        /// Reads and parses the JSON.
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public bool Initialize(string dataSource)
        {
            if (!_isInitialized)
            {
                string json = File.ReadAllText(dataSource);
                _instance._questionData = JsonConvert.DeserializeObject<List<QuestionData>>(json);
                _instance._questionIds = (from question in _instance._questionData
                                          select question.Id).ToArray();

                _isInitialized = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Resets loader state to post-constructor.
        /// </summary>
        public void ResetState()
        {
            _isInitialized = false;
            _instance._questionIds = null;
            _instance._questionData = null;

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
            PedanticCheck();

            return (from question in _instance.QuestionData
                    where question.Id == id
                    select question).FirstOrDefault();
        }

        /// <summary>
        /// Checks for validity of loader current state.
        /// Throws InvalidOperationException if invalid.
        /// </summary>
        private void PedanticCheck()
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException("Loader not yet initialized.");
            }
        }
    }
}
