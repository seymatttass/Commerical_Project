using System.ComponentModel.DataAnnotations;

namespace Stock.API.DTOS.Stock
{
    public class UpdateStockDTO
    {
        public int StockId { get; set; }

        public int ProductId { get; set; }

        public int Count { get; set; } 
    }
}
