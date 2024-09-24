using OpenQA.Selenium;
using System;
using NUnit.Framework; 

namespace HerfaTest.POM
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
        }

        private readonly By email = By.XPath("//div/input[@id='Email']");
        private readonly By password = By.XPath("//div/input[@id='myPass1']");
        private readonly By loginButton = By.XPath("//div/button[contains(text(), 'Login')]");

        public void EnterEmail(string value)
        {
            WaitForElementToBeVisible(email);
            _driver.FindElement(email).SendKeys(value);
        }

        public void EnterPassword(string value)
        {
            WaitForElementToBeVisible(password);
            _driver.FindElement(password).SendKeys(value);
        }

        public void ClickLoginButton()
        {
            WaitForElementToBeVisible(loginButton);
            _driver.FindElement(loginButton).Click();
        }

        private void WaitForElementToBeVisible(By locator)
        {
            int attempts = 0;
            while (attempts < 10)
            {
                try
                {
                    if (_driver.FindElement(locator).Displayed)
                    {
                        return; 
                    }
                }
                catch (NoSuchElementException)
                {
                }
                catch (ElementNotVisibleException)
                {
                }

                System.Threading.Thread.Sleep(5000); 
                attempts++;
            }
            throw new Exception($"Element {locator} not visible after waiting.");
        }
		public void ClickShoppingCartIcon()
		{
			By shoppingCartIcon = By.XPath("//a[contains(@class, 'cart') and @aria-label='View your shopping cart']");
			_driver.FindElement(shoppingCartIcon).Click();
		}

	}
}
