using FastEndpoints;
using HumanResourceTask.Api.Dto;
using Microsoft.Extensions.Primitives;

namespace HumanResourceTask.Api
{
    public static class CustomParsers
    {
        public static ParseResult SortParser(object? arg)
        {
            if (arg is not StringValues values || values == StringValues.Empty)
            {
                return new ParseResult(false, null);
            }

            var value = values.ToString();

            string columnName;
            bool ascending;
            if (value[0] == '-')
            {
                ascending = false;
                columnName = value[1..];
            }
            else if (value[0] == '+')
            {
                ascending = true;
                columnName = value[1..];
            }
            else
            {
                ascending = true;
                columnName = value;
            }

            var sort = new Sort
            {
                Ascending = ascending,
                Column = columnName
            };
            return new ParseResult(true, sort);
        }
    }
}
