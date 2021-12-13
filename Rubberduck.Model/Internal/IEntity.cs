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
    }
}
