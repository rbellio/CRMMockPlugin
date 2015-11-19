using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockPlugin
{
    public class OrgPlugin : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var entity = (Entity)context.InputParameters["Target"];

            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            var service = serviceFactory.CreateOrganizationService(context.UserId);

            var query = new QueryExpression();
            query.ColumnSet = new ColumnSet(true);
            var entitycollection = service.RetrieveMultiple(query);
        }
    }
}
