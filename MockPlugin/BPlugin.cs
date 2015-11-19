using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockPlugin
{
    public class BPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var entity = context.InputParameters["Target"] as Entity;

            //Establish case reactivation is being attempted - omitted to simplify example

            var dateClosed = entity.GetAttributeValue<DateTime>("new_dateclosed");
            if (dateClosed.AddDays(30) < DateTime.Now)
            {
                throw new InvalidPluginExecutionException("This case has been closed for too long to repopen.");
            }
        }
    }
}
