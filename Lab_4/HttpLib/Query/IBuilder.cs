namespace HttpLib.Query
{
    /// <summary>
    /// A builder interface that collects a query string in parts.
    /// </summary>
    public interface IBuilder
    {
        /// <summary>
        /// Adds a request method: GET, HEAD, or POST.
        /// </summary>
        /// <param name="method">Request method.</param>
        void AddMethod(string method);

        /// <summary>
        /// Adds the path where the request is to be executed.
        /// </summary>
        /// <param name="path">The path to the resource.</param>
        void AddPath(string path);

        /// <summary>
        /// Adds a protocol version (HTTP v1.0).
        /// </summary>
        void AddVersion();

        /// <summary>
        /// Adds a host.
        /// </summary>
        /// <param name="host">Host.</param>
        void AddHost(string host);

        /// <summary>
        /// Adds information about the client.
        /// </summary>
        void AddUserAgent();

        /// <summary>
        /// Adds information about files that the client can accept.
        /// </summary>
        void AddAccept();

        /// <summary>
        /// Adds the type of content that is sent to the server.
        /// </summary>
        void AddContentType();

        /// <summary>
        /// Adds the length of the content that is sent to the server.
        /// </summary>
        /// <param name="content">Content that is sent to the server.</param>
        void AddContentLength(string content);

        /// <summary>
        /// Adds content that is sent to the server.
        /// </summary>
        /// <param name="content">Content that is sent to the server.</param>
        void AddContent(string content);

        /// <summary>
        /// Adds the end of the query string.
        /// </summary>
        void AddEndOfQuery();
    }
}
