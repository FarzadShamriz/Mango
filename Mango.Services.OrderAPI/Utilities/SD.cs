namespace Mango.Services.OrderAPI.Utilities
{
    public class SD
    {
        public enum OrderStatus
        {
            Pending = 0,
            Approved = 1,
            ReadyForPickup = 2,
            Completed = 3,
            Refunded = 4,
            Cancelled = 5
        }

        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";

    }
}
