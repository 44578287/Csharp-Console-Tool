

using Json解析;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/*string Json = "[\"mc.445720.xyz\",\"10.10.10.8\"]";
string Json1 = "{\"VAR\":[\"mc.445720.xyz\",\"10.10.10.8\"]}";

//Console.WriteLine(Json);
//Console.WriteLine(处理.单层数组解析(Json, ""));

JObject jArray = (JObject)JsonConvert.DeserializeObject(Json);
//JsonConvert.SerializeObject(ss);

string[] ArrayData = new string[jArray.Count];

for (int i = 0; i < jArray.Count; i++)
{
    JObject JsonData = JObject.Parse(jArray[i].ToString());
    ArrayData[i] = JsonData[""].ToString();
}
Console.WriteLine(ArrayData);*/


string Json = "{\"服务器整合\":{\"模组\":{\"id\":1,\"模组包名\":\"123\"},\"光影\":{\"id\":1,\"光影包名\":\"123\"},\"材质\":{\"id\":1,\"材质包名\":\"123\"}}}";


Console.WriteLine(处理.单层对象解析(处理.单层对象解析(处理.单层对象解析(Json, "服务器整合"), "模组"),"id"));