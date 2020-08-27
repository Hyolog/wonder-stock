using System.Linq;
using System.Windows;
using System.Windows.Input;
using WonderStock.Models;
using WonderStock.ViewModels;

namespace WonderStock.Views
{
    public partial class SearchView : Window
    {
        public SearchView()
        {
            InitializeComponent();
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            var mainWindowViewModel = Application.Current.MainWindow.DataContext as MainWindowViewModel;

            foreach (var stock in StockListView.SelectedItems.Cast<Stock>())
            {
                if (mainWindowViewModel.Stocks.Any(d => d.Code == stock.Code))
                {
                    continue;
                }

                mainWindowViewModel.Stocks.Add(stock);
                App.Database. InsertStocksAsync(stock);
            }
        }

        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                MessageBox.Show("검색어를 입력해주세요.");
            }
        }

        private void SearchTextBoxKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchButtonClick(null, null);

                var viewModel = DataContext as SearchViewModel;
                viewModel.SearchCommand.Execute(SearchTextBox.Text);
            }
        }
    }
}
