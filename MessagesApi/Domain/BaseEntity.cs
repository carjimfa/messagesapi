using System;

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
        
        
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
    }
}