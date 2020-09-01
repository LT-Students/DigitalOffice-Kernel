namespace LT.DigitalOffice.Kernel.Broker
{
    public class RabbitMQOptions
    {
        public const string RabbitMQ = "RabbitMQ";
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AccessValidatorUserServiceURL { get; set; }
        public string AccessValidatorCheckRightsServiceURL { get; set; }
    }
}