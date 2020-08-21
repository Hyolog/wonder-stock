using mvvm_netstandard.ComponentModel;
using mvvm_netstandard.Windows.Input;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Windows.Input;
using System.Xml;
using WonderStock.Models;

namespace WonderStock.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChangedViewModel
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

        public ICommand AddCommand { get; set; }

        public MainWindowViewModel()
        {
            AddCommand = new RelayCommand(CanExecuteAdd, ExecuteAdd);
        }

        private void ExecuteAdd(object obj)
        {
            var streamReader = GetStreamReaderFromUrl("http://asp1.krx.co.kr/servlet/krx.asp.XMLSise?code=035420");
            DeleteFirstLine(streamReader);

            var xml = new XmlDocument();
            xml.Load(streamReader);

            var xmlNodes = xml.GetElementsByTagName("TBL_StockInfo");

            foreach (XmlNode xmlNode in xmlNodes)
            {
                Stocks.Add(new StockItem()
                {
                    Name = xmlNode.Attributes["JongName"].Value,
                    Price =xmlNode.Attributes["CurJuka"].Value
                });
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

        private bool CanExecuteAdd(object obj)
        {
            return true;
        }
    }
}
