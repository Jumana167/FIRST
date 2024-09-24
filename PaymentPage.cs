using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

public class PaymentPage
{
	private readonly IWebDriver driver;
	private readonly WebDriverWait wait;

	public PaymentPage(IWebDriver driver)
	{
		this.driver = driver;
		this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
	}

	public void EnterCardDetails(string cardholderName, string cardNumber, string cvv, string expirationDate)
	{
		var cardHolderNameField = wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[1]/section[2]/div/div/div/div/form/div[4]/input")));
		cardHolderNameField.Click(); 
		cardHolderNameField.SendKeys(cardholderName);

		var cardNumberField = wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[1]/section[2]/div/div/div/div/form/div[5]/div[1]/div/input")));
		cardNumberField.Click();
		cardNumberField.SendKeys(cardNumber);

		var cvvField = wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[1]/section[2]/div/div/div/div/form/div[5]/div[2]/div/input")));
		cvvField.Click(); 
		cvvField.SendKeys(cvv);

		SetExpirationDate(expirationDate);
	}

	private void SetExpirationDate(string expirationDate)
	{
		var expirationParts = expirationDate.Split('/');
		if (expirationParts.Length != 2)
		{
			throw new ArgumentException("Expiration date must be in MM/YY format.");
		}

		string month = expirationParts[0].Trim();
		string year = expirationParts[1].Trim();

		var datePickerField = wait.Until(driver => driver.FindElement(By.XPath("//input[contains(@placeholder, 'Expiration Date')]")));
		datePickerField.Click();

		var yearSelector = wait.Until(driver => driver.FindElement(By.XPath($"//div[text()='{year}']")));
		yearSelector.Click();

		var monthSelector = wait.Until(driver => driver.FindElement(By.XPath($"//div[text()='{month}']")));
		monthSelector.Click();
	}

	public void SubmitPayment()
	{
		var payButton = wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[1]/section[2]/div/div/div/div/form/button")));
		payButton.Click();
	}
}
