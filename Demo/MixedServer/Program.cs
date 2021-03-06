﻿using MixedServer.Filter;
using MixedServer.Service;
using NetworkSocket.Http;
using NetworkSocket.WebSocket.Fast;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MixedServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // http服务 用于提供静态html页面(也可以用IIS)
            var http = new HttpServer();           
            http.StartListen(82);

            // websocket服务
            var webSocket = new ChatWebSocketServer();
            webSocket.GlobalFilter.Add(new ExceptionFilter()); // 异常处理
            webSocket.BindService<ChatService>(); // 关联服务
            webSocket.StartListen(83);

            if (Directory.Exists("js") == false)
            {
                Directory.SetCurrentDirectory("../../");
            }

            Console.WriteLine("http服务启动成功 ..");
            Process.Start("http://localhost:82/index.html");
            Console.ReadLine();
        }
    }
}
