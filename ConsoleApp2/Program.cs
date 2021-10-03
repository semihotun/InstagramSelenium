using OpenQA.Selenium;
using System.Threading;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System;
using System.Linq;
using OpenQA.Selenium.Support.UI;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            FindTheUnf();
        }
        public static IReadOnlyCollection<IWebElement> TakipEttiklerim(Bilgiler bilgi, IWebDriver driver)
        {
            driver.Navigate().GoToUrl($"https://www.instagram.com/" + bilgi.yonlenicektag);
            Thread.Sleep(7000);
            IWebElement takipEdilenButonu= driver.FindElement(By.CssSelector("#react-root > section > main > div > header > section > ul > li:nth-child(3) > a"));
            takipEdilenButonu.Click();
            Thread.Sleep(2500);
            string jsCommand = "" +
                "sayfa = document.querySelector('.isgrP');" +
                "sayfa.scrollTo(0,sayfa.scrollHeight);" +
                "var sayfaSonu = sayfa.scrollHeight;" +
                "return sayfaSonu;";

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            var sayfaSonu = Convert.ToInt32(js.ExecuteScript(jsCommand));

            while (true)
            {
                var son = sayfaSonu;
                Thread.Sleep(750);
                sayfaSonu = Convert.ToInt32(js.ExecuteScript(jsCommand));
                if (son == sayfaSonu)
                    break;
            }

            IReadOnlyCollection<IWebElement> follwers = driver.FindElements(By.CssSelector(".FPmhX.notranslate._0imsa"));
         
            return follwers;

        }

        public static void FindTheUnf()
        {
            var options = new OpenQA.Selenium.Chrome.ChromeOptions();
            options.BinaryLocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
            using (IWebDriver driver = new OpenQA.Selenium.Chrome.ChromeDriver(options))
            {

                var UnfList=new List<string>();
                var followerList = new List<string>();
                var followList = new List<string>();

                var bilgi = Login(driver);
                var follwersWebElement = Takipçiler(bilgi, driver);
                Thread.Sleep(7000);
                foreach (IWebElement item in follwersWebElement)
                {
                    followerList.Add(item.Text);
                }

                var followWebElement=TakipEttiklerim(bilgi,driver);
                foreach (IWebElement item in followWebElement)
                {
                    followList.Add(item.Text);
                }

                foreach (var item in followList)
                {
                    if(followerList.Where(x=>x == item).Count() <= 0)
                    {
                        UnfList.Add(item);
                        Console.WriteLine(item);
                    }
                }

                
            }
            Console.ReadKey();
        }

        public static IReadOnlyCollection<IWebElement> Takipçiler(Bilgiler bilgi, IWebDriver driver)
        {
            driver.Navigate().GoToUrl($"https://www.instagram.com/" + bilgi.yonlenicektag);
            Thread.Sleep(7000);
            IWebElement followerLink = driver.FindElement(By.CssSelector("#react-root > section > main > div > header > section > ul > li:nth-child(2) > a"));
            followerLink.Click();
            Thread.Sleep(5000);
            string jsCommand = "" +
                "sayfa = document.querySelector('.isgrP');" +
                "sayfa.scrollTo(0,sayfa.scrollHeight);" +
                "var sayfaSonu = sayfa.scrollHeight;" +
                "return sayfaSonu;";

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            var sayfaSonu = Convert.ToInt32(js.ExecuteScript(jsCommand));

            while (true)
            {
                var son = sayfaSonu;
                Thread.Sleep(750);
                sayfaSonu = Convert.ToInt32(js.ExecuteScript(jsCommand));
                if (son == sayfaSonu)
                    break;
            }

            IReadOnlyCollection<IWebElement> follwers = driver.FindElements(By.CssSelector(".FPmhX.notranslate._0imsa"));

            return follwers;
        }

        public static Bilgiler Login(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("https://www.instagram.com");
            Thread.Sleep(2000);
            IWebElement userName = driver.FindElement(By.Name("username"));
            IWebElement password = driver.FindElement(By.Name("password"));
            IWebElement girisbutonu = driver.FindElement(By.CssSelector(".sqdOP.L3NKy.y3zKF"));
            Bilgiler bilgi = new Bilgiler();
            bilgi.kullaniciAdi = "";
            bilgi.sifre = "";
            bilgi.yonlenicektag = "";
            userName.SendKeys(bilgi.kullaniciAdi);
            password.SendKeys(bilgi.sifre);
            girisbutonu.Click();
            Thread.Sleep(7000);

            return bilgi;
        }

        public static void NameFollow(IReadOnlyCollection<IWebElement> follwers, IWebDriver driver)
        {
            foreach (IWebElement follower in follwers)
            {
                try
                {
                    driver.Navigate().GoToUrl($"https://www.instagram.com/" + follower.Text);
                    var Followbtn = driver.FindElement(By.XPath("//button[. = 'Takip Et']"));
                    Followbtn.Click();
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
            var options = new OpenQA.Selenium.Chrome.ChromeOptions();
            options.BinaryLocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
            using (IWebDriver driver = new OpenQA.Selenium.Chrome.ChromeDriver(options))
            {

                var bilgi = Login(driver);
                var follwers = Takipçiler(bilgi, driver);
                NameFollow(follwers, driver);
            }
        }

        public static void EtiketTakip()
        {
            var options = new OpenQA.Selenium.Chrome.ChromeOptions();
            options.BinaryLocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
            using (IWebDriver driver = new OpenQA.Selenium.Chrome.ChromeDriver(options))
            {

                driver.Navigate().GoToUrl("https://www.instagram.com");
                Thread.Sleep(2000);
                IWebElement userName = driver.FindElement(By.Name("username"));
                IWebElement password = driver.FindElement(By.Name("password"));
                IWebElement girisbutonu = driver.FindElement(By.CssSelector(".sqdOP.L3NKy.y3zKF"));

                Bilgiler bilgi = new Bilgiler();
                bilgi.kullaniciAdi = "";
                bilgi.sifre = "";
                bilgi.yonlenicektag = "";

                userName.SendKeys(bilgi.kullaniciAdi);
                password.SendKeys(bilgi.sifre);
                girisbutonu.Click();
                Thread.Sleep(9000);
                var gototagurl = "https://www.instagram.com/explore/tags/" + bilgi.yonlenicektag + "/";
                driver.Navigate().GoToUrl(gototagurl);
                Thread.Sleep(3500);
                IWebElement firtpicture = driver.FindElement(By.CssSelector("._9AhH0"));
                firtpicture.Click();
                while (true)
                {
                    try
                    {
                        IWebElement prevnex = driver.FindElement(By.CssSelector("._65Bje.coreSpriteRightPaginationArrow"));
                        prevnex.Click();
                        Thread.Sleep(2500);
                        IWebElement followbtn = driver.FindElement(By.CssSelector(".sqdOP.yWX7d.y3zKF"));
                        followbtn.Click();
                        Thread.Sleep(2500);
                    }
                    catch { }
                }
                var Followbtn = driver.FindElement(By.XPath("//button[. = 'Takip Et']"));
                Followbtn.Click();

            }
        }

        public static void İnstagramKayit()
        {
            var options = new OpenQA.Selenium.Chrome.ChromeOptions();
            options.BinaryLocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
            using (IWebDriver driver = new OpenQA.Selenium.Chrome.ChromeDriver(options))
            {

                #region Tempmail
                driver.Navigate().GoToUrl("https://tr.emailfake.com/");
                var tempmail = "";
                try
                {
                    tempmail = driver.FindElement(By.Id("userName")).GetAttribute("value") + "@" + driver.FindElement(By.Id("domainName2")).GetAttribute("value");
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
                IWebElement fullname = driver.FindElement(By.Name("fullName"));
                IWebElement username = driver.FindElement(By.Name("username"));
                IWebElement password = driver.FindElement(By.Name("password"));
                IWebElement Kayıtbutton = driver.FindElement(By.XPath("//button[. = 'Kaydol']"));
                User user = new User();
                user.adsoyad = "";
                user.eposta = tempmail;
                user.kullaniciadi = "";
                user.sifre = "";
                email.SendKeys(user.eposta);
                fullname.SendKeys(user.adsoyad);
                username.SendKeys(user.kullaniciadi);
                password.SendKeys(user.sifre);
                Kayıtbutton.Click();
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

                driver.SwitchTo().Window(driver.WindowHandles.First());
                #endregion

                var codelink = "";
                var sayaç = 0;
                while (true)
                {

                    try
                    {
                        sayaç++;
                        if (sayaç == 3000)
                        {
                            Thread.Sleep(2500);
                            driver.SwitchTo().Window(driver.WindowHandles.Last());
                            IWebElement kodutekrargonder = driver.FindElement(By.XPath("//button[. = 'Kodu Tekrar Gönder.']"));
                            kodutekrargonder.Click();
                            Thread.Sleep(2500);
                            driver.SwitchTo().Window(driver.WindowHandles.First());
                            sayaç = 0;
                        }
                        codelink = driver.FindElement(By.CssSelector("#email_content > table > tbody > tr:nth-child(4) > td > table > tbody > tr > td > table > tbody > tr:nth-child(2) > td:nth-child(2) > table > tbody > tr:nth-child(2) > td:nth-child(2)")).GetAttribute("innerHTML");

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

                    IWebElement emailconfirmationcode = driver.FindElement(By.Name("email_confirmation_code"));
                    emailconfirmationcode.SendKeys(codelink);

                    IWebElement Kayıtbuttonİleri = driver.FindElement(By.XPath("//button[. = 'İleri']"));
                    Kayıtbuttonİleri.Click();


                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }

                Thread.Sleep(20000);




            }

        }
    }
}
