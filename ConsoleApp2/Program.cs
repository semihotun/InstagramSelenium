using OpenQA.Selenium;
using System.Threading;
using OpenQA.Selenium.Chrome;
namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new OpenQA.Selenium.Chrome.ChromeOptions();
            options.BinaryLocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
            using (IWebDriver driver = new OpenQA.Selenium.Chrome.ChromeDriver(options))
            {

                //IWebDriver driver = new ChromeDriver();
                driver.Navigate().GoToUrl("https://www.instagram.com");
                Thread.Sleep(2000);
                IWebElement userName = driver.FindElement(By.Name("username"));
                IWebElement password = driver.FindElement(By.Name("password"));
                IWebElement girisbutonu = driver.FindElement(By.CssSelector(".sqdOP.L3NKy.y3zKF"));

                Bilgiler bilgi = new Bilgiler();
                bilgi.kullaniciAdi = "";
                bilgi.sifre = "";
                bilgi.yonlenicektag = "izmir";

                userName.SendKeys(bilgi.kullaniciAdi);
                password.SendKeys(bilgi.sifre);
                girisbutonu.Click();
                Thread.Sleep(2500);
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
    }
}
