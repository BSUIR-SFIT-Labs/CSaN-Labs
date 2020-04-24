using System;
using System.Windows;
using System.Windows.Documents;
using HttpLib;
using HttpLib.Query;

namespace HttpClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ClientRequestSeparator = "Client request:\n";
        private const string ServerResponseSeparator = "Server response:\n";
        private const string EndSeparator = "End\n";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Cbi_Selected(object sender, RoutedEventArgs e)
        {
            if (RtbRequestBody != null)
            {
                RtbRequestBody.Document.Blocks.Clear();
                RtbRequestBody.IsReadOnly = true;
            }
        }

        private void CbiPost_Selected(object sender, RoutedEventArgs e)
        {
            RtbRequestBody.IsReadOnly = false;
        }

        private void BtnSendRequest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var httpClient = new HttpClient(TbHost.Text);

                var director = new Director();
                var queryBuilder = new QueryBuilder();
                director.Builder = queryBuilder;

                string path = string.IsNullOrEmpty(TbPath.Text) ? " / " : $" {TbPath.Text} ";
                string query;
                if (CbRequestType.Text == "POST")
                {
                    string requestContent = new TextRange(RtbRequestBody.Document.ContentStart,
                        RtbRequestBody.Document.ContentEnd).Text.Trim();

                    director.BuildPostQuery(path, TbHost.Text, requestContent);
                    query = queryBuilder.GetQuery().GetStringQuery();
                }
                else
                {
                    director.BuildQueryWithoutBody(CbRequestType.Text, path, TbHost.Text);
                    query = queryBuilder.GetQuery().GetStringQuery();
                }

                string response = httpClient.SendRequest(query);

                TextRange textRange = new TextRange(RtbLog.Document.ContentEnd,
                    RtbLog.Document.ContentEnd)
                {
                    Text = ClientRequestSeparator.ToUpper() + query
                        + ServerResponseSeparator.ToUpper() + response + EndSeparator.ToUpper()
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnClearLog_Click(object sender, RoutedEventArgs e)
        {
            var textRange = new TextRange(RtbLog.Document.ContentStart, RtbLog.Document.ContentEnd)
            {
                Text = string.Empty
            };
        }
    }
}
