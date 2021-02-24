using Microsoft.Extensions.DependencyInjection;
using Rally.Core.Client;
using Rally.Core.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rally.Core
{
    public static class RallyExtensions
    {
        public static IServiceCollection AddRally(this IServiceCollection services, Action<IRallyBuilder> onbuild = null)
        {
            RallyBuilder builder = new RallyBuilder(services);
            if (builder != null)
            {
                onbuild(builder);
            }
            return builder.Build();
        }
    }
}
