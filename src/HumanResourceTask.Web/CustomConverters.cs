using MudBlazor;

namespace HumanResourceTask.Web
{
    public static class CustomConverters
    {
        public static readonly Converter<DateOnly?> DateOnlyConverter = new()
        {
            SetFunc = value => value?.ToString("yyyy-MM-dd") ?? "",
            GetFunc = text => string.IsNullOrEmpty(text) ? default : DateOnly.Parse(text)
        };
    }
}
