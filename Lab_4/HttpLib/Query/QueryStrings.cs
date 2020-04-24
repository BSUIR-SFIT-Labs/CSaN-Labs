namespace HttpLib.Query
{
    /// <summary>
    /// Contains the component lines of the request to the server.
    /// </summary>
    class QueryStrings
    {
        public const string NewLine = "\n";
        public const string HttpVersion = "HTTP/1.0\n";
        public const string Host = "Host: www.";
        public const string UserAgent = "User-Agent: Mozilla/4.05 (WinNT; 1)\n";
        public const string AcceptedFiles = "Accept: image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, */*\n";
        public const string ContentType = "Content-type: text/html\n";
        public const string ContentLength = "Content-length: ";
        public const string EndOfQuery = "\n\n";
    }
}
