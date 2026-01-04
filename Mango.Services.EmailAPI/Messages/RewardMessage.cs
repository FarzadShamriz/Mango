namespace Mango.Services.EmailAPI.Messages
{
    public class RewardMessage
    {
        public string UserId { get; set; }
        public int RewardsActity { get; set; }
        public int OrderId { get; set; }
    }
}
