using MongoDB.Driver;

namespace Common.Utils.Settings
{
    /// <summary>
    /// Options for database initialization.
    /// </summary>
    public class DatabaseInitializeOptions
    {
        // /// <summary>
        // /// Use these client settings when connecting.
        // /// </summary>
        public MongoClientSettings ConnectionSettings { get; set; }
        
        /// <summary>
        /// The database name to use
        /// </summary>
        public string DatabaseName { get; set; }
    }
}