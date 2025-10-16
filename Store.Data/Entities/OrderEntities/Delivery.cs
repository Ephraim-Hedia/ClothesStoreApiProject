namespace Store.Data.Entities.OrderEntities
{
    public class Delivery
    {
        public static int GetShippingPrice (string city)
        {
            return city switch
            {
                "Cairo" => 55,
                _ => 70 // default price or 0 if you prefer
            };
        }
    }
}
