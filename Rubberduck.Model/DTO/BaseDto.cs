using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubberduck.Model.DTO
{
    /// <summary>
    /// A mutable object representing a database record.
    /// </summary>
    public abstract class BaseDto
    {
        public int Id { get; set; }
        public DateTime DateInserted { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
