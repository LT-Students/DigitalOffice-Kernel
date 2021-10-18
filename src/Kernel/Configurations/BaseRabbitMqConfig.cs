using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Kernel.AccessValidatorEngine.Requests;
using LT.DigitalOffice.Kernel.Attributes;

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
        public string Username { get; init; }
        public string Password { get; init; }

        [AutoInjectRequest(typeof(ICheckUserIsAdminRequest))]
        public string CheckUserIsAdminEndpoint { get; init; }

        [AutoInjectRequest(typeof(ICheckUserRightsRequest))]
        public string CheckUserRightsEndpoint { get; init; }

        [AutoInjectRequest(typeof(ICheckTokenRequest))]
        public string ValidateTokenEndpoint { get; init; }
    }
}
