using System.Text;

namespace HttpLib.Query
{
    /// <summary>
    /// Allows you to work with the query string to the server.
    /// </summary>
    public class Query
    {
        private readonly StringBuilder stringBuilder;

        /// <summary>
        /// Initializes class fields.
        /// </summary>
        public Query()
        {
            stringBuilder = new StringBuilder();
        }

        /// <summary>
        /// Adds a part to the query string.
        /// </summary>
        /// <param name="partOfQuery">Part of the query string.</param>
        public void AddToQuery(string partOfQuery)
        {
            stringBuilder.Append(partOfQuery);
        }

        /// <summary>
        /// Returns the query string.
        /// </summary>
        /// <returns>Query string.</returns>
        public string GetStringQuery()
        {
            return stringBuilder.ToString();
        }
    }
}
