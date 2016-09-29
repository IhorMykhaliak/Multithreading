using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;

namespace M_Multithreading
{
    public partial class Form1 : Form
    {
        private string[] urls;
        private HttpClient client;
        private CancellationTokenSource tokenSource;
        private bool processing = false;

        public Form1()
        {
            InitializeComponent();

            urls = ConfigurationManager.AppSettings["urls"].Replace(" ", "").Split(new[] { "\r\n," }, StringSplitOptions.RemoveEmptyEntries);
            client = new HttpClient();
            tokenSource = new CancellationTokenSource();

            tbxUrls.Text = string.Join(Environment.NewLine, urls);
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (processing)
            {
                tokenSource.Cancel();
                rtbStatus.Text += "Cancallation requested";
                btnStart.Text = "Start";
            }
            else
            {
                processing = true;
                rtbStatus.Text = "";
                btnStart.Text = "Stop";
                foreach (var url in urls)
                {
                    //var t = new Task(() => GetData(url), tokenSource.Token);
                    //t.Start();

                    GetDataAsync(url, tokenSource.Token);

                    //try
                    //{
                    //    rtbStatus.Text += "\nProcessing " + url;

                    //    var response = await client.GetAsync(url, tokenSource.Token);

                    //    rtbStatus.Text += "\nParsing response from " + url;

                    //    var result = response.Content.ReadAsStringAsync();

                    //    ParseResult(result.Result);
                    //}
                    //catch (TaskCanceledException)
                    //{
                    //    rtbStatus.Text += "\nTask was cancaleld";
                    //}
                    //finally
                    //{
                    //    btnStart.Text = "Start";
                    //}
                }
            }
        }

        private void GetData(string url)
        {
            rtbStatus.Text += "\nProcessing " + url;

            var response = client.GetAsync(url).Result;

            rtbStatus.Text += "\nParsing response from " + url;

            var result = response.Content.ReadAsStringAsync().Result;

            ParseResult(result);
        }


        private async void GetDataAsync(string url, CancellationToken token)
        {
            try
            {
                rtbStatus.Text += "\nProcessing " + url;

                var response = await client.GetAsync(url, token);

                rtbStatus.Text += "\nParsing response from " + url;

                var result = await response.Content.ReadAsStringAsync();

                ParseResult(result);
            }
            catch (TaskCanceledException)
            {
                rtbStatus.Text += "\nTask was cancaleld";
            }
        }

        private void ParseResult(string content)
        {

        }

    }
}
