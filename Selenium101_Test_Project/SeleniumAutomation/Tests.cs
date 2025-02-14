using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace Selenium101_Test_Project.SeleniumAutomation
{
    public class Tests
    {
        public static string LT_USERNAME = Environment.GetEnvironmentVariable("LT_USERNAME") ?? "Pratibha_Jamdare";
        public static string LT_ACCESS_KEY = Environment.GetEnvironmentVariable("LT_ACCESS_KEY") ?? "LT_j5dCbPFOqZDQtNP5f3Pmi3Um3czmCgI0YZ8SkXhRJHsGJ6q";
        public static bool tunnel = bool.Parse(Environment.GetEnvironmentVariable("LT_TUNNEL") ?? "false");
        public static string build = Environment.GetEnvironmentVariable("LT_BUILD") ?? "LambdatestBuildChrome";
        public static string seleniumUri = "https://hub.lambdatest.com:443/wd/hub";
        IWebDriver? driver;
        TestLocators? testLocator;
        private string browser = string.Empty;
        private string version = string.Empty;
        private string os = string.Empty;

        [SetUp]
        public void Init()
        {
            ChromeOptions capabilities = new ChromeOptions();
            capabilities.BrowserVersion = "dev";
            Dictionary<string, object> ltOptions = new Dictionary<string, object>
                {
                    { "username", LT_USERNAME },
                    { "accessKey", LT_ACCESS_KEY },
                    { "platformName", "Windows 10" },
                    { "project", "Selenium101_Test_Project" },
                    { "w3c", true },
                    { "plugin", "c#-nunit" }
                };

            if (tunnel)
            {
                ltOptions.Add("tunnel", tunnel);
            }
            if (build != null)
            {
                ltOptions.Add("build", build);
            }

            capabilities.AddAdditionalOption("lt:options", ltOptions);

            capabilities.AddAdditionalOption("name",
                string.Format("{0}:{1}",
                TestContext.CurrentContext.Test.ClassName,
                TestContext.CurrentContext.Test.MethodName));

            driver = new RemoteWebDriver(new Uri(seleniumUri), capabilities.ToCapabilities(), TimeSpan.FromSeconds(600));
            Console.Out.WriteLine(driver);
        }

        [Test]
        public void TestScenario1()
        {
            string labdaTest = "Welcome to LambdaTest";
            // Arrange
            testLocator = new TestLocators(driver);
            driver.Navigate().GoToUrl("https://www.lambdatest.com/selenium-playground/");
            driver.Manage().Window.Maximize();
            // Act
            testLocator.Click(testLocator.SimpleFrmDemo);
            //testLocator.EnterText(testLocator.SimplefrmIp, "Welcome to LambdaTest");
            //testLocator.Click(testLocator.GetCheckedValue);
            // Assert
            Assert.That(driver.Url, Does.Contain("simple-form-demo"));
            testLocator.EnterMessageTxtBx.SendKeys(labdaTest);
            testLocator.GetCheckedValuebuttton.Click();
            Assert.AreEqual(testLocator.MessageDisplayed.Text, labdaTest);
            //Assert.That(testLocator.SampleMsg.Text, Is.EqualTo("Welcome to LambdaTest"));
        }
        [Test]
        public void TestScenario2()
        {
            // Arrange
            testLocator = new TestLocators(driver);
            driver.Navigate().GoToUrl("https://www.lambdatest.com/selenium-playground/");
            driver.Manage().Window.Maximize();
            //Act
            testLocator.Click(testLocator.DragAndDrop);
            Actions action = new Actions(driver);
            // Move slider to the right on x-axis by 212 pixels
            action.DragAndDropToOffset(testLocator.Slider, 212, 0).Perform();
            //Assert
            Assert.That(testLocator.SliderValue.GetAttribute("value"), Is.EqualTo("95"));
        }

        [Test]
        public void TestScenario3()
        {
            // Arrange
            testLocator = new TestLocators(driver);
            driver.Navigate().GoToUrl("https://www.lambdatest.com/selenium-playground/");
            driver.Manage().Window.Maximize();
            //Act
            testLocator.Click(testLocator.InputForm);
            testLocator.Click(testLocator.SubmitForm);

            // Switch to active element
            IWebElement activeElement = driver.SwitchTo().ActiveElement();
            var validationMessage = activeElement.GetAttribute("validationMessage");

            //Assert
            Assert.That(validationMessage, Is.EqualTo("Please fill out this field."));

            //act
            testLocator.EnterText(testLocator.Name, "Pratibha");
            testLocator.EnterText(testLocator.Email, "Pratibha.atkare@email.com");
            testLocator.EnterText(testLocator.Password, "Password");
            testLocator.EnterText(testLocator.Company, "LambdaTest");
            testLocator.EnterText(testLocator.Website, "https://www.lambdatest.com");
            //Select Country from drop down list
            SelectElement s = new SelectElement(testLocator.Country);
            s.SelectByText("United States");
            testLocator.EnterText(testLocator.City, "Albuquerque");
            testLocator.EnterText(testLocator.Address1, "123 Street");
            testLocator.EnterText(testLocator.Address2, "123 Street");
            testLocator.EnterText(testLocator.State, "New Maxico");
            testLocator.EnterText(testLocator.Zip, "111225");
            testLocator.Click(testLocator.SubmitForm);
            //Assert
            Assert.True(testLocator.SuccessMsg.Displayed);
            Assert.That(testLocator.SuccessMsg.Text, Is.EqualTo("Thanks for contacting us, we will get back to you shortly."));

        }


        [TearDown]
        public void Close()
        {
            driver?.Close();
        }
    }
}