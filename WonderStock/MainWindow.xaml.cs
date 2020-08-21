using System.Windows;
using System.Windows.Controls;
using WonderStock.Models;
using WonderStock.ViewModels;

namespace WonderStock
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SearchResultListView_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var stock = (StockItem)((sender as ListView).SelectedItem);

            var viewModel = DataContext as MainWindowViewModel;

            viewModel.Stocks.Add(stock);
        }
    }
}
