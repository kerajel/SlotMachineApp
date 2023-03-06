using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SlotMachineApp.Model;

namespace SlotMachineApp;

internal static class Registry
{
    internal static IServiceProvider GetServiceProvider()
    {
        var services = new ServiceCollection();

        var config = new ConfigurationBuilder()
            .AddJsonFile("Config/AppSettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("Config/SpinnerSections.json", optional: false, reloadOnChange: true)
            .AddJsonFile("Config/NoteSets.json", optional: false, reloadOnChange: true)
            .Build();

        services.Configure<SlotMachineSettings>(options => config.GetSection(nameof(SlotMachineSettings)).Bind(options));
        services.Configure<SpinnerSettings>(options => config.GetSection(nameof(SpinnerSettings)).Bind(options));
        services.Configure<SoundSettings>(options => config.GetSection(nameof(SoundSettings)).Bind(options));

        services.AddSingleton(_ => config.GetSection(nameof(SpinnerSection)).GetChildren().Select(s => s.Get<SpinnerSection>()!));
        services.AddSingleton(_ => config.GetSection("NoteSets").Get<IEnumerable<NoteSet>>()!);

        services.AddTransient<ISpinner, Spinner>();
        services.AddTransient<ISpinnerService, SpinnerService>();
        services.AddTransient<ISpinnerSectionService, SpinnerSectionService>();
        services.AddTransient<ISlotMachineGame, SlotMachineGame>();
        services.AddTransient<IOutputProvider, ConsoleOutputProvider>();
        services.AddTransient<IInputSource, ConsoleInputSource>();
        services.AddTransient<INoteSetProvider, NoteSetProvider>();
        services.AddTransient<ISoundService, SoundService>();

        var spinnerSettings = config.GetSection(nameof(SpinnerSettings)).Get<SpinnerSettings>()!;
        var spinnerCount = spinnerSettings.SpinnerCount;            
        services.AddTransient(sp => Enumerable.Range(1, spinnerCount).Select(_ => sp.GetRequiredService<ISpinner>()));

        return services.BuildServiceProvider();
    }
}
