using mvvm_netstandard.ComponentModel;
using mvvm_netstandard.Windows.Input;
using NLog;
using System.Collections.ObjectModel;
using System.Windows.Input;
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
    }
}
