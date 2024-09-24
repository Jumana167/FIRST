using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Testherfa;

namespace PaymentSystemTests
{
	[TestClass]
	public class PaymentTests
	{
		private IWebDriver driver;
		private WebDriverWait wait;

		[TestInitialize]
		public void TestInitialize()
		{
			driver = new ChromeDriver();
			driver.Manage().Window.Maximize();
			wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
		}


		[TestCleanup]
		public void TestCleanup()
		{
			driver.Quit();
		}

		[TestMethod]
		public void TestValidPayment()
		{
			NavigateToPaymentPage();
			EnterPaymentDetails("1234123412340000", "AhmadOmari", "357", "OCT-2030");
			SubmitPayment();
			VerifyPaymentSuccess();
		}

		[TestMethod]
		public void TestExpiredCard()
		{
			NavigateToPaymentPage();
			EnterPaymentDetails("5555666677778888", "qusayqa", "555", "FEB - 2023");
			SubmitPayment();
			VerifyErrorMessage("Card has expired");
		}

		[TestMethod]
		public void TestInsufficientFunds()
		{
			NavigateToPaymentPage();
			EnterPaymentDetails("5555666677778923", "Abdelrahman Husseini", "994", "APR-2024");
			SubmitPayment();
			VerifyErrorMessage("Insufficient funds");
		}

		[TestMethod]
		public void TestInvalidCVV()
		{
			NavigateToPaymentPage();
			EnterPaymentDetails("5555666677778907", "Batool Jarrah", "3799", "MAR-2025");
			SubmitPayment();
			VerifyErrorMessage("Invalid CVV");
		}

		[TestMethod]
		public void TestCardNumberValidation()
		{
			NavigateToPaymentPage();
			EnterPaymentDetails("1234123412341234", "Raghad Albetar", "789", "APR-2026");
			SubmitPayment();
			VerifyErrorMessage("Invalid card number");
		}

		private void NavigateToPaymentPage()
		{
			driver.Navigate().GoToUrl("https://localhost:44349/Auth/Login");
			wait.Until(driver => driver.FindElement(By.Id("Email")));

			UserloginPage loginPage = new UserloginPage(driver);
			loginPage.Login("hayaah2018@gmail.com", "123456");

			wait.Until(driver => driver.Url.Contains("https://localhost:44349/User"));
			Assert.AreEqual("https://localhost:44349/User", driver.Url, "Did not navigate to expected page after login");

			driver.Navigate().GoToUrl("https://localhost:44349/User/ShoppingCart");
			wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[1]/div/div/div[2]/div[3]/a[2]")));

			driver.FindElement(By.XPath("/html/body/div[1]/div/div/div[2]/div[3]/a[2]")).Click();

			wait.Until(driver => driver.Url.Contains("https://localhost:44349/User/PayForTheOrder"));
		}

		private void EnterPaymentDetails(string cardNumber, string cardholderName, string cvv, string expirationDate)
		{
			wait.Until(driver => driver.FindElement(By.Id("card-number")));
			driver.FindElement(By.Id("card-number")).SendKeys(cardNumber);
			driver.FindElement(By.Id("cardholder-name")).SendKeys(cardholderName);
			driver.FindElement(By.Id("cvv")).SendKeys(cvv);
			SetExpirationDate(expirationDate);
		}

		private void EnterAmount(string amount)
		{
			wait.Until(driver => driver.FindElement(By.Id("amount")));
			driver.FindElement(By.Id("amount")).Clear();
			driver.FindElement(By.Id("amount")).SendKeys(amount);
		}

		private void SubmitPayment()
		{
			wait.Until(driver => driver.FindElement(By.Id("submit-payment")));
			driver.FindElement(By.Id("submit-payment")).Click();
		}

		private void VerifyPaymentSuccess()
		{
			wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div/div[2]/div[1]")));
			Assert.IsTrue(driver.FindElement(By.XPath("/html/body/div[1]/div[1]/div/div[2]/div[1]")).Displayed, "Payment success message not displayed");
		}

		private void VerifyErrorMessage(string expectedError)
		{
			wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[1]/section[2]/div/div/div/div/form/span/div")));
			string actualError = driver.FindElement(By.XPath("/html/body/div[1]/section[2]/div/div/div/div/form/span/div")).Text;
			Assert.IsTrue(actualError.Contains(expectedError), $"Expected error message '{expectedError}' not found. Actual: '{actualError}'");
		}
		private void SetExpirationDate(string expirationDate)
		{
			string[] dateParts = expirationDate.Split('/');
			string month = dateParts[0];
			string year = dateParts[1];

			var datePickerField = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//input[contains(@placeholder, 'Expiration Date')]")));
			datePickerField.Click();

			// Select Year
			var yearSelector = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath($"//div[text()='{year}']")));
			yearSelector.Click();

			// Select Month
			var monthSelector = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath($"//div[text()='{month}']")));
			monthSelector.Click();
		}


	}
}
