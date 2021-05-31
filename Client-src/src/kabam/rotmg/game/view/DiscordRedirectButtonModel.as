package kabam.rotmg.game.view {

import com.company.assembleegameclient.util.TextureRedrawer;
import com.company.util.AssetLibrary;
import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.display.Sprite;
import flash.events.MouseEvent;
import flash.net.URLRequest;
import flash.net.navigateToURL;
import kabam.rotmg.ui.UIUtils;

public class DiscordRedirectButtonModel extends Sprite {

    public static const IMAGE_NAME:String = "emotes";
    public static const IMAGE_ID:int = 880;


    private var bitmap:Bitmap;
    private var background:Sprite;
    private var icon:BitmapData;

    public function DiscordRedirectButtonModel() {
        mouseChildren = false;
        this.icon = TextureRedrawer.redraw(AssetLibrary.getImageFromSet(IMAGE_NAME, IMAGE_ID), 20, true,0);
        this.bitmap = new Bitmap(this.icon);
        this.bitmap.x = -5;
        this.bitmap.y = -8;
        this.background = UIUtils.makeHUDBackground(31, UIUtils.NOTIFICATION_BACKGROUND_HEIGHT);
        addChild(this.background);
        addChild(this.bitmap);
        addEventListener(MouseEvent.CLICK, this.onClick);
    }

    public function onClick(_arg1:MouseEvent):void {
        var url:String = "https://discord.gg/MPCDAppkW4";
        var urlReq:URLRequest = new URLRequest(url);
        navigateToURL(urlReq);
    }


}
}
