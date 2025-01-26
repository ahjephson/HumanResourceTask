using HumanResourceTask.Api.Dto;
using HumanResourceTask.MetaModels;

namespace HumanResourceTask.Api.Mapping
{
    public static class PaginationMapping
    {
        public static PaginationDefinition ToModel(this Pagination? pagination)
        {
            if (pagination is null)
            {
                return PaginationDefinition.Defaults;
            }

            return new PaginationDefinition
            {
                Offset = pagination.Offset,
                Limit = pagination.Limit,
            };
        }
    }
}
