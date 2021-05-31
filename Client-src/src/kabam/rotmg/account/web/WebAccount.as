package kabam.rotmg.account.web
{
    import com.company.assembleegameclient.parameters.Parameters;
    import com.company.assembleegameclient.util.GUID;
    import flash.external.ExternalInterface;
    import flash.net.SharedObject;
    import kabam.rotmg.account.core.Account;

    public class WebAccount implements Account
    {
        public static const NETWORK_NAME:String = "rotmg";
        private static const WEB_USER_ID:String = "";
        private static const WEB_PLAY_PLATFORM_NAME:String = "rotmg";

        private var userId:String = "";
        private var password:String;
        private var token:String = "";
        private var entryTag:String = "";
        private var isVerifiedEmail:Boolean;
        private var platformToken:String;
        private var _userDisplayName:String = "";
        private var _rememberMe:Boolean = true;
        private var _paymentProvider:String = "";
        private var _paymentData:String = "";
        public var signedRequest:String;
        public var kabamId:String;

        public function WebAccount()
        {
            try
            {
                this.entryTag = ExternalInterface.call("rotmg.UrlLib.getParam", "entrypt");
            }
            catch (error:Error)
            {
            }
        }

        public function getUserName():String
        {
            return this.userId;
        }

        public function getUserId():String
        {
            return this.userId = this.userId || GUID.create();
        }

        public function getPassword():String
        {
            return this.password || "";
        }

        public function getToken():String
        {
            return "";
        }

        public function getCredentials():Object
        {
            return {
                "guid": this.getUserId(),
                "password": this.getPassword()
            };
        }

        public function isRegistered():Boolean
        {
            return this.getPassword() != "";
        }

        public function updateUser(userId:String, password:String, token:String):void
        {
            var rotmg:SharedObject;
            this.userId = userId;
            this.password = password;
            this.token = token;
            try
            {
                if (this._rememberMe)
                {
                    rotmg = SharedObject.getLocal("GhostMareeRealms", "/");
                    rotmg.data["GUID"] = userId;
                    rotmg.data["Token"] = token;
                    rotmg.data["Password"] = password;
                    rotmg.flush();
                }
            }
            catch (error:Error)
            {
            }
        }

        public function clear():void
        {
            this._rememberMe = true;
            this.updateUser(GUID.create(), null, null);
            Parameters.sendLogin_ = true;
            Parameters.data_.charIdUseMap = {};
            Parameters.save();
        }

        public function reportIntStat(name:String, value:int):void
        {
            trace("Setting int stat \"" + name + "\" to \"" + value + "\"");
        }

        public function getRequestPrefix():String
        {
            return "/credits";
        }

        public function gameNetworkUserId():String
        {
            return WEB_USER_ID;
        }

        public function gameNetwork():String
        {
            return NETWORK_NAME;
        }

        public function playPlatform():String
        {
            return WEB_PLAY_PLATFORM_NAME;
        }

        public function getEntryTag():String
        {
            return this.entryTag || "";
        }

        public function getSecret():String
        {
            return "";
        }

        public function verify(value:Boolean):void
        {
            this.isVerifiedEmail = value;
        }

        public function isVerified():Boolean
        {
            return this.isVerifiedEmail;
        }

        public function getPlatformToken():String
        {
            return this.platformToken || "";
        }

        public function setPlatformToken(token:String):void
        {
            this.platformToken = token;
        }

        public function getMoneyAccessToken():String
        {
            return this.signedRequest;
        }

        public function getMoneyUserId():String
        {
            return this.kabamId;
        }

        public function get userDisplayName():String
        {
            return this._userDisplayName;
        }

        public function set userDisplayName(name:String):void
        {
            this._userDisplayName = name;
        }

        public function set rememberMe(value:Boolean):void
        {
            this._rememberMe = value;
        }

        public function get paymentProvider():String
        {
            return this._paymentProvider;
        }

        public function get paymentData():String
        {
            return this._paymentData;
        }
    }
}