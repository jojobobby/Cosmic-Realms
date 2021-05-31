using System;
using System.Threading;
using System.IO;
using Anna;
using log4net;
using log4net.Config;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using System.Web;
using Anna.Request;
using common;
using common.resources;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using System.Diagnostics;
using DiscordWebhook;
namespace server
{
    class Program
    {
        internal static readonly ILog Log = LogManager.GetLogger("Server");
        internal static readonly ManualResetEvent Shutdown = new ManualResetEvent(false);
        internal static ServerConfig Config;
        internal static Resources Resources;
        internal static Database Database;
        internal static ISManager ISManager;
        internal static ChatManager ChatManager;
        internal static ISControl ISControl;
        internal static LegendSweeper LegendSweeper;
        internal static Stopwatch packetsStopWatch;
        internal static Stopwatch banStopWatch;
        static Dictionary<string, int> playerPackets = new Dictionary<string, int>();
        static Dictionary<string, bool> packetBan  = new Dictionary<string, bool>();
        [Obsolete]
        static void Main(string[] args)
        {
            packetsStopWatch = Stopwatch.StartNew();
            banStopWatch = Stopwatch.StartNew();
            AppDomain.CurrentDomain.UnhandledException += LogUnhandledException;

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.Name = "Entry";

            Config = args.Length > 0 ?
                ServerConfig.ReadFile(args[0]) :
                ServerConfig.ReadFile("server.json");

            Environment.SetEnvironmentVariable("ServerLogFolder", Config.serverSettings.logFolder);
            GlobalContext.Properties["ServerName"] = Config.serverInfo.name;
            GlobalContext.Properties["ServerType"] = Config.serverInfo.type.ToString();

            XmlConfigurator.ConfigureAndWatch(new FileInfo(Config.serverSettings.log4netConfig));

            using (Resources = new Resources(Config.serverSettings.resourceFolder))
            using (Database = new Database(
                    Config.dbInfo.host,
                    Config.dbInfo.port,
                    Config.dbInfo.auth,
                    Config.dbInfo.index,
                    Resources))
            {
                RequestHandlers.Initialize(Resources);

                Config.serverInfo.instanceId = Guid.NewGuid().ToString();
                ISManager = new ISManager(Database, Config);
                ISManager.Run();
                ChatManager = new ChatManager(ISManager);
                ISControl = new ISControl(ISManager);
                LegendSweeper = new LegendSweeper(Database);
                LegendSweeper.Run();
                
                Console.CancelKeyPress += delegate
                {
                    Shutdown.Set();
                };

                var port = Config.serverInfo.port;
                using (var server = new HttpServer($"http://{Config.serverInfo.bindAddress}:{port}/"))
                {
                    //try
                    //{
                        foreach (var uri in RequestHandlers.Get.Keys)
                        {
                            try
                            {
                                server.GET(uri).Subscribe(Response);
                            }
                            catch
                            {
                                //Log.Error(e);
                                continue;
                            }
                        }

                        foreach (var uri in RequestHandlers.Post.Keys)
                        {
                            try
                            {
                                server.POST(uri).Subscribe(Response);
                            }
                            catch
                            {
                                //Log.Error(e);
                                continue;
                            }
                        }

                        Log.Info("Listening at port " + port + "...");
                        Shutdown.WaitOne();
                    //}
                    //catch (Exception e)
                    //{
                    //    RequestHandlers.Get.Clear();
                    //    RequestHandlers.Post.Clear();
                    //   Log.Error(e);
                        
                    //}
                }
                
                Log.Info("Terminating...");
                ISManager.Dispose();
            }
        }

