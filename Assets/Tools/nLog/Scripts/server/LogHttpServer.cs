using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using System.IO;
namespace NLog.Server
{
    

    public class BaseServerModule
    {
        public short Cmd;

        public virtual void Process(Socket clientSocket, byte[] clientBS)
        {

        }

    }

    /// <summary>
    /// NINFO 使用方法，直接挂一个空场景节点上就可以接收消息
    /// 注意要设置下ip，局域网设置ipv4的就可以
    /// 
    /// </summary>
    public class LogHttpServer : MonoBehaviour
    {
        public static string LogFileSavePath;

        private static bool BeWriteLog2File = true;

        public static LogHttpServer Ins;

        Socket serverSocket;

        bool isRunning = false;

        public string ServerIP = "127.0.0.1";

        public int ServerPort = 5432;

        public Dictionary<short, BaseServerModule> RecLogicDic = new Dictionary<short, BaseServerModule>();

        void Awake()
        {
            Ins = this;
            LogFileSavePath = Application.persistentDataPath + "/nServerLog.txt";
        }

        void Start()
        {
            //ServerIP = GetLocalIPv4();

            if (LogConfig.BeWrite2File && File.Exists(LogFileSavePath))
            {
                File.Delete(LogFileSavePath);
            }
            File.Create(LogFileSavePath).Dispose();


            Debug.Log("启动log server 监听地址ip:" + ServerIP + " port:" + ServerPort);

            Run();
        }

        void OnDestroy()
        {
            Ins = null;
            serverSocket.Close();
            serverSocket.Dispose();
        }

        public void Run()
        {
            if (isRunning)
                return;

            //创建服务端Socket
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse(ServerIP), ServerPort));
            serverSocket.Listen(10);
            isRunning = true;

            //输出服务器状态
            Debug.Log(string.Format("Sever is running at http://{0}:{1}/.", ServerIP, ServerPort));

            serverSocket.BeginAccept(AsyncAcceptClient, serverSocket);

        }

        private void AsyncAcceptClient(IAsyncResult asyncResult)
        {
            Socket clientSocket = serverSocket.EndAccept(asyncResult);
            clientSocket.NoDelay = true;
            Thread requestThread = new Thread(
                () =>
                {
                    //NINFO 这句能解决，服务器接收客户端数据只接受到httpHead，接受不到内容数据的问题
                    //说明数据有时会被拆包发送，根本解决方法还是处理拆包分包的方法
                    Thread.Sleep(100);
                    ProcessRequest(clientSocket);
                }
                );
            requestThread.Start();
            serverSocket.BeginAccept(AsyncAcceptClient, serverSocket);

        }
        private void ProcessRequest(Socket clientSocket)
        {
            //构造请求报文
            HttpRequest request = new HttpRequest(clientSocket);

            //根据请求类型进行处理
            switch (request.Method)
            {
                case "GET":
                    OnGet(request);
                    break;
                case "POST":
                    OnPost(request);
                    break;
            }

        }

        private void OnGet(HttpRequest request)
        {
            //NTODO 这里需要解析client传入的地址参数，然后根据参数返回，可以暂时不处理get
            HttpResponse response = new HttpResponse("<html><body><h1>Hello World</h1></body></html>", Encoding.UTF8);
            //response.StatusCode = "200";
            //response.Server = "A Simple HTTP Server";
            //response.Content_Type = "text/html";
            ProcessResponse(request.ClientSocket, response);
        }

        private void OnPost(HttpRequest request)
        {
            byte[] cacheBs = new byte[102400];
            int len = request.ClientSocket.Receive(cacheBs);
            byte[] bs = new byte[len];
            Array.Copy(cacheBs, 0, bs, 0, len);

            //Debug.Log("服务端接受len :" + bs.Length);
            //for (int i = 0; i < bs.Length; i++)
            //{
            //    Debug.LogError("server rec: i:"+i+"--->"+bs[i]);
            //}

            //用于存储消息长度int的bytes
            byte[] msgContentLenBytes = new byte[4];
            Array.Copy(bs, bs.Length - 4, msgContentLenBytes, 0, 4);

            IoBuffer ibb = new IoBuffer(1024);
            ibb.PutBytes(msgContentLenBytes);
            //真实消息长度
            int msgContentLen = ibb.GetInt();
            //用于存储去除httpRequest头后的真实消息内容
            byte[] msgContentBytes = new byte[msgContentLen];
            //Debug.LogError("server rec msgContentLen:" + msgContentLen);
            Array.Copy(bs, bs.Length - msgContentLen - 4, msgContentBytes, 0, msgContentLen);

            IoBuffer buffer = new IoBuffer(10240);
            buffer.PutBytes(msgContentBytes);

            string s = buffer.GetString();

            if (BeWriteLog2File)
            {
                WriteString2File_Append(LogFileSavePath, s);
            }

            Debug.Log(s);

            request.ClientSocket.Send(Encoding.UTF8.GetBytes(HttpResponse.ResponseHead));
            request.ClientSocket.Close();
            request.ClientSocket = null;
        }

        public void ProcessResponse(Socket clientSocket, HttpResponse response)
        {
            Debug.LogError("服务端连续发送响应头和数据信息");
            clientSocket.Send(Encoding.UTF8.GetBytes(HttpResponse.ResponseHead));
            clientSocket.Send(response.Datas);
            //server向client回消息后，要关闭clinet，否则clinet收不到返回
            clientSocket.Close();
            clientSocket = null;
        }


        public static string GetLocalIPv4()
        {
            try
            {
                IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress item in IpEntry.AddressList)
                {
                    if (item.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return item.ToString();
                    }
                }
                return "";
            }
            catch { return ""; }
        }

        public static void WriteString2File_Append(string url, string content)
        {
            if (!File.Exists(url))
                File.Create(url).Dispose();

            FileStream fs = new FileStream(url, FileMode.Append);

            StreamWriter sw = new StreamWriter(fs);

            sw.WriteLine(content);

            sw.Close();

            fs.Close();

            sw.Dispose();

            fs.Dispose();

        }


    }

    public class HttpRequest
    {
        public string Method = "POST";

        public Socket ClientSocket;

        public HttpRequest(Socket clientSocket)
        {
            ClientSocket = clientSocket;
        }
    }

    public class HttpResponse
    {
        public const string ResponseHead = "HTTP/1.1 200 OK\nContent-Type:text/html\n\n";

        //public string StatusCode = "200";

        //public string Server = "dServer";

        //public string Content_Type = "text/html";

        public byte[] Datas;

        public HttpResponse(string s, Encoding encod1)
        {
            Datas = encod1.GetBytes(s);
        }

        public HttpResponse(byte[] bs)
        {
            Datas = bs;
        }

    }
}
