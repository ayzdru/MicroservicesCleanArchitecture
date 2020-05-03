using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Services.Catalog.API.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }
        public User CreatedByUser { get; protected set; }
        public Guid? CreatedByUserId { get; protected set; }
        public DateTime Created { get; protected set; }
        public User LastModifiedByUser { get; protected set; }
        public Guid? LastModifiedByUserId { get; protected set; }
        public DateTime? LastModified { get; protected set; }
        public byte[] RowVersion { get; protected set; }
        public void Create(Guid? createdByUserId)
        {
            CreatedByUserId = createdByUserId;
            Created = DateTime.Now;
        }
        public void Update(Guid? lastModifiedByUserId)
        {
            if (lastModifiedByUserId.HasValue)
            {
                LastModifiedByUserId = lastModifiedByUserId;
                LastModified = DateTime.Now;
            }
        }
    }
}
