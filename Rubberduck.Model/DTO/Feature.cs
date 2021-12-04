using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubberduck.Model.DTO
{
    [Table("Features")]
    public class Feature : BaseDto
    {
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ContentUrl { get; set; }
        public bool IsNew { get; set; }
        public bool IsHidden { get; set; }
        public int SortOrder { get; set; }
        public string XmlDocSource { get; set; }
    }
}
