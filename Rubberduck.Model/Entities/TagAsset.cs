using Rubberduck.Model.Abstract;

namespace Rubberduck.Model.Entities
{
    public class TagAsset : Entity
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        public string DownloadUrl { get; set; }
    }
}
