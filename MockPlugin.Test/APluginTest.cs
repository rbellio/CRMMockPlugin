using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk.Client;
using System.Fakes;
using Microsoft.Xrm.Sdk.Fakes;
using Microsoft.Xrm.Sdk;

namespace MockPlugin.Test
{
    /// <summary>
    /// Summary description for APluginTest
    /// </summary>
    [TestClass]
    public class APluginTest
    {
        public APluginTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

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

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void FirstTest()
        {
            var serviceProvider = new StubIServiceProvider();
            var context = new StubIPluginExecutionContext();

            serviceProvider.GetServiceType =
                (type) =>
                {
                    if (type == typeof(IPluginExecutionContext))
                    {
                        return context;
                    }
                    else
                    {
                        return null;
                    }
                };

            var inputParameters = new ParameterCollection();
            context.InputParametersGet = () => { return inputParameters; };

            var testEntity = new Entity();
            inputParameters.Add(new KeyValuePair<string,object>("Target", testEntity));

            var plugin = new APlugin();

            plugin.Execute(serviceProvider);

            Assert.AreEqual<bool>(true,testEntity.GetAttributeValue<bool>("new_pluginexecuted"), 
                "Set new_pluginexecuted attribute value was not expected");
        }
    }
}
