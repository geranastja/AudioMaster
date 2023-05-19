using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V106.DOM;

namespace BoxTests
{
    
    public class Tests: Base
    {

        [Test]
        [TestCaseSource(nameof(GetSites))]
        public void User(string site)
        {
            string User = "UserAutoTest";
            string EditUser = "EditUserAutoTest";
            driver.Navigate().GoToUrl(baseURL+ "cabinet/users");
            Authorization(site, AdminLogin);
            Message = "Search through a filter the user " + User;
            WaitUntilVisible(By.XPath(".//*[@role='rowgroup']/tr[1]"));
            WaitUntilVisibleAndClick(By.XPath(".//*[@id='search-select']"));//filter
            WaitUntilVisibleAndClick(By.XPath(".//*[@id='search-select']/option[@value='username']"));//select filter
            driver.FindElement(By.XPath(".//*[@id='cabinet-search']")).SendKeys(User);
            string Tolal = driver.FindElement(By.XPath(".//*[contains(text(),'Total')]")).Text;
            driver.FindElement(By.XPath(".//*[@type='submit']")).Click();

            //ждем прелоадер появится, исчезнет
            WaitUntilVisibleInText(By.XPath(".//*[contains(text(),'Total')]"), Tolal);
            Console.WriteLine(Message);
            //если есть юзер, то удаляем
            if (IsElementPresent(By.XPath(".//a[contains(text(),'" + User + "')]")) == true)
            {
                Message = "   Deleting " + User;
                driver.FindElement(By.XPath(".//*[@role='cell']//*[contains(@class,'menu-dots')]")).Click();
                WaitUntilVisibleAndClick(By.XPath(".//*[@role='cell']//*[contains(text(),'Delete')]"));
                WaitUntilVisibleAndClick(By.XPath(".//*[contains(@class,'shadow-modal')]//*[contains(text(),'Delete')]"));
                WaitUntilInVisible(By.XPath(".//a[contains(text(),'" + User + "')]"));
                Console.WriteLine(Message);
            }
            //если есть editюзер, то удаляем
            if (IsElementPresent(By.XPath(".//a[contains(text(),'" + EditUser + "')]")) == true)
            {
                Message = "Удаление " + EditUser;
                driver.FindElement(By.XPath(".//*[@role='cell']//*[contains(@class,'menu-dots')]")).Click();
                WaitUntilVisibleAndClick(By.XPath(".//*[@role='cell']//*[contains(text(),'Delete')]"));
                WaitUntilVisibleAndClick(By.XPath(".//*[contains(@class,'shadow-modal')]//*[contains(text(),'Delete')]"));
                WaitUntilInVisible(By.XPath(".//a[contains(text(),'" + EditUser + "')]"));
                Console.Write(Message);
            }
            Console.WriteLine("Adding the new user " + User);
            Message = "  Field Validation";
            WaitUntilVisibleAndClick(By.XPath(".//button[contains(text(),'Add user')]"));
            WaitUntilVisible(By.XPath(".//*[@type='submit']"));
            driver.FindElement(By.XPath(".//*[@id='username']")).SendKeys("1");
            driver.FindElement(By.XPath(".//*[@type='submit']")).Click();
            if (IsElementPresent(By.XPath(".//*[contains(@class,'error')]/*[@id='password']")) == false)
            {
                driver.FindElement(By.XPath(".//*[@type='submit']")).Click();
            }
            WaitUntilVisible(By.XPath(".//*[contains(@class,'error')]/*[@id='password']"));
            WaitUntilVisible(By.XPath(".//*[@id='first_name']/following-sibling::span[contains(@class,'error')]"));
            WaitUntilVisible(By.XPath(".//*[@id='last_name']/following-sibling::span[contains(@class,'error')]"));
            WaitUntilVisible(By.XPath(".//*[contains(@class,'error')]/*[@aria-labelledby='pick_city']"));
            WaitUntilVisible(By.XPath(".//*[contains(@class,'error')]/*[@aria-labelledby='pick_cabinet']"));
            WaitUntilVisible(By.XPath(".//*[@placeholder='max_seconds_record']/" +
                "following-sibling::span[contains(@class,'error')]"));
            WaitUntilVisible(By.XPath(".//*[@placeholder='min_seconds_record']/" +
                "following-sibling::span[contains(@class,'error')]"));
            WaitUntilVisible(By.XPath(".//*[@for='Manual']/following-sibling::span[contains(@class,'error')]"));
            Console.WriteLine(Message);

            Message = "  Entering data into fields ";
            driver.FindElement(By.XPath(".//*[@id='username']")).Clear();
            driver.FindElement(By.XPath(".//*[@id='username']")).SendKeys(User);
            driver.FindElement(By.XPath(".//*[@id='password']")).SendKeys(User);
            driver.FindElement(By.XPath(".//*[@id='first_name']")).SendKeys(User);
            driver.FindElement(By.XPath(".//*[@id='last_name']")).SendKeys(User);
            driver.FindElement(By.XPath(".//*[@id='phone']")).SendKeys(User);
            driver.FindElement(By.XPath(".//*[@id='email']")).SendKeys(User + "@mail.com");
            driver.FindElement(By.XPath(".//*[@aria-labelledby='pick_city']")).Click();
            WaitUntilVisibleAndClick(By.XPath(".//*[@class='dropdown-content']//*[contains(text(),'Select All')]"));
            driver.FindElement(By.XPath(".//*[@aria-labelledby='pick_city']")).Click();//close the window of city
            driver.FindElement(By.XPath(".//*[@aria-labelledby='pick_cabinet']")).Click();
            WaitUntilVisibleAndClick(By.XPath(".//*[@class='dropdown-content']//*[contains(text(),'Select All')]"));
            driver.FindElement(By.XPath(".//*[@aria-labelledby='pick_cabinet']")).Click();//close the window of city
            driver.FindElement(By.XPath(".//*[@placeholder='max_seconds_record']")).SendKeys("60");
            driver.FindElement(By.XPath(".//*[@placeholder='min_seconds_record']")).SendKeys("60");
            driver.FindElement(By.XPath(".//*[@for='Manual']")).Click();
            driver.FindElement(By.XPath(".//*[@type='submit']")).Click();
            WaitUntilVisible(By.XPath(".//button[contains(text(),'Add user')]"));
            driver.FindElement(By.XPath(".//a[contains(text(),'" + User + "')]"));
            Console.WriteLine(Message);

            //edit the user
            Message = "User editing";
            driver.FindElement(By.XPath(".//*[@role='cell']//*[contains(@class,'menu-dots')]")).Click();
            WaitUntilVisibleAndClick(By.XPath(".//*[@role='cell']//*[contains(text(),'Update')]"));
            WaitUntilVisible(By.XPath(".//*[@id='username'][@value='UserAutoTest']"));
            driver.FindElement(By.XPath(".//*[@id='username']")).Clear();
            driver.FindElement(By.XPath(".//*[@id='username']")).SendKeys(EditUser);
            driver.FindElement(By.XPath(".//*[@id='password']")).Clear();
            driver.FindElement(By.XPath(".//*[@id='password']")).SendKeys(EditUser);
            driver.FindElement(By.XPath(".//*[@id='first_name']")).Clear();
            driver.FindElement(By.XPath(".//*[@id='first_name']")).SendKeys(EditUser);
            driver.FindElement(By.XPath(".//*[@id='last_name']")).Clear();
            driver.FindElement(By.XPath(".//*[@id='last_name']")).SendKeys(EditUser);
            driver.FindElement(By.XPath(".//*[@id='phone']")).Clear();
            driver.FindElement(By.XPath(".//*[@id='phone']")).SendKeys(EditUser);
            driver.FindElement(By.XPath(".//*[@type='submit']")).Click();
            WaitUntilVisible(By.XPath(".//button[contains(text(),'Add user')]"));
            WaitUntilVisible(By.XPath(".//a[contains(text(),'" + EditUser + "')]"));
            Console.WriteLine(Message);

            //deleting the user
            Message = "Deleting user " + EditUser;
            driver.FindElement(By.XPath(".//*[@role='cell']//*[contains(@class,'menu-dots')]")).Click();
            WaitUntilVisibleAndClick(By.XPath(".//*[@role='cell']//*[contains(text(),'Delete')]"));
            WaitUntilVisibleAndClick(By.XPath(".//*[contains(@class,'shadow-modal')]//*[contains(text(),'Delete')]"));
            WaitUntilInVisible(By.XPath(".//a[contains(text(),'" + EditUser + "')]"));
            Console.WriteLine(Message);
        }


