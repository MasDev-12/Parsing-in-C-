using HtmlAgilityPack;
using System.Collections;
using System.Net.Http;
using System.Text.RegularExpressions;

class Program
{
    static HttpClient httpClient = new HttpClient();
    static void Main()
    {
        //string search = Console.ReadLine();//Romeo and Juliet
        Console.WriteLine("Parsing started");
        Thread.Sleep(1000);
        string url = $"https://www.nur.kz/";
        ArrayList? result = Parsing(url);

        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, result[0].ToString()))
        {
            httpClient.SendAsync(request);
            using (HttpResponseMessage response = httpClient.GetAsync(url).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrEmpty(content))
                    {
                        Console.WriteLine(content);
                    }
                }
            }
        }
    }

    private static ArrayList Parsing(string url)
    {
        using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
        {
            ArrayList arrayList = new ArrayList();
            httpClient.SendAsync(request);
            int count = 0;//первые пять ссылок
            using (HttpResponseMessage response = httpClient.GetAsync(url).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrEmpty(content))
                    {
                        HtmlDocument htmlDocument = new HtmlDocument();
                        htmlDocument.LoadHtml(content);

                        foreach (HtmlNode node in htmlDocument.DocumentNode.SelectNodes("//li[contains(@class,'block-top-hero__item block-top-hero__item--big')]//a[@href]"))
                        {
                            Console.WriteLine(url + " " +node.GetAttributeValue("href", null));
                            arrayList.Add(url + " " +node.GetAttributeValue("href", null));
                            count++;
                            if (count==3)
                            {
                                break;
                            }
                        }

                        //var books = htmlDocument.DocumentNode.SelectNodes(".//div[@class='body']//ul[@class='results']//li[@class='booklink']//");
                        //if (books!=null && books.Count>0)
                        //{
                        //    foreach (var book in books)
                        //    {
                        //        var titleNode = book.SelectSingleNode(".//a[@class='link']");
                        //        if (titleNode!=null)
                        //        {

                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    Console.WriteLine("empty");
                        //}
                    }
                }
                return arrayList;
            }
        }
    }
}