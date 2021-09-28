using System;

namespace BFF.Service.Controllers
{
    public class TagAttribute : Attribute
    {
        public TagAttribute(params string[] tags)
        {
            Tags = tags;
        }
        public string[] Tags { get; set; }
    }
}