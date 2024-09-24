using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace Testherfa
{
	public class UserloginPage
	{
		private readonly IWebDriver driver;

		public UserloginPage(IWebDriver driver)
		{
			this.driver = driver;
		}

		public void EnterEmail(string email)
		{
			driver.FindElement(By.Id("Email")).SendKeys(email);
		}

		public void EnterPassword(string password)
		{
			driver.FindElement(By.Id("myPass1")).SendKeys(password);
		}

		public void ClickLoginButton()
		{
			By loginButton = By.XPath("//div/button[contains(text(), 'Login')]");
			driver.FindElement(loginButton).Click();
		}

		public void Login(string email, string password)
		{
			EnterEmail(email);
			EnterPassword(password);
			ClickLoginButton();
		}

		public void ClickShoppingCartIcon()
		{
			By shoppingCartIcon = By.ClassName("cart");
			driver.FindElement(shoppingCartIcon).Click();
		}
	}

	[TestClass]
	public class PaymentTests
	{
		private IWebDriver driver;

		[TestInitialize]
		public void Setup()
		{
			driver = new ChromeDriver();
			driver.Manage().Window.Maximize();
		}

		[TestCleanup]
		public void Cleanup()
		{
			driver.Quit();
		}

		[TestMethod]
		public void TestValidPayment()
		{
			driver.Navigate().GoToUrl("https://localhost:44349/Auth/Login");
			WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
			wait.Until(driver => driver.FindElement(By.Id("Email"))); 

			UserloginPage loginPage = new UserloginPage(driver);
			loginPage.Login("hayaah2018@gmail.com", "123456");

			wait.Until(driver => driver.Url.Contains("https://localhost:44349/User"));
			Assert.AreEqual("https://localhost:44349/User", driver.Url, "لم يتم الانتقال إلى الصفحة المتوقعة بعد تسجيل الدخول");

			driver.Navigate().GoToUrl("https://localhost:44349/User/ShoppingCart");
			Thread.Sleep(5000);

			ScrollToElementAndClick("/html/body/div[1]/div/div/div[2]/div[3]/a[2]");

			driver.Navigate().GoToUrl("https://localhost:44349/User/PayForTheOrder");
		}

		private void ScrollToElementAndClick(string xpath)
		{
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
			var element = driver.FindElement(By.XPath(xpath));
			js.ExecuteScript("arguments[0].scrollIntoView(true);", element); 
			Thread.Sleep(1000);
			element.Click(); 

			WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
			wait.Until(driver => driver.Url.Contains("https://localhost:44349/User/PayForTheOrder"));
			Thread.Sleep(5000);

			}
		}
	}
