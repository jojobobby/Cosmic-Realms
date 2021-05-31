package kabam.rotmg.ui.view.components {
import com.company.assembleegameclient.ui.SoundIcon;

import flash.display.Sprite;

public class ScreenBase extends Sprite {

    public function ScreenBase() {
        //addChild(new MapBackground()); -- reduce ram usage
        addChild(new DarkLayer());
        addChild(new SoundIcon());
    }

}
}
