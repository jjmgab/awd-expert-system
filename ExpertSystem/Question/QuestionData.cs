using System.Collections.Generic;

namespace ExpertSystem.Question
{
    /// <summary>
    /// Class defining question data.
    /// </summary>
    public class QuestionData
    {
        /// <summary>
        /// Question id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Question string.
        /// </summary>
        public string String { get; set; }

        /// <summary>
        /// List of possible answers for that question.
        /// </summary>
        public List<AnswerData> Answers { get; set; }
    }
}
