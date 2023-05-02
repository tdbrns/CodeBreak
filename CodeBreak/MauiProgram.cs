using Microsoft.Extensions.Logging;

namespace CodeBreak;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("abduction2002.ttf", "Abduction2002");
                fonts.AddFont("VCR_OSD_MONO_1.001.ttf", "VCR_OSD_Mono");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
