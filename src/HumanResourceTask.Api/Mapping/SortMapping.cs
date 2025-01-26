using HumanResourceTask.Api.Dto;
using HumanResourceTask.MetaModels;

namespace HumanResourceTask.Api.Mapping
{
    public static class SortMapping
    {
        public static SortDefinition ToModel(this Sort? sort)
        {
            if (sort is null)
            {
                return SortDefinition.Defaults;
            }

            return new SortDefinition
            {
                ColumnName = sort.Column,
                Direction = sort.Ascending ? SortDirection.Ascending : SortDirection.Descending,
            };
        }
    }
}
