using mvvm_netstandard.ComponentModel;
using mvvm_netstandard.Windows.Input;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using System.Xml;
using WonderStock.Models;

namespace WonderStock.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChangedViewModel
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        
        // KRT Xml서비스 링크: https://kasp.krx.co.kr/contents/02/02010000/ASP02010000.jsp
        const string krtXmlServiceUrl = "http://asp1.krx.co.kr/servlet/krx.asp.XMLSise";

        private ObservableCollection<StockItem> stocks = new ObservableCollection<StockItem>();
        public ObservableCollection<StockItem> Stocks
        {
            get { return stocks; }
            set
            {
                SetValueWithNotify(ref stocks, value);
            }
        }

        private ObservableCollection<StockItem> searchResult = new ObservableCollection<StockItem>();
        public ObservableCollection<StockItem> SearchResult
        {
            get { return searchResult; }
            set
            {
                SetValueWithNotify(ref searchResult, value);
            }
        }

        public ICommand SearchCommand { get; set; }
        public ICommand AddCommand { get; set; }

        public MainWindowViewModel()
        {
            SearchCommand = new RelayCommand(CanExecuteSearch, ExecuteSearch);
            AddCommand = new RelayCommand(CanExecuteAdd, ExecuteAdd);
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

        private bool CanExecuteAdd(object obj)
        {
            return true;
        }

        private void ExecuteAdd(object obj)
        {
            var stock = obj as StockItem;

            Stocks.Add(stock);
        }

        private bool CanExecuteSearch(object obj)
        {
            return true;
        }

        private void ExecuteSearch(object obj)
        {
            string searchText = obj as string;

            var codes = GetCodeAndNameDic().Where(d => d.Value.Contains(searchText)).Select(d => d.Key);
            
            foreach (var code in codes)
            {
                // TODO: 임시로 만든 SearchView 어떻게 적용(UI적으로)할지
                SearchResult.Add(GetStock(code));
            }
        }

        private Dictionary<string, string> GetCodeAndNameDic()
        {
            // Mock
            var result = new Dictionary<string, string>();
            result.Add("035720", "카카오");
            result.Add("035420", "NAVER");
            result.Add("006400", "삼성SDI");
            result.Add("009150", "삼성전기");

            // TODO: excel 읽어서 가져오도록 구현
            //http://kind.krx.co.kr/corpgeneral/corpList.do?method=download&searchType=13

            return result;
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
                };
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                return null;
                // TODO: 팝업창 띄울까?
            }
        }
    }
}
