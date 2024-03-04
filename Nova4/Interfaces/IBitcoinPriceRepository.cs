using Nova4.Models;

namespace Nova4.Interfaces
{
    public interface IBitcoinPriceRepository
    {
        public Task<IEnumerable<BitcoinPrice>> GetAllPrices();
        public Task<IEnumerable<BitcoinPrice>> GetAllPricesForCurrencyChange();
        public Task<BitcoinPrice> GetPriceById(int id);
        public void RemoveHistoryCash();
        public Task AddPrice(BitcoinPrice price);
        public Task<decimal> ConvertToCurrency(decimal amount, string sourceCurrency, string targetCurrency);
        public Task SaveChanges(); 
    }
}