        public static void Stop()
        {
            Shutdown.Set();
        }
        private static int PacketSendPerMinute(RequestContext rContext)
        {
            if (packetsStopWatch.ElapsedMilliseconds >= 60 * 1000) {
                playerPackets.Clear();
                packetsStopWatch.Stop();
                packetsStopWatch = Stopwatch.StartNew();
            }

            if (playerPackets.ContainsKey(rContext.Request.ClientIP()))
                playerPackets[rContext.Request.ClientIP()]++;  
            else
                playerPackets.Add(rContext.Request.ClientIP(), 1); 
            return playerPackets[rContext.Request.ClientIP()]; 
        }
        private static bool isBanned(RequestContext rContext)
        {
            if (!packetBan.ContainsKey(rContext.Request.ClientIP()))
                packetBan.Add(rContext.Request.ClientIP(), false);
            if (PacketSendPerMinute(rContext) >= 350)
            {

                if (!packetBan.ContainsKey(rContext.Request.ClientIP()))
                {
                    packetBan.Add(rContext.Request.ClientIP(), true);
                    playerPackets[rContext.Request.ClientIP()] = 1;
                    return true;
                }
                else
                {
                    packetBan[rContext.Request.ClientIP()] = true;
                    playerPackets[rContext.Request.ClientIP()] = 1;
                    return (packetBan[rContext.Request.ClientIP()]);
                }
            }
            return false;
        }


        [Obsolete]
        private static void Response(RequestContext rContext)
        {
            if (banStopWatch.ElapsedMilliseconds >= 60 * 60 * 1000)
            {
                packetBan.Clear();
                banStopWatch.Stop();
                banStopWatch = Stopwatch.StartNew();
            }
            isBanned(rContext);
            if (packetBan[rContext.Request.ClientIP()])
            {
                if (!Database.IsIpBanned(rContext.Request.ClientIP()))
                {
                    Embed[] embeds = {
                    new Embed
                    {
                        title = "",
                        description = rContext.Request.ClientIP()+" has exceeded the number of queries, got banned from sending any packets for an hour + got an ip ban"
                    }
                };
                    Database.BanIp(rContext.Request.ClientIP(), "Packet Spamming");
                }
                return;
            } 
            try
            {
                Log.InfoFormat("Dispatching '{0}'@{1}",
                    rContext.Request.Url.LocalPath, 
                    rContext.Request.ClientIP());

                if (rContext.Request.HttpMethod.Equals("GET"))
                {
                    var query = HttpUtility.ParseQueryString(rContext.Request.Url.Query);
                    RequestHandlers.Get[rContext.Request.Url.LocalPath].HandleRequest(rContext, query);
                    return;
                }

                try
                {
                    GetBody(rContext.Request, 4096).Subscribe(body =>
                    {
                        try
                        {
                            var query = HttpUtility.ParseQueryString(body);
                            RequestHandlers.Post[rContext.Request.Url.LocalPath]
                                .HandleRequest(rContext, query);
                        }
                        catch
                        {
                            //OnError(e, rContext);
                        }
                    });
                }
                catch
                {
                    //OnError(e, rContext);
                }
            }
            catch
            {
                //OnError(e, rContext);
            }
        }

        private static void OnError(Exception e, RequestContext rContext)
        {
            Log.Error($"{e.Message}\r\n{e.StackTrace}");

            try
            {
                rContext.Respond("<Error>Internal server error</Error>", 500);
            }
            catch
            {
            }
        }

        [Obsolete]
        private static IObservable<string> GetBody(Request r, int maxContentLength = 50000)
        {
            try
            {
                int bufferSize = maxContentLength;
                if (r.Headers.ContainsKey("Content-Length"))
                    bufferSize = Math.Min(maxContentLength, int.Parse(r.Headers["Content-Length"].First()));

                var buffer = new byte[bufferSize];
                return Observable.FromAsyncPattern<byte[], int, int, int>(r.InputStream.BeginRead, r.InputStream.EndRead)(buffer, 0, bufferSize)
                    .Select(bytesRead => r.ContentEncoding.GetString(buffer, 0, bytesRead));
            }
            catch
            {
                //Log.Error(e);
                return null;
            }
        }

        private static void LogUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Log.Fatal((Exception)args.ExceptionObject);
        }
    }
}
