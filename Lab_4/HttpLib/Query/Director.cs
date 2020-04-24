namespace HttpLib.Query
{
    /// <summary>
    /// Manages the builder.
    /// </summary>
    public class Director
    {
        private IBuilder _builder;

        public IBuilder Builder
        {
            set { _builder = value; }
        }

        /// <summary>
        /// Composes a query string without passing content to the server.
        /// </summary>
        /// <param name="method">Request method (GET or HEAD).</param>
        /// <param name="path">The path to the resource.</param>
        /// <param name="host">Host.</param>
        public void BuildQueryWithoutBody(string method, string path, string host)
        {
            _builder.AddMethod(method);
            _builder.AddPath(path);
            _builder.AddVersion();
            _builder.AddHost(host);
            _builder.AddUserAgent();
            _builder.AddAccept();
            _builder.AddEndOfQuery();
        }

        /// <summary>
        /// Composes a query string with the transfer of content to the server.
        /// </summary>
        /// <param name="path">The path to the resource.</param>
        /// <param name="host">Host.</param>
        /// <param name="content">Content that is sent to the server.</param>
        public void BuildPostQuery(string path, string host, string content)
        {
            _builder.AddMethod("POST");
            _builder.AddPath(path);
            _builder.AddVersion();
            _builder.AddHost(host);
            _builder.AddUserAgent();
            _builder.AddAccept();
            _builder.AddContentType();
            _builder.AddContentLength(content);
            _builder.AddContent(content);
            _builder.AddEndOfQuery();
        }
    }
}
