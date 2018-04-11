using System;
using System.IO;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ContactsTests
{
    [TestFixture]
    public class CreateContactTests
    {
        [Test]
        public void CreateContact_AllFields_Success()
        {
            IWebDriver driver = new ChromeDriver();
            try
            {
                driver.Navigate().GoToUrl("http://93.174.133.33/");

                new WebDriverWait(driver, TimeSpan.FromSeconds(1))
                    .Until(ExpectedConditions.ElementExists(By.CssSelector("div.list-data")));

                driver.FindElement(By.ClassName("js-create")).Click();

                IWebElement firstName = new WebDriverWait(driver, TimeSpan.FromSeconds(1))
                    .Until(ExpectedConditions.ElementExists(By.Id("FirstName")));

                // Test data
                firstName.SendKeys("Name");
                driver.FindElement(By.Id("SecondName")).SendKeys("Secondname");
                string expectedSureName = Guid.NewGuid().ToString(); //Id for future search in the grid
                driver.FindElement(By.Id("SureName")).SendKeys(expectedSureName);
                driver.FindElement(By.Id("PhoneStringValue")).SendKeys("8(822)6111111");
                driver.FindElement(By.Id("Email")).SendKeys("valid@email.com");

                driver.FindElement(By.ClassName("js-save")).Click();

                new WebDriverWait(driver, TimeSpan.FromSeconds(1))
                    .Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.form-item")));

                var elements = driver.FindElements(By.XPath(string.Format("//div[contains(text(),'{0}')]", expectedSureName)));

                Assert.AreEqual(elements.Count, 1);
            }
            catch (Exception ex)
            {
                var screenshotFolder = "C:/Temp/TestOuput/";
                Directory.CreateDirectory(screenshotFolder);
                var screenshotPath = screenshotFolder + String.Format(DateTime.Now.ToString("HHmmss")) + ".png";
                var screenShot = ((ITakesScreenshot)driver).GetScreenshot();

                screenShot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);

                throw;
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}
