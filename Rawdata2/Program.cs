using DomainModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace EchoServer
{
    public static class Program
    {

        public static void Main(string[] args)
        {
            var addr = IPAddress.Parse("127.0.0.1");
            var server = new TcpListener(addr, 5000);

            server.Start();
            Console.WriteLine("Server started ...");


            while (true)
            {
                var client = server.AcceptTcpClient();
                Console.WriteLine("Client connected");
                new Thread(() => ProcessData(client)).Start();

            }

            //server.Stop();

        }

        public static void ProcessData(TcpClient client)
        {
            var strm = client.GetStream();

            var buffer = new byte[client.ReceiveBufferSize];
            var readCnt = strm.Read(buffer, 0, buffer.Length);

            var payload = Encoding.UTF8.GetString(buffer, 0, readCnt);
            var request = JsonConvert.DeserializeObject<Request>(payload);

            Console.WriteLine(request.Body);
            Console.WriteLine("Soo far");
            var res = Encoding.UTF8.GetBytes(GetStatus(request));

            strm.Write(res, 0, res.Length);
        }

        public static string GetStatus(Request request)
        {
            var result = new Response();


            //check method
            switch (request.Method)
            {
                case "create":
                    result.Status = "2 ok";
                    break;
                case "read":
                    result.Status = "2 ok";
                    break;
                case "update":
                    result.Status = "2 ok";
                    break;
                case "delete":
                    result.Status = "2 ok";
                    break;
                case "echo":
                    result.Status = "2 ok";
                    break;
                case "":
                    result.Status += "4 missing method";
                    break;
                default:
                    result.Status += "4 illegal method";
                    break;
            }


            // /[category]/[resource] 
            var path = request.Path.Split('/');
            if (!String.IsNullOrEmpty(request.Path))
            {
                result.Status += "4 missing path,";
            }
            else if (request.Path.Contains(" ") || path.Length < 1 || path.Length > 2)
            {
                result.Status += "4 illegal path,";
            }
            else if (path.Length == 1)
            {
                result.Status += "4 missing resource,";
            }
            else if (path.Length == 2 && String.IsNullOrEmpty(path[1]))
            {
                result.Status = "2 ok,";
            }


            return ToJson(result);

        }

        public static string ToJson(this object data)
        {
            return JsonConvert.SerializeObject(data,
            new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }
    }
}