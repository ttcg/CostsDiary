using CostsDiary.Domain.Entities;

namespace CostsDiary.Web.Models.Mapping
{
    public static class CostTypeMapping
    {
        public static CostTypeModel ToModel(this CostType costType)
        {
            return new CostTypeModel
            {
                CostTypeDescription = costType.CostTypeDescription,
                CostTypeId = costType.CostTypeId
            };
        }

        public static CostType ToEntity(this CostTypeModel model)
        {
            return new CostType
            {
                CostTypeDescription = model.CostTypeDescription,
                CostTypeId = model.CostTypeId
            };
        }
    }
}
