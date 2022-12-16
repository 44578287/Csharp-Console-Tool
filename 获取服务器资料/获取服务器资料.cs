using System.Net;
using System.Text.RegularExpressions;




string url = "http://mc.445720.xyz:8877/";
HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
List<string> list = new List<string>();
using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
{
    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
    {
        Console.WriteLine(response.LastModified);
        string html = reader.ReadToEnd();
        Regex regex = new Regex("<A HREF=\".*\">(?<name>.*)</A>");
        MatchCollection matches = regex.Matches(html);

        if (matches.Count > 0)
        {
            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    string[] matchData = match.Groups[0].ToString().Split('\"');
                    if (matchData[1].EndsWith("html"))
                    {
                        list.Add(matchData[1]);
                        Console.WriteLine(matchData[1]);
                    }
                }
            }
        }

    }


    Console.WriteLine();
}
string path = Path.Combine(url, list[0]);
request = (HttpWebRequest)WebRequest.Create(path);
var response1 = (HttpWebResponse)request.GetResponse();
Console.WriteLine(response1.LastModified);






Console.ReadLine();