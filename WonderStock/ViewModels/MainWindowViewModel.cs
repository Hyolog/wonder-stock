using mvvm_netstandard.ComponentModel;
using mvvm_netstandard.Windows.Input;
using System;
using System.Collections.Generic;
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

        public ICommand AddCommand { get; set; }

        public MainWindowViewModel()
        {
            AddCommand = new RelayCommand(CanExecuteAdd, ExecuteAdd);
        }

        private void ExecuteAdd(object obj)
        {
            Stocks.Add(new StockItem() { Name = "카카오", Price = 360000 });
        }

        private bool CanExecuteAdd(object obj)
        {
            return true;
        }
    }
}
