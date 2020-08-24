using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WonderStock.Models;
using WonderStock.ViewModels;

namespace WonderStock.Views
{
    public partial class SearchView : Page
    {
        public SearchView()
        {
            InitializeComponent();
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            var mainWindowViewModel = Application.Current.MainWindow.DataContext as MainWindowViewModel;

            foreach (var stock in StocksListView.SelectedItems.Cast<StockItem>())
            {
                mainWindowViewModel.Stocks.Add(stock);
            }
        }

        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text) || SearchTextBox.Text.Equals("검색어 입력"))
            {
                MessageBox.Show("검색어를 입력해주세요.");
            }
        }
    }
}
