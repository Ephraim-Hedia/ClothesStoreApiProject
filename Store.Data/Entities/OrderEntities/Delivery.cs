namespace Store.Data.Entities.OrderEntities
{
    public class Delivery
    {
        public static int GetShippingPrice (string city)
        {
            switch(city)
            {
                case "Cairo":
                    return 55;
                default:
                    throw new NotImplementedException ();
            }
        }
    }
}