        [Test]
        [TestCaseSource(nameof(GetSites))]
        public void Cabinet(string site)
        {
            string Cabinet = "CabinetAutoTest";
            string EditCabinet = "EditCabinetAutoTest";
            driver.Navigate().GoToUrl(baseURL + "cabinet/cabinets");
            Authorization(site, ModerateLogin);
            WaitUntilVisible(By.XPath(".//*[@role='rowgroup']/tr[1]"));
            //если есть Cabinet, то удаляем
            if (IsElementPresent(By.XPath("(.//*[contains(text(),'" + Cabinet + "')])[1]")) == true)
            {
                Message = "   Deleting " + Cabinet;
                driver.FindElement(By.XPath(".//*[contains(text(),'" + Cabinet + "')]" +
                    "//ancestor::tr//*[contains(@class,'menu-dots')]")).Click();
                WaitUntilVisibleAndClick(By.XPath(".//*[contains(text(),'" + Cabinet + "')]//ancestor::tr//*[contains(text(),'Delete')]"));
                WaitUntilVisibleAndClick(By.XPath(".//*[contains(@class,'shadow-modal')]//*[contains(text(),'Delete')]"));
                WaitUntilInVisible(By.XPath(".//a[contains(text(),'" + Cabinet + "')]"));
                Console.WriteLine(Message);
            }
            //если есть editCabinet, то удаляем
            if (IsElementPresent(By.XPath("(.//*[contains(text(),'" + EditCabinet + "')])[1]")) == true)
            {

                Message = "   Deleting " + EditCabinet;
                driver.FindElement(By.XPath(".//*[contains(text(),'" + EditCabinet + "')]" +
                    "//ancestor::tr//*[contains(@class,'menu-dots')]")).Click();
                WaitUntilVisibleAndClick(By.XPath(".//*[contains(text(),'" + EditCabinet + "')]//ancestor::tr//*[contains(text(),'Delete')]"));
                WaitUntilVisibleAndClick(By.XPath(".//*[contains(@class,'shadow-modal')]//*[contains(text(),'Delete')]"));
                WaitUntilInVisible(By.XPath(".//a[contains(text(),'" + EditCabinet + "')]"));
                Console.WriteLine(Message);
            }
            Console.WriteLine("Adding the new user Cabinet " + Cabinet);
            Message = "  Field Validation";
            WaitUntilVisibleAndClick(By.XPath(".//button[contains(text(),'Add cabinet')]"));
            WaitUntilVisible(By.XPath(".//*[@type='submit']"));

            //checking required fields
            driver.FindElement(By.XPath(".//*[@id='Period']")).SendKeys("1");
            driver.FindElement(By.XPath(".//*[@type='submit']")).Click();
           WaitUntilVisible(By.XPath(".//*[@id='text']/following-sibling::span[contains(@class,'error')]"));
            WaitUntilVisible(By.XPath(".//*[@id='Licence']/following-sibling::span[contains(@class,'error')]"));
            Console.WriteLine(Message);
            //fill in the fields
            Message = "  Entering data into fields ";
            driver.FindElement(By.XPath(".//*[@id='text']")).SendKeys(Cabinet);
            driver.FindElement(By.XPath(".//*[@id='Licence']")).SendKeys(Cabinet);
           
            driver.FindElement(By.XPath(".//*[@type='submit']")).Click();
            WaitUntilVisible(By.XPath(".//button[contains(text(),'Add cabinet')]"));
            driver.FindElement(By.XPath("(.//*[contains(text(),'"+ Cabinet + "')])[1]"));
            Console.WriteLine(Message);

            //edit 
            Message = "editing";

            driver.FindElement(By.XPath(".//*[contains(text(),'" + Cabinet + "')]" +
                   "//ancestor::tr//*[contains(@class,'menu-dots')]")).Click();
            WaitUntilVisibleAndClick(By.XPath(".//*[contains(text(),'" + Cabinet + "')]//ancestor::tr//*[contains(text(),'Edit')]"));
            WaitUntilVisible(By.XPath(".//*[@id='text'][@value='" + Cabinet+"']"));
            driver.FindElement(By.XPath(".//*[@id='text']")).Clear();
            driver.FindElement(By.XPath(".//*[@id='text']")).SendKeys(EditCabinet);
            driver.FindElement(By.XPath(".//*[@id='cron_days']")).SendKeys("11");
            driver.FindElement(By.XPath(".//*[@id='license']")).Clear();
            driver.FindElement(By.XPath(".//*[@id='license']")).SendKeys(EditCabinet);
            driver.FindElement(By.XPath(".//*[@type='submit']")).Click();
            WaitUntilVisible(By.XPath(".//button[contains(text(),'Add cabinet')]"));
            driver.FindElement(By.XPath("(.//*[contains(text(),'" + EditCabinet + "')])[1]"));
            Console.WriteLine(Message);

            Message = "   Deleting " + Cabinet;
            driver.FindElement(By.XPath(".//*[contains(text(),'" + Cabinet + "')]" +
                "//ancestor::tr//*[contains(@class,'menu-dots')]")).Click();
            WaitUntilVisibleAndClick(By.XPath(".//*[contains(text(),'" + Cabinet + "')]//ancestor::tr//*[contains(text(),'Delete')]"));
            WaitUntilVisibleAndClick(By.XPath(".//*[contains(@class,'shadow-modal')]//*[contains(text(),'Delete')]"));
            WaitUntilInVisible(By.XPath(".//a[contains(text(),'" + Cabinet + "')]"));
            Console.WriteLine(Message);

        }


