using ExpertSystem.Services;
using System.Collections.Generic;

namespace ExpertSystem.Questions
{
    /// <summary>
    /// Class representing question data and user answer to it.
    /// </summary>
    public class Question
    {
        /// <summary>
        /// Question id.
        /// </summary>
        public int QuestionId { get; set; }

        /// <summary>
        /// Chosen answer ids.
        /// </summary>
        public List<int> AnswerIds { get; }

        /// <summary>
        /// Question data - strings and possible answers.
        /// </summary>
        public QuestionData Data { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">Question id</param>
        public Question(int id)
        {
            QuestionId = id;
            Data = QuestionDataLoader.Instance.GetQuestionDataById(QuestionId);
            AnswerIds = new List<int>(); // -1 when not answered
        }
    }
}
