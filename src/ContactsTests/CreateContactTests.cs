using System;
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

                int initialContactsCount = driver.FindElements(By.CssSelector("li.item-line")).Count;
                string expectedSureName = Guid.NewGuid().ToString();

                driver.FindElement(By.ClassName("js-create")).Click();

                IWebElement firstName = new WebDriverWait(driver, TimeSpan.FromSeconds(1))
                    .Until(ExpectedConditions.ElementExists(By.Id("FirstName")));
                firstName.SendKeys("Name");
                driver.FindElement(By.Id("SecondName")).SendKeys("Secondname");
                driver.FindElement(By.Id("SureName")).SendKeys(expectedSureName);
                driver.FindElement(By.Id("PhoneStringValue")).SendKeys("11111111111");
                driver.FindElement(By.Id("Email")).SendKeys("valid@email.com");

                driver.FindElement(By.ClassName("js-save")).Click();

                new WebDriverWait(driver, TimeSpan.FromSeconds(1))
                    .Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector("div.form-item")));

                int finalContactsCount = driver.FindElements(By.CssSelector("li.item-line")).Count;
                var elements = driver.FindElements(By.XPath(string.Format("//div[contains(text(),'{0}')]", expectedSureName)));

                Assert.AreEqual(initialContactsCount + 1, finalContactsCount);
                Assert.AreEqual(elements.Count, 1);
            }
            finally
            {
                driver.Quit();
            }
        }
    }
}
