using System;

namespace Rubberduck.Model.Internal
{
    /// <summary>
    /// An immutable object representing a data entity.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// The internal primary key.
        /// </summary>
        int Id { get; }
        /// <summary>
        /// The timestamp when the entity was first created.
        /// </summary>
        DateTime DateInserted { get; }
        /// <summary>
        /// The timestamp when the entity was last updated.
        /// </summary>
        DateTime? DateUpdated { get; }
    }

    public abstract class EntityBase : IEntity
    {
        protected EntityBase(int id, DateTime inserted, DateTime? updated)
        {
            Id = id;
            DateInserted = inserted;
            DateUpdated = updated;
        }

        public int Id { get; }

        public DateTime DateInserted { get; }

        public DateTime? DateUpdated { get; }
    }
}
