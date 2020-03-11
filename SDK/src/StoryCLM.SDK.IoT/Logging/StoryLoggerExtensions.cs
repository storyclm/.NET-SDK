using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoryCLM.SDK.IoT.Logging
{
    public static class StoryLoggerExtensions
    {

        //public static ILoggingBuilder AddStory(this ILoggingBuilder builder)
        //{
        //    builder.AddConfiguration();

        //    builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, StoryLoggerProvider>());
        //    LoggerProviderOptions.RegisterProviderOptions<ConsoleLoggerOptions, ConsoleLoggerProvider>(builder.Services);
        //    return builder;
        //}

        ///// <summary>
        ///// Adds a console logger named 'Console' to the factory.
        ///// </summary>
        ///// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
        ///// <param name="configure"></param>
        //public static ILoggingBuilder AddConsole(this ILoggingBuilder builder, Action<ConsoleLoggerOptions> configure)
        //{
        //    if (configure == null)
        //    {
        //        throw new ArgumentNullException(nameof(configure));
        //    }

        //    builder.AddConsole();
        //    builder.Services.Configure(configure);

        //    return builder;
        //}
    }
}
