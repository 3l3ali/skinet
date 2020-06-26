using System.Collections.Generic;

namespace Core.Entities
{
    public class CostumerBasket
    {
        public string Id { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();

        public CostumerBasket()
        {
        }
        public CostumerBasket(string id)
        {
            Id = id;
        }
    }
}