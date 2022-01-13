using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubberduck.Model
{
    /// <summary>
    /// Encapsulates the inspection type and default severity setting override for an inspection.
    /// </summary>
    public class InspectionDefaultConfig
    {
        /// <summary>
        /// Get/sets the name (unique) of the inspection.
        /// </summary>
        public string InspectionName { get; set; }
        /// <summary>
        /// Gets/sets the type of inspection.
        /// </summary>
        public string InspectionType { get; set; }
        /// <summary>
        /// Gets/sets the default severity setting value.
        /// </summary>
        public string DefaultSeverity { get; set; }
    }
}
