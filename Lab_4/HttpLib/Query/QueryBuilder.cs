namespace HttpLib.Query
{
    /// <summary>
    /// Allows to "build" a query string.
    /// </summary>
    public class QueryBuilder : IBuilder
    {
        private Query _query;

        /// <summary>
        /// Initializes class fields.
        /// </summary>
        public QueryBuilder()
        {
            Reset();
        }

        /// <summary>
        /// Creates a new query string object.
        /// </summary>
        public void Reset()
        {
            _query = new Query();
        }

        /// <summary>
        /// Adds a request method: GET, HEAD, or POST.
        /// </summary>
        /// <param name="method">Request method.</param>
        public void AddMethod(string method)
        {
            _query.AddToQuery(method);
        }

        /// <summary>
        /// Adds the path where the request is to be executed.
        /// </summary>
        /// <param name="path">The path to the resource.</param>
        public void AddPath(string path)
        {
            _query.AddToQuery(path);
        }

        /// <summary>
        /// Adds a protocol version (HTTP v1.0).
        /// </summary>
        public void AddVersion()
        {
            _query.AddToQuery(QueryStrings.HttpVersion);
        }

        /// <summary>
        /// Adds a host.
        /// </summary>
        /// <param name="host">Host.</param>
        public void AddHost(string host)
        {
            _query.AddToQuery(QueryStrings.Host + host + QueryStrings.NewLine);
        }


        /// <summary>
        /// Adds information about the client.
        /// </summary>
        public void AddUserAgent()
        {
            _query.AddToQuery(QueryStrings.UserAgent);
        }

        /// <summary>
        /// Adds information about files that the client can accept.
        /// </summary>
        public void AddAccept()
        {
            _query.AddToQuery(QueryStrings.AcceptedFiles);
        }

        /// <summary>
        /// Adds the type of content that is sent to the server.
        /// </summary>
        public void AddContentType()
        {
            _query.AddToQuery(QueryStrings.ContentType);
        }

        /// <summary>
        /// Adds the length of the content that is sent to the server.
        /// </summary>
        /// <param name="content">Content that is sent to the server.</param>
        public void AddContentLength(string content)
        {
            _query.AddToQuery(QueryStrings.ContentLength + content.Length.ToString()
                + QueryStrings.NewLine + QueryStrings.NewLine);
        }

        /// <summary>
        /// Adds content that is sent to the server.
        /// </summary>
        /// <param name="content">Content that is sent to the server.</param>
        public void AddContent(string content)
        {
            _query.AddToQuery(content);
        }

        /// <summary>
        /// Adds the end of the query string.
        /// </summary>
        public void AddEndOfQuery()
        {
            _query.AddToQuery(QueryStrings.EndOfQuery);
        }

        /// <summary>
        /// Returns the "constructed" string.
        /// </summary>
        /// <returns>Query string.</returns>
        public Query GetQuery()
        {
            var query = _query;

            Reset();

            return query;
        }
    }
}
