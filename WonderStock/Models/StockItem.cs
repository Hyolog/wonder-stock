namespace WonderStock.Models
{
    public class StockItem
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Price { get; set; }
        public int PreviousPrice { get; set; }
        public int Volume { get; set; }

        public int AmountOfChange
        {
            get
            {
                return Price - PreviousPrice;
            }
        }

        public double FluctuationRate
        {
            get
            {
                return AmountOfChange / Price;
            }
        }
    }
}
