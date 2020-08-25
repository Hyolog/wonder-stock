using System.Collections.ObjectModel;
using System.Windows.Input;
using WonderStock.Models;
using mvvm_netstandard.ComponentModel;
using mvvm_netstandard.Windows.Input;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Net.Http;
using System.Linq;
using System.IO;
using System.Text;
using NLog;
using System;
using System.Xml;

namespace WonderStock.ViewModels
{
    public class SearchViewModel : NotifyPropertyChangedViewModel
    {
        private ObservableCollection<StockItem> stocks = new ObservableCollection<StockItem>();
        public ObservableCollection<StockItem> Stocks
        {
            get { return stocks; }
            set
            {
                SetValueWithNotify(ref stocks, value);
            }
        }

        private Dictionary<string, string> codeAndNamePairs;
        public Dictionary<string, string> CodeAndNamePairs
        {
            get
            {
                if (codeAndNamePairs == null)
                {
                    var htmlString = GetString(kindListedCompanyDownloadUrl);

                    codeAndNamePairs = GetDictionaryFromHtmlString(htmlString);
                }

                return codeAndNamePairs;
            }
            set { codeAndNamePairs = value; }
        }

        private Logger logger = LogManager.GetCurrentClassLogger();
        // KRT Xml서비스 링크: https://kasp.krx.co.kr/contents/02/02010000/ASP02010000.jsp
        const string krtXmlServiceUrl = "http://asp1.krx.co.kr/servlet/krx.asp.XMLSise";
        // 상장기업목록 다운로드 링크
        const string kindListedCompanyDownloadUrl = "http://kind.krx.co.kr/corpgeneral/corpList.do?method=download&searchType=13";

        public ICommand SearchCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public SearchViewModel()
        {
            SearchCommand = new RelayCommand(CanExecuteSearch, ExecuteSearch);
            DeleteCommand = new RelayCommand(CanExecuteDelete, ExecuteDelete);
            // 이거 미리 로드하는게 맞는거야?
            var htmlString = GetString(kindListedCompanyDownloadUrl);

            codeAndNamePairs = GetDictionaryFromHtmlString(htmlString);
        }

        private bool CanExecuteSearch(object obj)
        {
            return true;
        }

        private void ExecuteSearch(object obj)
        {
            string searchText = obj as string;

            if (string.IsNullOrEmpty(searchText))
            {
                return;
            }

            var codes = CodeAndNamePairs.Where(d => d.Value.Contains(searchText)).Select(d => d.Key);

            foreach (var code in codes)
            {
                if (Stocks.Any(d => d.Code == code))
                {
                    continue;
                }

                Stocks.Add(GetStock(code));
            }
        }

        private bool CanExecuteDelete(object obj)
        {
            return true;
        }

        private void ExecuteDelete(object obj)
        {
            Stocks.Clear();
        }

        private string GetString(string url)
        {
            using (var client = new HttpClient())
            {
                return client.GetStringAsync(url).Result;
            }
        }

        // TODO: 코드리펙토링
        private Dictionary<string, string> GetDictionaryFromHtmlString(string htmlString)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlString);

            var result = doc.DocumentNode.SelectNodes("//table")
                .Descendants("tr")
                .Skip(1)
                .Where(tr => tr.Elements("td").Count() > 1)
                .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).Take(2).ToList())
                .ToList();

            var codeAndNameDic = new Dictionary<string, string>();

            foreach (var item in result)
            {
                codeAndNameDic.Add(item[1], item[0]);
            }

            return codeAndNameDic;
        }

        private StockItem GetStock(string code)
        {
            try
            {
                var url = new StringBuilder(krtXmlServiceUrl);
                url.Append($"?code={code}");

                var streamReader = GetStreamReaderFromUrl(url.ToString());
                DeleteFirstLine(streamReader);

                var xml = new XmlDocument();
                xml.Load(streamReader);

                var xmlNodes = xml.GetElementsByTagName("TBL_StockInfo");

                if (xmlNodes == null || xmlNodes.Count == 0)
                    return null;

                return new StockItem()
                {
                    Name = xmlNodes[0].Attributes["JongName"].Value,
                    Code = code,
                    Price = int.Parse(xmlNodes[0].Attributes["CurJuka"].Value.Replace(",", "")),
                    PreviousPrice = int.Parse(xmlNodes[0].Attributes["PrevJuka"].Value.Replace(",", "")),
                    Volume = int.Parse(xmlNodes[0].Attributes["Volume"].Value.Replace(",", "")),
                    Fluctuation = int.Parse(xmlNodes[0].Attributes["DungRak"].Value)
                };
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return null;
                // TODO: 팝업창 띄울까?
            }
        }

        private StreamReader GetStreamReaderFromUrl(string url)
        {
            return new StreamReader(new HttpClient().GetStreamAsync(url).Result);
        }

        /// <summary>
        /// KRX response(xml format) include empty line before xml declaration
        /// </summary>
        private void DeleteFirstLine(StreamReader streamReader)
        {
            streamReader.ReadLine();
        }
    }
}
