using System;
using System.Linq;
using System.Windows;
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

            Loaded += MainWindowLoaded;
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;

            if (!viewModel.GetStocks())
            {
                MessageBox.Show("이전 정보를 불러오는데 실패 했습니다.");
            }
        }

        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            var searchView = new SearchView();

            searchView.Show();
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;

            foreach (var stock in StockListView.SelectedItems.Cast<Stock>().ToList())
            {
                viewModel.Stocks.Remove(stock);
                App.Database.DeleteNoteAsync(stock).Wait();
            }
        }
    }
}
