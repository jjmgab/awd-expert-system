namespace ExpertSystem.Question
{
    /// <summary>
    /// Class representing question data and user answer to it.
    /// </summary>
    class Question
    {
        /// <summary>
        /// Question id.
        /// </summary>
        public int QuestionId { get; set; }

        /// <summary>
        /// Chosen answer id.
        /// </summary>
        public int AnswerId { get; }

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
            AnswerId = -1; // -1 when not answered
        }
    }
}
