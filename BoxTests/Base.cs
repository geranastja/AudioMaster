using System;
using System.IO;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Text;
using NUnit.Framework.Interfaces;
using System.Collections;



namespace BoxTests
{
    [TestFixture]

    public class Base
    {
       
        protected IWebDriver driver;
        protected string baseURL;
        protected WebDriverWait iWait;
        protected string Message;
        protected static string TestName;
        protected string AdminLogin;
        protected string ModerateLogin;

        protected static IEnumerable GetSites()
        {

            yield return new TestCaseData("https://ac.sc-discovery.ru/");
          //  yield return new TestCaseData("https://audiomaster.sc-discovery.ru/", "admin");

        }

        [OneTimeSetUp] // вызывается перед началом запуска всех тестов
        protected void TestFixtureSetUp()
        {
            DeleteDirectory();
        }
        protected static void DeleteDirectory()
        {

            try
            {
                var test = TestContext.CurrentContext.Test;
                string TestClassName = test.ClassName.Replace("BoxTests.", "");
                Directory.Delete(Path.Combine(TestContext.CurrentContext.TestDirectory, TestClassName), true);
            }
            catch
            {
            }
        }


        [SetUp]
        protected void TestSetup()
        {
            try
            {
                var options = new ChromeOptions();
                var service = ChromeDriverService.CreateDefaultService();
                driver = new ChromeDriver(service, options,
                    TimeSpan.FromSeconds(40));
                baseURL = "https://ac.sc-discovery.ru/";
                iWait = new WebDriverWait(driver, TimeSpan.FromSeconds(40));
                TrySetEncoding();
                TestName = GetTestClass();
                AdminLogin = "admin";
                ModerateLogin = "adminAutoTest";
    }
            catch
            {
                Cleanup();
                throw;
            }
        }
        protected static void TrySetEncoding()
        {
            try
            {
                Console.OutputEncoding = Encoding.UTF8;
            }
            catch
            {
            }
        }

