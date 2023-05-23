using System;
using NUnit.Framework;
using OpenQA.Selenium;

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
            Message = "2. Search through a filter the user " + User;
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
                Message = "  2.1 Deleting " + User;
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
            Console.WriteLine("3. Adding the new user " + User);
            Message = "  3.1 Field Validation";
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

            Message = "  3.2 Entering data into fields ";
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
            Message = "4. User editing";
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
            Message = "5. Deleting user " + EditUser;
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
            Authorization(site, AdminLogin);
            Message = "2. Page of Cabinets";
            WaitUntilVisible(By.XPath(".//*[@role='rowgroup']/tr[1]"));
            Console.WriteLine(Message);
            //если есть Cabinet, то удаляем
            Console.WriteLine("3. Check if there are cabinets, if there are, then delete");
            if (IsElementPresent(By.XPath("(.//*[contains(text(),'" + Cabinet + "')])[1]")) == true)
            {
                Message = "   3.1. Deleting " + Cabinet;
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

                Message = "   3.2. Deleting " + EditCabinet;
                driver.FindElement(By.XPath(".//*[contains(text(),'" + EditCabinet + "')]" +
                    "//ancestor::tr//*[contains(@class,'menu-dots')]")).Click();
                WaitUntilVisibleAndClick(By.XPath(".//*[contains(text(),'" + EditCabinet + "')]//ancestor::tr//*[contains(text(),'Delete')]"));
                WaitUntilVisibleAndClick(By.XPath(".//*[contains(@class,'shadow-modal')]//*[contains(text(),'Delete')]"));
                WaitUntilInVisible(By.XPath(".//a[contains(text(),'" + EditCabinet + "')]"));
                Console.WriteLine(Message);
            }
            Console.WriteLine("4. Adding the new Cabinet " + Cabinet);
            Message = "  4.1.  Field Validation";
            WaitUntilVisibleAndClick(By.XPath(".//button[contains(text(),'Add cabinet')]"));
            WaitUntilVisible(By.XPath(".//*[@type='submit']"));
            //checking required fields
            driver.FindElement(By.XPath(".//*[@id='Period']")).SendKeys("1");
            driver.FindElement(By.XPath(".//*[@type='submit']")).Click();
           WaitUntilVisible(By.XPath(".//*[@id='text']/following-sibling::span[contains(@class,'error')]"));
            WaitUntilVisible(By.XPath(".//*[@id='Licence']/following-sibling::span[contains(@class,'error')]"));
            Console.WriteLine(Message);
            //fill in the fields
            Message = "  4.2. Entering data into fields ";
            driver.FindElement(By.XPath(".//*[@id='text']")).SendKeys(Cabinet);
            driver.FindElement(By.XPath(".//*[@id='Licence']")).SendKeys(Cabinet);
            driver.FindElement(By.XPath(".//*[@type='submit']")).Click();
            WaitUntilVisible(By.XPath(".//button[contains(text(),'Add cabinet')]"));
            driver.FindElement(By.XPath("(.//*[contains(text(),'"+ Cabinet + "')])[1]"));
            Console.WriteLine(Message);

            //edit 
            Message = "5. Editing";
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

            Message = "6. Deleting " + Cabinet;
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
            Message = "2. Page of Records ";
            WaitUntilVisible(By.XPath("(.//*[@role='rowgroup']//a)[1]"));
            string recordId = driver.FindElement(By.XPath("(.//*[@role='rowgroup']//a)[1]")).Text;
            driver.FindElement(By.XPath("(.//*[@role='rowgroup']//a)[1]")).Click();
            Console.WriteLine(Message);
            Message = "3. Record " + recordId;
            WaitUntilVisible(By.XPath(".//h1[contains(text(),'"+ recordId + "')]"));
            WaitUntilVisibleAndClick(By.XPath("//button[contains(text(),'Moderate')]"));
            Console.WriteLine(Message);
            Message = "4. Moderate " + recordId;
            WaitUntilVisible(By.XPath("//h1[contains(text(),'Moderate record')]"));
            int i = 1;
            WaitUntilVisible(By.XPath("(.//*[contains(@class,'enter justify-between mt')][1]//label)[1]/span[1]"));
            Console.WriteLine(Message);
            Message = "  4.1. Mark all the questions ";
            while (IsElementPresent(By.XPath("((.//*[contains(@class,'enter justify-between mt')])["+i+"]" +
                "//label)[1]")) == true)
            {
                driver.FindElement(By.XPath("((.//*[contains(@class,'enter justify-between mt')])["+i+"]" +
                    "//label)[1]")).Click();
                i++;
            }
            Console.WriteLine(Message);
            Message = "  4.2. Mark attention and cheating ";
            driver.FindElement(By.XPath(".//*[@for='attention']")).Click();
            driver.FindElement(By.XPath(".//*[@for='cheating']")).Click();
            driver.FindElement(By.XPath(".//*[@type='submit']")).Click();
            Console.WriteLine(Message);
            Message = "5. Сhecking the record is missing in the Records";
            WaitUntilInVisible(By.XPath("(.//*[@role='rowgroup']//a)[contains(text(),'"+ recordId + "')]"));
            Console.WriteLine(Message);
            Message = "6. Сhecking the record is in Moderates";
            driver.Navigate().GoToUrl(baseURL + "cabinet/moderate");
            WaitUntilVisible(By.XPath("(.//*[@role='rowgroup']//a)[contains(text(),'" + recordId + "')]"));
            Console.WriteLine(Message);
       
        }


        [Test]
        [TestCaseSource(nameof(GetSites))]
        public void Glue(string site)
        {
            driver.Navigate().GoToUrl(baseURL + "cabinet/records");
            Authorization(site, AdminLogin);
            Message = "2. Page of Records";
            WaitUntilVisible(By.XPath("(.//*[@role='rowgroup']//a)[1]"));
            Console.WriteLine(Message);
            Message = "3. Glu 2 records ";
            WaitUntilVisibleAndClick(By.XPath(".//label[@for='search-glue']"));
            WaitUntilVisible(By.XPath(".//*[contains(@class,'btn btn-outlined')][@disabled]"));//the button Glu isnt active
            WaitUntilVisibleAndClick(By.XPath("(.//*[@role='rowgroup']//*[contains(@for,'search-glue')])[1]"));
            driver.FindElement(By.XPath("(.//*[@role='rowgroup']//*[contains(@for,'search-glue')])[2]")).Click();
           string Record2 = driver.FindElement(By.XPath("(.//*[@role='rowgroup']//a)[1]")).Text;
            string Record1 = driver.FindElement(By.XPath("(.//*[@role='rowgroup']//tr[2]//a)[1]")).Text;
            WaitUntilInVisible(By.XPath(".//*[contains(@class,'btn btn-outlined')][@disabled]"));//the button Glu is active
            driver.FindElement(By.XPath(".//*[contains(@class,'btn btn-outlined')]")).Click();
            
            
            Console.WriteLine(Message + Record1 + ", " + Record2);
            Message = "4. Check info on Page of glue record ";
             WaitUntilVisible(By.XPath(".//*[contains(text(),'Record is glued')]"));
            string GluRecord = driver.FindElement(By.XPath(".//*[@class='text-xl font-semibold uppercase']")).Text;
            Message += GluRecord;
            WaitUntilInVisible(By.XPath(".//*[@class='wrapper-preloader']/div"));//preloDER isnt
            WaitUntilVisible(By.XPath(".//*[contains(text(),'"+ Record1 + ","+ Record2 + "')]"));
            Console.WriteLine(Message);
            Message = "5. Click 'Back'";
            driver.FindElement(By.XPath(".//*[contains(text(),'Back')]")).Click();
            Console.WriteLine(Message);
            Message = "  5.1 Check the absence of the glue record and the presence of the old records";
            WaitUntilVisible(By.XPath("(.//*[@role='rowgroup']//a)[contains(text(),'"+ GluRecord + "')]"));
            Console.WriteLine(Message);
            Message = "  5.2 Check the presence of the old records";
            WaitUntilInVisible(By.XPath("(.//*[@role='rowgroup']//a)[contains(text(),'" + Record2 + "')]"));
            WaitUntilInVisible(By.XPath("(.//*[@role='rowgroup']//a)[contains(text(),'" + Record1 + "')]"));
            Console.WriteLine(Message);
        
        }


        [Test]
        [TestCaseSource(nameof(GetSites))]
        public void DeleteRecord(string site)
        {
            driver.Navigate().GoToUrl(baseURL + "cabinet/records");
            Authorization(site, ModerateLogin);
            Message = "2. Page of Records";
            WaitUntilVisible(By.XPath("(.//*[@role='rowgroup']//a)[1]"));
            string Record1 = driver.FindElement(By.XPath("(.//*[@role='rowgroup']//a)[1]")).Text;
            string Record2 = driver.FindElement(By.XPath("(.//*[@role='rowgroup']//tr[2]//a)[1]")).Text;
            Console.WriteLine(Message);

            Message = "3. Delete record in the Records list ";
            driver.FindElement(By.XPath(".//a[contains(text(),'" + Record1 + "')]" +
                "//ancestor::tr//*[contains(@class,'menu-dots')]")).Click();
            WaitUntilVisibleAndClick(By.XPath(".//a[contains(text(),'" + Record1 + "')]" +
                "//ancestor::tr//*[contains(@class,'shadow-menu ')]//*[contains(text(),'Delete')]"));
            WaitUntilVisibleAndClick(By.XPath("//*[contains(@class,'body shadow-modal')]//button[contains(text(),'Delete')]"));
            WaitUntilInVisible(By.XPath(".//a[contains(text(),'" + Record1 + "')]"));
            Console.WriteLine(Message + Record1);

            Message = "4. Delete the record on the page of record ";
            driver.FindElement(By.XPath("(.//*[@role='rowgroup']//a)[contains(text(),'" + Record2 + "')]")).Click();
            WaitUntilVisible(By.XPath("//button[contains(text(),'Delete')]"));
            WaitUntilInVisible(By.XPath(".//*[@class='wrapper-preloader']/div"));//preloDER isnt
            driver.FindElement(By.XPath("//button[contains(text(),'Delete')]")).Click();
            WaitUntilVisibleAndClick(By.XPath("//*[contains(@class,'body shadow-modal')]//button[contains(text(),'Delete')]"));
            WaitUntilVisible(By.XPath("//*[contains(text(),'Record " + Record2 + " deleted successfully')]"));
            WaitUntilInVisible(By.XPath("(.//*[@role='rowgroup']//a)[contains(text(),'" + Record2 + "')]"));
            Console.WriteLine(Message + Record2);

        }


        [Test]
        [TestCaseSource(nameof(GetSites))]
        public void City(string site)
        {
            string City = "CityAutoTest";
            string EditCity = "EditCityAutoTest";
            driver.Navigate().GoToUrl(baseURL + "cabinet/cities");
            Authorization(site, AdminLogin);
            Message = "2. Search through a filter the sity " + City;
            WaitUntilVisible(By.XPath(".//*[@role='rowgroup']/tr[1]"));
            WaitUntilVisible(By.XPath(".//*[@id='cabinet-search']"));//filter
            driver.FindElement(By.XPath(".//*[@id='cabinet-search']")).SendKeys(City);
            string Tolal = driver.FindElement(By.XPath(".//*[contains(text(),'Total')]")).Text;
            driver.FindElement(By.XPath(".//*[@type='submit']")).Click();

            //ждем прелоадер появится, исчезнет
            WaitUntilVisibleInText(By.XPath(".//*[contains(text(),'Total')]"), Tolal);
            Console.WriteLine(Message);
            //если есть юзер, то удаляем
            if (IsElementPresent(By.XPath(".//div[contains(text(),'" + City + "')]")) == true)
            {
                Message = "  2.1 Deleting " + City;
                driver.FindElement(By.XPath(".//*[@role='cell']//*[contains(@class,'menu-dots')]")).Click();
                WaitUntilVisibleAndClick(By.XPath(".//*[@role='cell']//*[contains(text(),'Delete')]"));
                WaitUntilVisibleAndClick(By.XPath(".//*[contains(@class,'shadow-modal')]//*[contains(text(),'Delete')]"));
                WaitUntilInVisible(By.XPath(".//a[contains(text(),'" + City + "')]"));
                Console.WriteLine(Message);
            }
            //если есть editюзер, то удаляем
            if (IsElementPresent(By.XPath(".//div[contains(text(),'" + EditCity + "')]")) == true)
            {
                Message = "Delete " + EditCity;
                driver.FindElement(By.XPath(".//*[@role='cell']//*[contains(@class,'menu-dots')]")).Click();
                WaitUntilVisibleAndClick(By.XPath(".//*[@role='cell']//*[contains(text(),'Delete')]"));
                WaitUntilVisibleAndClick(By.XPath(".//*[contains(@class,'shadow-modal')]//*[contains(text(),'Delete')]"));
                WaitUntilInVisible(By.XPath(".//a[contains(text(),'" + EditCity + "')]"));
                Console.Write(Message);
            }
            Console.WriteLine("3. Adding the new City " + City);
            Message = "  3.1 Field Validation";
            WaitUntilVisibleAndClick(By.XPath(".//button[contains(text(),'Add city')]"));
            WaitUntilVisible(By.XPath(".//*[@type='submit']"));
            driver.FindElement(By.XPath(".//*[@type='submit']")).Click();
            //if (IsElementPresent(By.XPath(".//*[@id='text']/following-sibling::span[contains(@class,'error')]")) == false)
            //{
            //    driver.FindElement(By.XPath(".//*[@type='submit']")).Click();
            //}
            WaitUntilVisible(By.XPath(".//*[@id='text']/following-sibling::span[contains(@class,'error')]"));
            Console.WriteLine(Message);

            Message = "  3.2 Entering data into fields ";
            driver.FindElement(By.XPath(".//*[@id='text']")).SendKeys(City);
            driver.FindElement(By.XPath(".//*[@type='submit']")).Click();
            WaitUntilVisible(By.XPath(".//button[contains(text(),'Add city')]"));
            driver.FindElement(By.XPath(".//div[contains(text(),'" + City + "')]"));
            Console.WriteLine(Message);

            //edit the user
            Message = "4. User editing";
            driver.FindElement(By.XPath(".//*[@role='cell']//*[contains(@class,'menu-dots')]")).Click();
            WaitUntilVisibleAndClick(By.XPath(".//*[@role='cell']//*[contains(text(),'Edit')]"));
            WaitUntilVisible(By.XPath(".//*[@id='text'][@value='"+ City + "']"));
            driver.FindElement(By.XPath(".//*[@id='text']")).Clear();
            driver.FindElement(By.XPath(".//*[@id='text']")).SendKeys(EditCity);
            driver.FindElement(By.XPath(".//*[@type='submit']")).Click();
            WaitUntilVisible(By.XPath(".//button[contains(text(),'Add city')]"));
            WaitUntilVisible(By.XPath(".//div[contains(text(),'" + EditCity + "')]"));
            Console.WriteLine(Message);

            //deleting the user
            Message = "5. Deleting city " + EditCity;
            driver.FindElement(By.XPath(".//*[@role='cell']//*[contains(@class,'menu-dots')]")).Click();
            WaitUntilVisibleAndClick(By.XPath(".//*[@role='cell']//*[contains(text(),'Delete')]"));
            WaitUntilVisibleAndClick(By.XPath(".//*[contains(@class,'shadow-modal')]//*[contains(text(),'Delete')]"));
            WaitUntilInVisible(By.XPath(".//div[contains(text(),'" + EditCity + "')]"));
            Console.WriteLine(Message);
        }







    }
}