using MongoDB.Driver;

namespace Common.Utils.Extensions
{
    public static class MongoUrlMixins
    {
        /// <summary>
        /// Return a public string for the mongo url (hides the password).
        /// </summary>
        public static string ToPublicString(this MongoUrl url)
        {
            var builder = new MongoUrlBuilder(url.Url);
            if (!string.IsNullOrEmpty(builder.Password))
                builder.Password = "xxxx";
            if (!string.IsNullOrEmpty(builder.Username))
                builder.Username = "xxxx";

            return builder.ToString();
        }
    }
}