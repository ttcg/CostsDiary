namespace CostsDiary.Business.Entities.Mapping
{
    public static class CostTypeMapping
    {
        public static Data.Dto.CostType ToDto(this CostType costType)
        {
            return new Data.Dto.CostType
            {
                CostTypeDescription = costType.CostTypeDescription,
                CostTypeId = costType.CostTypeId
            };
        }

        public static CostType ToEntity(this Data.Dto.CostType costType)
        {
            return new CostType
            {
                CostTypeDescription = costType.CostTypeDescription,
                CostTypeId = costType.CostTypeId
            };
        }
    }
}