        [Test]
        [TestCaseSource(nameof(GetSites))]
        public void Moderate(string site)
        {
            driver.Navigate().GoToUrl(baseURL + "cabinet/records");
            Authorization(site, ModerateLogin);
            Message = "1. Records ";
            WaitUntilVisible(By.XPath("(.//*[@role='rowgroup']//a)[1]"));
            string recordId = driver.FindElement(By.XPath("(.//*[@role='rowgroup']//a)[1]")).Text;
            driver.FindElement(By.XPath("(.//*[@role='rowgroup']//a)[1]")).Click();
            Console.WriteLine(Message);
            Message = "2. Record " + recordId;
            WaitUntilVisible(By.XPath(".//h1[contains(text(),'"+ recordId + "')]"));
            WaitUntilVisibleAndClick(By.XPath("//button[contains(text(),'Moderate')]"));
            Console.WriteLine(Message);
            Message = "3. Moderate " + recordId;
            WaitUntilVisible(By.XPath("//h1[contains(text(),'Moderate record')]"));
            int i = 1;
            WaitUntilVisible(By.XPath("(.//*[contains(@class,'enter justify-between mt')][1]//label)[1]/span[1]"));
            Console.WriteLine(Message);
            Message = "  3.1. Mark all the questions ";
            while (IsElementPresent(By.XPath("((.//*[contains(@class,'enter justify-between mt')])["+i+"]" +
                "//label)[1]")) == true)
            {
                driver.FindElement(By.XPath("((.//*[contains(@class,'enter justify-between mt')])["+i+"]" +
                    "//label)[1]")).Click();
                i++;
            }
            Console.WriteLine(Message);
            Message = "  3.2. Mark attention and cheating ";
            driver.FindElement(By.XPath(".//*[@for='attention']")).Click();
            driver.FindElement(By.XPath(".//*[@for='cheating']")).Click();
            driver.FindElement(By.XPath(".//*[@type='submit']")).Click();
            Console.WriteLine(Message);
            Message = "4. Сhecking the record is missing in the Records";
            WaitUntilInVisible(By.XPath("(.//*[@role='rowgroup']//a)[contains(text(),'"+ recordId + "')]"));
            Message = "5. Сhecking the record is in Moderates";
            driver.Navigate().GoToUrl(baseURL + "cabinet/moderate");
            WaitUntilVisible(By.XPath("(.//*[@role='rowgroup']//a)[contains(text(),'" + recordId + "')]"));
            Console.WriteLine(Message);

        }
    }
}