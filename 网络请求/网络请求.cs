/*
Get请求
public static string GetHttpResponse(string url, int Timeout)//Get请求 (地址,等待响应时间)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = Timeout;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            //ReceivedData=retString;

            return retString;
        }
Post请求
public static string HttpPost(string Url, string postDataStr)//Post请求 (地址,回传内容)
{
    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
    request.Method = "POST";
    request.ContentType = "application/x-www-form-urlencoded";
    request.ContentLength = postDataStr.Length;
    StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
    writer.Write(postDataStr);
    writer.Flush();
    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    string encoding = response.ContentEncoding;
    if (encoding == null || encoding.Length < 1)
    {
        encoding = "UTF-8"; //默认编码 
    }
    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
    string retString = reader.ReadToEnd();
    return retString;
}













 */