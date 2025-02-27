using System.ComponentModel.DataAnnotations;

namespace Stock.API.DTOS.Stock
{
    public class CreateStockDTO
    {
        public int ProductId { get; set; }

        public int Count { get; set; }  //ürüne ait stok bilgisi.
    }
}
