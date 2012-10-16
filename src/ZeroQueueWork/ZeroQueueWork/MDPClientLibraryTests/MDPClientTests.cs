using MDPClientLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MDPClientLibraryTests
{


    /// <summary>
    ///This is a test class for MDPClientTests and is intended
    ///to contain all MDPClientTests Unit Tests
    ///</summary>
    [TestClass]
    public class MDPClientTests
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for MDPClient Constructor
        ///</summary>
        [TestMethod]
        public void MDPClientConstructorTest()
        {
            new MDPClient(null);
        }

        [TestMethod]
        public void MDPClientSendMessageTest()
        {
            var target = new MDPClient("tcp://localhost:5555");     // Create MDP Client to send messages to

            const string message = "hello world";
            const string service = "ECHO";

            var reply = target.SendMessage(message, service);

            Assert.AreEqual(message, reply);

        }
    }
}
