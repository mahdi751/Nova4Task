using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nova4.Enums;
using Nova4.Interfaces;
using Nova4.Models;

namespace BitcoinPriceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BitcoinPriceController : ControllerBase
    {
        private readonly IBitcoinPriceRepository _repository;
        private readonly ILogger<BitcoinPriceController> _logger;
        private readonly IHttpClientFactory _clientFactory; 

        public BitcoinPriceController(IBitcoinPriceRepository repository, ILogger<BitcoinPriceController> logger, IHttpClientFactory clientFactory)
        {
            _repository = repository;
            _logger = logger;
            _clientFactory = clientFactory; 
        }

        [HttpGet("Sources")]
        public IActionResult GetSources()
        {
            var sources = Enum.GetNames(typeof(PriceSource));
            return Ok(sources);
        }

        [HttpGet("Price/{source}")]
        public async Task<IActionResult> GetBitcoinPrice(string source)
        {
            try
            {
                var price = await FetchBitcoinPrice(source);
                 
                await _repository.AddPrice(price);

                _repository.RemoveHistoryCash();

                return Ok(price);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch Bitcoin price");
                return StatusCode(500, "Failed to fetch Bitcoin price");
            }
        }

        [HttpGet("History")]
        public async Task<IActionResult> GetPriceHistory()
        {
            try
            {
                var prices = await _repository.GetAllPrices();
                return Ok(prices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve Bitcoin price history");
                return StatusCode(500, "Failed to retrieve Bitcoin price history");
            }
        }


        [HttpGet("Prices/{currency}")]
        public async Task<IActionResult> GetAllBitcoinPricesInCurrency(string currency)
        {
            try
            { 
                var bitcoinPrices = await _repository.GetAllPricesForCurrencyChange();
                 
                foreach (var price in bitcoinPrices)
                {
                    price.Price = await _repository.ConvertToCurrency(price.Price, "USD", currency);
                }

                return Ok(bitcoinPrices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch Bitcoin prices");
                return StatusCode(500, "Failed to fetch Bitcoin prices");
            }
        }



        private async Task<BitcoinPrice> FetchBitcoinPrice(string source)
        {
            var apiUrl = source switch
            {
                "Bitstamp" => "https://www.bitstamp.net/api/v2/ticker/btcusd/",
                "Bitfinex" => "https://api.bitfinex.com/v1/pubticker/btcusd",
                _ => throw new ArgumentException("Invalid price source")
            };

            var client = _clientFactory.CreateClient();

            var response = await client.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to fetch Bitcoin price from {source}. Status code: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
             
            var result = JsonConvert.DeserializeObject<BitcoinPriceData>(content);
             
            var price = new BitcoinPrice
            {
                Price = decimal.Parse(result.Last),
                Timestamp = DateTime.UtcNow,
                Source = source
            };

            return price;
        }

    }
}
