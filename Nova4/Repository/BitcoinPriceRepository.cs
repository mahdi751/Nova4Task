using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Nova4.Data;
using Nova4.Interfaces;
using Nova4.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nova4.Enums;

namespace Nova4.Repository
{
    public class BitcoinPriceRepository : IBitcoinPriceRepository
    {
        private readonly DataContext _context;
        private readonly IMemoryCache _cache;
        private readonly IHttpClientFactory _clientFactory;

        public BitcoinPriceRepository(DataContext context, IMemoryCache cache, IHttpClientFactory clientFactory)
        {
            _context = context;
            _cache = cache;
            _clientFactory = clientFactory;
        } 

        public async Task<IEnumerable<BitcoinPrice>> GetAllPrices()
        {
            if (!_cache.TryGetValue("BitcoinPriceHistory", out IEnumerable<BitcoinPrice> prices))
            {
                prices = await _context.BitcoinPrices.ToListAsync(); 
                _cache.Set("BitcoinPriceHistory", prices, TimeSpan.FromDays(1));
            }

            return prices;
        }

        public async Task<IEnumerable<BitcoinPrice>> GetAllPricesForCurrencyChange()
        { 
            var prices = await _context.BitcoinPrices.ToListAsync(); 
            return prices;
        }
        
        public void RemoveHistoryCash()
        {
            _cache.Remove("BitcoinPriceHistory");
        }

        public async Task<BitcoinPrice> GetPriceById(int id)
        {
            return await _context.BitcoinPrices.FindAsync(id);
        }

        public async Task AddPrice(BitcoinPrice price)
        {
            _context.BitcoinPrices.Add(price);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public Task<decimal> ConvertToCurrency(decimal amount, string sourceCurrency, string targetCurrency)
        {
            var conversionRates = new Dictionary<string, decimal>
            {
                { "USD", 1 },
                { "EUR", 0.82m },
                { "GBP", 0.72m } 
            };

            if (!conversionRates.ContainsKey(targetCurrency))
            {
                throw new ArgumentException("Target currency not supported.");
            }

            decimal convertedAmount = amount * conversionRates[targetCurrency];

            return Task.FromResult(convertedAmount);
        }


    }
}
