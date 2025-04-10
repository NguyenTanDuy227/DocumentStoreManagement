namespace DocumentStoreManagement.Services.MessageBroker
{
    /// <summary>
    /// RabbitMQ Producer interface
    /// </summary>
    public interface IRabbitMQProducer
    {
        /// <summary>
        /// Send order message to RabbitMQ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        public Task SendOrderMessageAsync<T>(T message);
    }
}
