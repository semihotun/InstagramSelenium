using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
namespace ConsoleApp2
{
    class Program
    {
        [DllImport("wininet.dll")]
        public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        public const int INTERNET_OPTION_REFRESH = 37;
        static void Main(string[] args)
        {
            RegisterInstagram();
        }
        public static IReadOnlyCollection<IWebElement> MyFollow(Information info, IWebDriver driver)
        {
            driver.Navigate().GoToUrl("https://www.instagram.com/" + info.DirectedTag);
            Thread.Sleep(7000);
            IWebElement followedBtn = driver.FindElement(By.CssSelector("#react-root > section > main > div > header > section > ul > li:nth-child(3) > a"));
            followedBtn.Click();
            Thread.Sleep(2500);
            const string jsCommand = "" +
                "sayfa = document.querySelector('.isgrP');" +
                "sayfa.scrollTo(0,sayfa.scrollHeight);" +
                "var sayfaSonu = sayfa.scrollHeight;" +
                "return sayfaSonu;";
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            var pageEnd = Convert.ToInt32(js.ExecuteScript(jsCommand));
            while (true)
            {
                var end = pageEnd;
                Thread.Sleep(750);
                pageEnd = Convert.ToInt32(js.ExecuteScript(jsCommand));
                if (end == pageEnd)
                    break;
            }
            return driver.FindElements(By.CssSelector(".FPmhX.notranslate._0imsa"));
        }
        public static void FindTheUnf()
        {
            var options = new OpenQA.Selenium.Chrome.ChromeOptions
            {
                BinaryLocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe"
            };
            using (IWebDriver driver = new OpenQA.Selenium.Chrome.ChromeDriver(options))
            {
                var unfList = new List<string>();
                var followerList = new List<string>();
                var followList = new List<string>();
                var info = Login(driver);
                var follwersWebElement = Followers(info, driver);
                Thread.Sleep(7000);
                foreach (IWebElement item in follwersWebElement)
                {
                    followerList.Add(item.Text);
                }
                foreach (IWebElement item in MyFollow(info, driver))
                {
                    followList.Add(item.Text);
                }
                foreach (var item in followList)
                {
                    if (followerList.Count(x => x == item) <= 0)
                    {
                        unfList.Add(item);
                        Console.WriteLine(item);
                    }
                }
            }
            Console.ReadKey();
        }
        public static IReadOnlyCollection<IWebElement> Followers(Information info, IWebDriver driver)
        {
            driver.Navigate().GoToUrl("https://www.instagram.com/" + info.DirectedTag);
            Thread.Sleep(7000);
            IWebElement followerLink = driver.FindElement(By.CssSelector("#react-root > section > main > div > header > section > ul > li:nth-child(2) > a"));
            followerLink.Click();
            Thread.Sleep(5000);
            const string jsCommand = "" +
                "sayfa = document.querySelector('.isgrP');" +
                "sayfa.scrollTo(0,sayfa.scrollHeight);" +
                "var sayfaSonu = sayfa.scrollHeight;" +
                "return sayfaSonu;";
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            var endPage = Convert.ToInt32(js.ExecuteScript(jsCommand));
            while (true)
            {
                var end = endPage;
                Thread.Sleep(750);
                endPage = Convert.ToInt32(js.ExecuteScript(jsCommand));
                if (end == endPage)
                    break;
            }
            return driver.FindElements(By.CssSelector(".FPmhX.notranslate._0imsa"));
        }
        public static Information Login(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("https://www.instagram.com");
            Thread.Sleep(2000);
            IWebElement userName = driver.FindElement(By.Name("username"));
            IWebElement password = driver.FindElement(By.Name("password"));
            IWebElement loginBtn = driver.FindElement(By.CssSelector(".sqdOP.L3NKy.y3zKF"));
            var info = new Information();
            userName.SendKeys(info.UserName);
            password.SendKeys(info.Password);
            loginBtn.Click();
            Thread.Sleep(7000);
            return info;
        }
        public static void NameFollow(IReadOnlyCollection<IWebElement> follwers, IWebDriver driver)
        {
            foreach (IWebElement follower in follwers)
            {
                try
                {
                    driver.Navigate().GoToUrl("https://www.instagram.com/" + follower.Text);
                    var followBtn = driver.FindElement(By.XPath("//button[. = 'Takip Et']"));
                    followBtn.Click();
                    Thread.Sleep(2500);
                }
                catch (Exception)
                {
                    Thread.Sleep(2500);
                }
            }
        }
        public static void Follow()
        {
            var options = new OpenQA.Selenium.Chrome.ChromeOptions
            {
                BinaryLocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe"
            };
            using IWebDriver driver = new OpenQA.Selenium.Chrome.ChromeDriver(options);
            var follwers = Followers(Login(driver), driver);
            NameFollow(follwers, driver);
        }
        public static void TagTracking()
        {
            var options = new OpenQA.Selenium.Chrome.ChromeOptions
            {
                BinaryLocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe"
            };
            using IWebDriver driver = new OpenQA.Selenium.Chrome.ChromeDriver(options);
            driver.Navigate().GoToUrl("https://www.instagram.com");
            Thread.Sleep(2000);
            IWebElement userName = driver.FindElement(By.Name("username"));
            IWebElement password = driver.FindElement(By.Name("password"));
            IWebElement loginBtn = driver.FindElement(By.CssSelector(".sqdOP.L3NKy.y3zKF"));
            var info = new Information();
            userName.SendKeys(info.UserName);
            password.SendKeys(info.Password);
            loginBtn.Click();
            Thread.Sleep(9000);
            driver.Navigate().GoToUrl("https://www.instagram.com/explore/tags/" + info.DirectedTag + "/");
            Thread.Sleep(3500);
            IWebElement firstPicture = driver.FindElement(By.CssSelector("._9AhH0"));
            firstPicture.Click();
            while (true)
            {
                try
                {
                    IWebElement prevNext = driver.FindElement(By.CssSelector("._65Bje.coreSpriteRightPaginationArrow"));
                    prevNext.Click();
                    Thread.Sleep(2500);
                    IWebElement followBtn = driver.FindElement(By.CssSelector(".sqdOP.yWX7d.y3zKF"));
                    followBtn.Click();
                    Thread.Sleep(2500);
                }
                catch { }
            }
        }
        public static void RegisterInstagram()
        {
            var options = new OpenQA.Selenium.Chrome.ChromeOptions();
            var proxy = new Proxy
            {
                HttpProxy = "111.90.179.74:8080"
            };
            options.Proxy = proxy;
            options.BinaryLocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
            using IWebDriver driver = new OpenQA.Selenium.Chrome.ChromeDriver(options);
            #region Tempmail
            driver.Navigate().GoToUrl("https://tr.emailfake.com/");
            var tempMail = "";
            try
            {
                tempMail = driver.FindElement(By.Id("userName")).GetAttribute("value") + "@" + driver.FindElement(By.Id("domainName2")).GetAttribute("value");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            #region instagramkayıt
                ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            driver.Navigate().GoToUrl("https://www.instagram.com/accounts/emailsignup/");
            Thread.Sleep(2000);
            IWebElement email = driver.FindElement(By.Name("emailOrPhone"));
            IWebElement fullName = driver.FindElement(By.Name("fullName"));
            IWebElement username = driver.FindElement(By.Name("username"));
            IWebElement password = driver.FindElement(By.Name("password"));
            IWebElement registerBtn = driver.FindElement(By.XPath("//button[. = 'Kaydol']"));
            User user = new User
            {
                NameSurname = "sohistory14",
                Mail = tempMail,
                UserName = "sohistory14",
                Password = "historyso2q"
            };
            email.SendKeys(user.Mail);
            fullName.SendKeys(user.NameSurname);
            username.SendKeys(user.UserName);
            password.SendKeys(user.Password);
            registerBtn.Click();
            Thread.Sleep(5000);
            SelectElement oSelect = new SelectElement(driver.FindElement(By.XPath("//*[@title='Ay:']")));
            oSelect.SelectByText("Mayıs");
            SelectElement oSelect2 = new SelectElement(driver.FindElement(By.XPath("//*[@title='Gün:']")));
            oSelect2.SelectByText("1");
            SelectElement oSelect3 = new SelectElement(driver.FindElement(By.XPath("//*[@title='Yıl:']")));
            oSelect3.SelectByText("1998");
            IWebElement ileributton = driver.FindElement(By.XPath("//button[. = 'İleri']"));
            ileributton.Click();
            IWebElement body2 = driver.FindElement(By.TagName("body"));
            body2.SendKeys(Keys.Alt + Keys.Tab);
            driver.SwitchTo().Window(driver.WindowHandles[0]);
            #endregion
            var codeLink = "";
            var counter = 0;
            while (true)
            {
                try
                {
                    if (!string.IsNullOrEmpty(codeLink))
                    {
                        Thread.Sleep(2500);
                        driver.SwitchTo().Window(driver.WindowHandles.Last());
                        IWebElement resendTheCode = driver.FindElement(By.XPath("//button[. = 'Kodu Tekrar Gönder.']"));
                        resendTheCode.Click();
                        Thread.Sleep(2500);
                        driver.SwitchTo().Window(driver.WindowHandles[0]);
                        counter = 0;
                    }
                    codeLink = driver.FindElement(By.CssSelector("#email_content > table > tbody > tr:nth-child(4) > td > table > tbody > tr > td > table > tbody > tr:nth-child(2) > td:nth-child(2) > table > tbody > tr:nth-child(2) > td:nth-child(2)")).GetAttribute("innerHTML");
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            try
            {
                IWebElement emailConfirmationCode = driver.FindElement(By.Name("email_confirmation_code"));
                emailConfirmationCode.SendKeys(codeLink);
                IWebElement registerBtnNext = driver.FindElement(By.XPath("//button[. = 'İleri']"));
                registerBtnNext.Click();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Thread.Sleep(20000);
            Console.ReadKey();
        }
    }
}
