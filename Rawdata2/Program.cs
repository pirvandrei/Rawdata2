using DomainModel;
using Assignment3Tests;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Assignment3Tests;
using System.Threading;

namespace EchoServer
{
    public class Server
    {

        public static void Main(string[] args)
        { 
            Console.WriteLine("Getting started!");
            new Server().NewServer(); 
        }
         
        private TcpListener _server;
         
        public Server()
        {
            _server = new TcpListener(IPAddress.Loopback, 5000);
        }
         
        public void NewServer()
        { 
            _server.Start();
            Console.WriteLine("Server started ...");

            while (true)
            {
                var client = _server.AcceptTcpClient();

                Console.WriteLine("client accepted");
                var thread = new Thread(NewClient);

                thread.Start(client);
            }
        }
         
        private void NewClient(object clientObject)
        {
            try
            {
                using (var client = clientObject as TcpClient)
                {
                    var strm = client.GetStream();
                    var buffer = new byte[client.ReceiveBufferSize];
                    var readCnt = strm.Read(buffer, 0, buffer.Length);
                    var payload = Encoding.UTF8.GetString(buffer, 0, readCnt);
                    var request = JsonConvert.DeserializeObject<Request>(payload);
                    Console.WriteLine(request.Body);
                    var res = Encoding.UTF8.GetBytes(GetStatus(request));

                    strm.Write(res, 0, res.Length);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong: " + e.Message + e.StackTrace);
            }
        }

        public string GetStatus(Request request)
        {
            var result = new DomainModel.Response();


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
                result.Status += "4 missing resource,";
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
            return JsonConvert.SerializeObject(result, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
             
        } 

    }

}