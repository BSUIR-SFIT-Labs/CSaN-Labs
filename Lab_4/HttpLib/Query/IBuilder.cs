using System;
using System.Collections.Generic;
using System.Text;

namespace HttpLib.Query
{
    public interface IBuilder
    {
        void AddMethod(string method);

        void AddPath(string path);

        void AddVersion();

        void AddHost(string host);

        void AddUserAgent();

        void AddAccept();

        void AddContentType();

        void AddContentLength(string content);

        void AddContent(string content);

        void AddEndOfQuery();
    }
}
