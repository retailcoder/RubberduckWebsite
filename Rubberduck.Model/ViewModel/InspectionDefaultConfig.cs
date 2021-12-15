namespace Rubberduck.Model.ViewModel
{
    public class InspectionDefaultConfig
    {
        public static InspectionDefaultConfig FromDTO(DTO.InspectionDefaultConfig dto) => new(dto);
        public static DTO.InspectionDefaultConfig ToDTO(InspectionDefaultConfig entity) => new()
        {
            InspectionName = entity.InspectionName,
            InspectionType = entity.InspectionType,
            DefaultSeverity = entity.DefaultSeverity
        };

        internal InspectionDefaultConfig(DTO.InspectionDefaultConfig dto)
        {
            InspectionName = dto.InspectionName;
            InspectionType = dto.InspectionType;
            DefaultSeverity = dto.DefaultSeverity;
        }

        public string InspectionName { get; }
        public string InspectionType { get; }
        public string DefaultSeverity { get; }
    }
}
