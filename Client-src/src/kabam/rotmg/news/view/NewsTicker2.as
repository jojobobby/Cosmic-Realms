//by GhostMaree
package kabam.rotmg.news.view
{
    import flash.display.Loader;
    import flash.display.Sprite;
    import flash.media.Video;
    import flash.net.NetConnection;
    import flash.net.NetStream;
    import flash.net.URLRequest;

    public class NewsTicker2 extends Sprite
    {
        private static var urlText:String = "";
        public static var loader:Loader = new Loader();
        public static var nc:NetConnection = new NetConnection();
        public static var video:Video = new Video();
        public static var ns:NetStream;
        public static var url:URLRequest = new URLRequest();

        public function NewsTicker2()
        {
            if (NewsTicker2.urlText != "")
            {
                if (NewsTicker2.urlText.indexOf(".mp4") >= 0)
                {
                    this.loadMovieFromUrl(urlText);
                    //NewsTicker2.urlText = "";
                }
                else
                {
                    this.loadFromUrl(urlText);
                    //NewsTicker2.urlText = "";
                }
            }
        }

        public static function setUrl(_arg1:String):void
        {
            NewsTicker2.urlText = _arg1;
        }

        private function loadMovieFromUrl(text:String):void
        {
            try
            {
                nc.connect(null);
                ns = new NetStream(nc);
                addChild(video);
                video.attachNetStream(ns);
                ns.play(text);
            }
            catch (e)
            {
                NewsTicker2.urlText = "";
            }
        }

        private function loadFromUrl(text:String):void
        {
            try
            {
                url = new URLRequest(text);
                loader.load(url);
                addChild(loader);
            }
            catch (e)
            {
                NewsTicker2.urlText = "";
            }
        }

        public static function dispose():void
        {
            nc.connect(null);
            video.attachNetStream(null);
            ns = null;
            url = null;
            loader.unloadAndStop();
        }
    }
}