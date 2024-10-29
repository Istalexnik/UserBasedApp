﻿using Microsoft.Extensions.Logging;
using UserBasedApp.Data;
using UserBasedApp.Services;

namespace UserBasedApp
{
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
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<UserRepository>();
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<ProjectRepository>();
            builder.Services.AddSingleton<ProjectService>();
            builder.Services.AddSingleton<AuthService>();

            return builder.Build();
        }
    }
}
