using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp;

namespace EmailMaketing;

public class EmailMaketingWebTestStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<EmailMaketingWebTestModule>();
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        app.InitializeApplication();
    }
}
