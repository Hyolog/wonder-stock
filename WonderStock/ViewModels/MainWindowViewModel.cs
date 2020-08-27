using mvvm_netstandard.ComponentModel;
using mvvm_netstandard.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WonderStock.Models;

namespace WonderStock.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyChangedViewModel
    {
        private ObservableCollection<Stock> stocks = new ObservableCollection<Stock>();
        public ObservableCollection<Stock> Stocks
        {
            get { return stocks; }
            set
            {
                SetValueWithNotify(ref stocks, value);
            }
        }

        public bool GetStocks()
        {
            try
            {
                Stocks = new ObservableCollection<Stock>(App.Database.GetStocksAsync().Result);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
