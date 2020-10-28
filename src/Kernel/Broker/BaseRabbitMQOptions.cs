using LT.DigitalOffice.Kernel.AccessValidatorEngine;

namespace LT.DigitalOffice.Kernel.Broker
{
    /// <summary>
    /// Base configuration class for RabbitMQ.
    /// </summary>
    public class BaseRabbitMqOptions
    {
        /// <summary>
        /// Section name.
        /// </summary>
        public const string RabbitMqSectionName = "RabbitMQ";
        /// <summary>
        /// RabbitMQ host address.
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// RabbitMQ username.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// RabbitMQ password.
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// RabbitMQ UserService channel url used by <see cref="AccessValidator"/>.
        /// </summary>
        public string AccessValidatorUserServiceURL { get; set; }
        /// <summary>
        /// RabbitMQ CheckRightsService channel url used by <see cref="AccessValidator"/>.
        /// </summary>
        public string AccessValidatorCheckRightsServiceURL { get; set; }
    }
}