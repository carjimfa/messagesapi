using System;
using Newtonsoft.Json;

namespace MessagesApi.Domain
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid();
            IsDeleted = false;
            CreationDate = DateTime.UtcNow;
            ModificationDate = DateTime.UtcNow;
        }
        
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "isDeleted")]
        public bool IsDeleted { get; set; }
        [JsonProperty(PropertyName = "creationDate")]
        public DateTime CreationDate { get; set; }
        [JsonProperty(PropertyName = "modificationDate")]
        public DateTime ModificationDate { get; set; }
    }
}