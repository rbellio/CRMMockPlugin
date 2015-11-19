using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Fakes;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Fakes;
using Microsoft.QualityTools.Testing.Fakes;

namespace MockPlugin.Test
{
    /// <summary>
    /// Summary description for BPluginTest
    /// </summary>
    [TestClass]
    public class BPluginTest
    {
        private StubIServiceProvider ServiceProvider { get; set; }
        private StubIOrganizationServiceFactory OrganizationServiceFactory { get; set; }
        private Entity TestEntity { get; set; }
        private EntityCollection TestEntityCollection {get;set;}
        private StubIOrganizationService Service { get; set; }
        public BPluginTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        [TestInitialize]
        public void Init()
        {
            var context = new StubIPluginExecutionContext();
            Service = new StubIOrganizationService();
            Service.RetrieveStringGuidColumnSet = (entityName, id, columns) =>
                {
                    return TestEntity;
                };

            // You can configure the type of entity collection you'd expect here.
            Service.RetrieveMultipleQueryBase = (queryExpression) =>
                {
                    return TestEntityCollection;
                };
            
            OrganizationServiceFactory = new StubIOrganizationServiceFactory();
            OrganizationServiceFactory.CreateOrganizationServiceNullableOfGuid 
                = (id) =>
                {
                    return Service;
                };

            ServiceProvider = new StubIServiceProvider();
            ServiceProvider.GetServiceType =
                (type) =>
                {
                    if (type == typeof(IPluginExecutionContext))
                    {
                        return context;
                    }
                    else if (type == typeof(IOrganizationServiceFactory))
                    {
                        return OrganizationServiceFactory;
                    }
                    else
                    {
                        return null;
                    }
                };

            var inputParameters = new ParameterCollection();
            context.InputParametersGet = () => { return inputParameters; };

            TestEntity = new Entity();
            inputParameters.Add(new System.Collections.Generic.KeyValuePair<string, object>("Target", TestEntity));
        }

        [TestCleanup]
        public void Cleanup()
        {
            ServiceProvider = null;
            TestEntity = null;
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidPluginExecutionException), "Exception not thrown")]
        public void TestPluginThrowsDateException()
        {
            TestEntity["new_dateclosed"] = new DateTime(2010, 1, 1);
            var plugin = new BPlugin();

            plugin.Execute(ServiceProvider);
        }

        [TestMethod]
        public void TestValidDate()
        {
            TestEntity["new_dateclosed"] = new DateTime(DateTime.Now.Year, 11, 1);
            var plugin = new BPlugin();

            plugin.Execute(ServiceProvider);
        }

        [TestMethod]
        public void TestSpecificDateTimeNow()
        {
            TestEntity["new_dateclosed"] = new DateTime(2012, 11, 1);
            var plugin = new BPlugin();

            using (ShimsContext.Create())
            {
                System.Fakes.ShimDateTime.NowGet = () =>
                {
                    return new DateTime(2012, 1, 15);
                };
                plugin.Execute(ServiceProvider);
            }
        }

        [TestMethod]
        public void TestOrganizationServiceMock()
        {
            TestEntityCollection = new EntityCollection();
            TestEntityCollection.Entities.Add(new Entity("account"));

            var plugin = new OrgPlugin();

            plugin.Execute(ServiceProvider);
        }
    }
}
