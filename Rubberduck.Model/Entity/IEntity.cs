﻿using System;

namespace Rubberduck.Model.Entity
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
        /// An object representing the unique key for the entity.
        /// </summary>
        object Key();
    }
}
