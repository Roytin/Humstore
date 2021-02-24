using Microsoft.Extensions.Hosting;
using Rally.Core;
using SuperSocket;
using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Hosting
{
    public static class GenericHostBuilderExtensions
    {
        public static IHostBuilder ConfigureRallyHost(this IHostBuilder builder, Action<IRallyBuilder> configure = null)
        {
            //初始化ss
            return builder
                .AsSuperSocketHostBuilder<TextPackageInfo, LinePipelineFilter>()
                .UsePackageHandler(async (s, p) =>
                {
                    await s.SendAsync(Encoding.UTF8.GetBytes(p.Text + "\r\n"));
                })
                .ConfigureServices(services =>//注入rally.core
                {
                    RallyBuilder rallyBuilder = new RallyBuilder(services);
                    if (configure != null)
                    {
                        configure(rallyBuilder);
                    }
                    rallyBuilder.Build();
                });
        }
    }
}
