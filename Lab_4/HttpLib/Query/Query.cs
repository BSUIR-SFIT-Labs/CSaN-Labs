using System.Text;

namespace HttpLib.Query
{
    public class Query
    {
        private readonly StringBuilder stringBuilder;

        public Query()
        {
            stringBuilder = new StringBuilder();
        }

        public void AddToQuery(string partOfQuery)
        {
            stringBuilder.Append(partOfQuery);
        }

        public string GetStringQuery()
        {
            return stringBuilder.ToString();
        }
    }
}
