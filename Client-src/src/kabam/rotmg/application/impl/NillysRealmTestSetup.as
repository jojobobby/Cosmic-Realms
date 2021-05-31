package kabam.rotmg.application.impl {
import com.company.assembleegameclient.parameters.Parameters;

import kabam.rotmg.application.api.ApplicationSetup;

public class NillysRealmTestSetup implements ApplicationSetup {
    
    private const SERVER:String = "http://185.109.163.163:8080";
    private const UNENCRYPTED:String = "http://" + SERVER;
    private const ENCRYPTED:String = "http://" + SERVER;
    private const BUILD_LABEL:String = "Tidan Realms <font color='#FFEE00'>TESTING</font> #{VERSION}.{MINOR}";
    
    
    public function getAppEngineUrl(_arg1:Boolean = false):String {
        return this.SERVER;
    }
    
    public function getBuildLabel():String {
        return this.BUILD_LABEL.replace("{VERSION}", Parameters.BUILD_VERSION).replace("{MINOR}", Parameters.MINOR_VERSION);
    }
    
    public function useLocalTextures():Boolean {
        return true;
    }
    
    public function isToolingEnabled():Boolean {
        return true;
    }

    public function isServerLocal():Boolean {
        return true;
    }
    
    public function isGameLoopMonitored():Boolean {
        return true;
    }
    
    public function useProductionDialogs():Boolean {
        return false;
    }
    
    public function areErrorsReported():Boolean {
        return false;
    }
    
    public function areDeveloperHotkeysEnabled():Boolean {
        return true;
    }
    
    public function isDebug():Boolean {
        return true;
    }
    
    
}
}