        [TearDown]
        protected void Cleanup()
        {
            if ((TestContext.CurrentContext.Result.Outcome == ResultState.Failure) || (TestContext.CurrentContext.Result.Outcome == ResultState.Error))
            {

                Console.WriteLine("Error - " + Message);
                Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
                Directory.CreateDirectory(Path.Combine(TestContext.CurrentContext.TestDirectory, GetTestClass()));
                ss.SaveAsFile(Path.Combine(Environment.CurrentDirectory, GetScreenshotFileName()));
            }
            else
            {
                Console.WriteLine("Not detected, the test was successful");
            }
            driver?.Quit();
            driver?.Dispose();
        }
        protected static string GetScreenshotPath()
        {
            return Path.Combine(TestContext.CurrentContext.TestDirectory, GetTestClass());
        }
        protected static string GetScreenshotFileName()
        {
            return Path.Combine(GetScreenshotPath(), GetTestName() + ".png");
        }
        protected static string GetTestClass()
        {
            var test = TestContext.CurrentContext.Test;
          
            if (test.Arguments.Length > 0)
            {
                var arg = test.Arguments[0].ToString();
                if (Uri.IsWellFormedUriString(arg, UriKind.Absolute))
                {
                    //string TestClassName = test.ClassName.Replace("BoxTests.", "");
                    //Console.WriteLine("GetTestName function  " + TestClassName);
                    
                    return test.ClassName.Replace("BoxTests.", "");
                }
            }

            return test.MethodName;
        }
        protected static string GetTestName()
        {
            var test = TestContext.CurrentContext.Test;
            if (test.Arguments.Length > 0)
            {
                var arg = test.Arguments[0].ToString();
                if (Uri.IsWellFormedUriString(arg, UriKind.Absolute))
                {
                    return test.MethodName + "-" + new Uri(arg).Host;
                }
            }

            return test.MethodName;
        }
        protected bool IsElementPresent(By Element)
        {
            try
            {


                driver.FindElement(Element);
                return true;


            }
            catch (NoSuchElementException)
            {
                return false;

            }
        }
        protected bool IsWaitElementToBeClickable(By Element)
        {
            try
            {

                WaitUntilClickable(Element);

                return true;


            }
            catch (WebDriverTimeoutException)
            {
                return false;

            }
        }
        protected void CheckedAndClick(By Element)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView();", driver.FindElement(Element));
            if (driver.FindElement(Element).Selected == false)
            {
                js.ExecuteScript("arguments[0].click()", driver.FindElement(Element));
            }
        }
        protected void WaitUntilVisible(By element)
        {
            iWait.Until(driver =>
            {
                try
                {
                    var elementToBeDisplayed = driver.FindElement(element);
                    if (elementToBeDisplayed.Displayed)
                    {
                        return elementToBeDisplayed;
                    }
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            });
        }
        protected void WaitUntilEnabled(By element)
        {
            iWait.Until(driver =>
            {
                try
                {
                    var elementToBeEnabled = driver.FindElement(element);
                    if (elementToBeEnabled.Enabled)
                    {
                        return elementToBeEnabled;
                    }
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            });
        }
        protected void WaitUntilClickable(By element)
        {
            iWait.Until(driver =>
            {
                try
                {
                    var elementToBeDisplayed = driver.FindElement(element);
                    if (elementToBeDisplayed.Displayed)// && elementToBeDisplayed.Enabled
                    {
                        return elementToBeDisplayed;
                    }
                    return null;
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            });
        }
        protected void WaitUntilVisibleAndClick(By element)
        {
            iWait.Until(driver =>
            {
                try
                {
                    var elementToBeDisplayed = driver.FindElement(element);
                    if (elementToBeDisplayed.Displayed)
                    {
                        return elementToBeDisplayed;
                    }
                    return null;

                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }

            });
            driver.FindElement(element).Click();
        }
        protected void WaitUntilVisibleText(By element, string Text)
        {
            iWait.Until(driver =>
            {
                try
                {

                    string Text2 = driver.FindElement(element).Text;
                    var elementToBeDisplayed = driver.FindElement(element);
                    if (Text2.Contains(Text))
                    {
                        return elementToBeDisplayed;
                    }
                    return null;

                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            });
        }
        protected void WaitUntilVisibleInText(By element, string Text)
        {
            iWait.Until(driver =>
            {
                try
                {
                    var elementToBeDisplayed = driver.FindElement(element);

                    if (elementToBeDisplayed.Text != Text)
                    {
                        return elementToBeDisplayed;
                    }
                    return null;

                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            });
        }
        protected void WaitUntilInVisibleAttribut(By element, string Attribut, string ValueAttribut)
        {
            iWait.Until(driver =>
            {
                try
                {
                    var elementToBeDisplayed = driver.FindElement(element);
                    if (elementToBeDisplayed.GetAttribute(Attribut) != ValueAttribut)
                    {
                        //  Console.WriteLine("атрибут не равен значению");
                        return elementToBeDisplayed;
                    }
                    return null;

                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }

            });
        }
        public void WaitUntilInVisible(By element)
        {
            iWait.Until(driver =>
            {
                try
                {


                    if (IsElementPresent(element) == false)
                    {
                        // Console.WriteLine("элемент не существует");
                        return true;
                    }
                    else
                    {
                        //  Console.WriteLine("элемент существует");
                        if (IsElementVisible(element) == false)
                        {
                            //  Console.WriteLine("элемент не видим");
                            return true;
                        }
                        return false;
                    }


                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });
        }
        private bool IsElementVisible(By Element)
        {

            if (driver.FindElement(Element).Displayed)
            {
                return true;
            }
            else
            {
                return false;
            }


        }


        protected void Authorization(string site, string login)
        {
            baseURL = site;
            Message = "1. Login under the user ";
            driver.Manage().Window.Maximize();
            //IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            WaitUntilVisible(By.XPath(".//*[@id='login']"));
            driver.FindElement(By.XPath(".//*[@id='login']")).SendKeys(login);
            WaitUntilVisible(By.XPath(".//*[@type='password']"));
            driver.FindElement(By.XPath(".//*[@type='password']")).SendKeys(login);
            driver.FindElement(By.XPath(".//*[@type='submit']")).Click();
            WaitUntilVisible(By.XPath(".//header//span[contains(text(),'"+ login + "')]"));
            Console.WriteLine(Message + login);
        }


    }
    }
