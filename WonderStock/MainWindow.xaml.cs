using System.Windows;
using System.Windows.Controls;
using WonderStock.Models;
using WonderStock.ViewModels;
using WonderStock.Views;

namespace WonderStock
{
    public partial class MainWindow : Window
    {
        // TODO: NavigationWindow로 변경
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

        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            var searchView = new SearchView();

            searchView.Show();
        }
    }
}
