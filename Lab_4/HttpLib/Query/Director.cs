namespace HttpLib.Query
{
    public class Director
    {
        private IBuilder _builder;

        public IBuilder Builder
        {
            set { _builder = value; }
        }

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
