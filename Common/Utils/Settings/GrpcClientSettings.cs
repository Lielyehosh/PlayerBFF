namespace Common.Utils.Settings
{
    public class GrpcClientSettings
    {
        /// <summary>
        /// Https2 address
        /// </summary>
        public string Address { get; set; }
        
        /// <summary>
        /// Ignore SSL validation when connecting to Micro Services
        /// </summary>
        public bool IgnoreSsl { get; set; }

    }
}