using Bunit;
using MudBlazor.Services;

namespace HumanResourceTask.Web.Test
{
    public abstract class TestMudContext : TestContext
    {
        protected TestMudContext()
        {
            JSInterop.Mode = JSRuntimeMode.Loose;
            Services.AddMudServices(options =>
            {
                options.SnackbarConfiguration.ShowTransitionDuration = 0;
                options.SnackbarConfiguration.HideTransitionDuration = 0;
                options.PopoverOptions.CheckForPopoverProvider = false;
            });

            AdditionalSetup();
        }

        protected virtual void AdditionalSetup()
        {
        }

        protected static void SetPrivateProperty<T>(T instance, string propertyName, object value)
        {
            if (instance is null)
            {
                return;
            }

            var property = instance.GetType().GetProperty(propertyName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            property?.SetValue(instance, value);
        }
    }
}
