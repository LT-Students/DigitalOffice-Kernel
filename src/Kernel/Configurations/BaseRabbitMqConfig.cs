namespace LT.DigitalOffice.Kernel.Configurations
{
    /// <summary>
    /// Base configuration class for RabbitMQ.
    /// </summary>
    public class BaseRabbitMqConfig
    {
        public const string SectionName = "RabbitMQ";

        private const string RabbitMqProtocol = "rabbitmq";

        public string BaseUrl => $"{RabbitMqProtocol}://{Host}";

        public string Host { get; init; }

        public string CheckUserIsAdminEndpoint { get; init; }

        public string CheckUserRightsEndpoint { get; init; }

        public string ValidateTokenEndpoint { get; init; }
    }
}
