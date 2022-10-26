using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace MailCheck.Common.TestSupport
{
    public class BaseIntegrationTest<TStartup, TController> where TStartup : class
    {
        protected List<(string, string)> Endpoints;
        protected WebApplicationFactory<TStartup> Factory;

        [SetUp]
        public void BaseSetUp()
        {
            Factory = new WebApplicationFactory<TStartup>();
            Endpoints = new List<(string, string)>();

            Factory = Factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                        {
                            IEnumerable<ActionDescriptor> actionDescriptors = services.BuildServiceProvider().GetRequiredService<IActionDescriptorCollectionProvider>().ActionDescriptors.Items
                                .Where(x => (x as ControllerActionDescriptor)?.ControllerTypeInfo.AsType() == typeof(TController));

                            foreach (ActionDescriptor actionDescriptor in actionDescriptors)
                            {
                                string template = actionDescriptor.AttributeRouteInfo.Template;
                                string method = ((HttpMethodActionConstraint)actionDescriptor.ActionConstraints[0]).HttpMethods.First();
                                Endpoints.Add((method, template));
                            }
                        }
                    );
                }
            );
        }
    }
}