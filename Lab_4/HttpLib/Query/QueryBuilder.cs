using System;

namespace HttpLib.Query
{
    public class QueryBuilder : IBuilder
    {
        private Query _query;

        public QueryBuilder()
        {
            Reset();
        }

        public void Reset()
        {
            _query = new Query();
        }

        public void AddMethod(string method)
        {
            _query.AddToQuery(method);
        }

        public void AddPath(string path)
        {
            _query.AddToQuery(path);
        }

        public void AddVersion()
        {
            _query.AddToQuery(QueryStrings.HttpVersion);
        }

        public void AddHost(string host)
        {
            _query.AddToQuery(QueryStrings.Host + host + QueryStrings.NewLine);
        }

        public void AddUserAgent()
        {
            _query.AddToQuery(QueryStrings.UserAgent);
        }

        public void AddAccept()
        {
            _query.AddToQuery(QueryStrings.AcceptedFiles);
        }

        public void AddContentType()
        {
            _query.AddToQuery(QueryStrings.ContentType);
        }

        public void AddContentLength(string content)
        {
            _query.AddToQuery(QueryStrings.ContentLength + content.Length.ToString()
                + QueryStrings.NewLine + QueryStrings.NewLine);
        }

        public void AddContent(string content)
        {
            _query.AddToQuery(content);
        }

        public void AddEndOfQuery()
        {
            _query.AddToQuery(QueryStrings.EndOfQuery);
        }

        public Query GetQuery()
        {
            var query = _query;

            Reset();

            return query;
        }
    }
}
