namespace ExpertSystem.Questions
{
    /// <summary>
    /// Class defining answer data (answer string and the id,
    /// as represented in the database).
    /// </summary>
    public class AnswerData
    {
        /// <summary>
        /// Answer id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Answer string.
        /// </summary>
        public string String { get; set; }
    }
}
