using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using WonderStock.Models;

namespace WonderStock.Database
{
    public class StockDatabase
    {
        readonly SQLiteAsyncConnection database;

        public StockDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Stock>().Wait();
        }

        public Task<List<Stock>> GetStocksAsync()
        {
            return database.Table<Stock>().ToListAsync();
        }

        public Task<int> InsertStocksAsync(Stock stock)
        {
            return database.InsertAsync(stock);
        }

        public Task<int> DeleteNoteAsync(Stock stock)
        {
            return database.DeleteAsync(stock);
        }
    }
}
