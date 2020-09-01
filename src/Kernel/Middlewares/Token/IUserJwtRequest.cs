namespace LT.DigitalOffice.Broker.Requests
{
    /// <summary>
    /// The DTO model is a binding the request internal model of consumer for RabbitMQ.
    /// </summary>
    public interface IUserJwtRequest
    {
        ///<value>User json web token.</value>
        string UserJwt { get; }
    }
}