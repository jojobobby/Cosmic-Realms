package kabam.rotmg.memMarket.tabs {
import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.game.GameSprite;

import flash.display.Sprite;

public class MemMarketTab extends Sprite
{
    public var gameSprite_:GameSprite;

    /* Not an actual tab, but provides the base variables and functions for each tab */
    public function MemMarketTab(gameSprite:GameSprite, xOffset:int = 265)
    {
        this.gameSprite_ = gameSprite;

        /* Draw vertical line */
        graphics.clear();
        graphics.lineStyle(1,6184542);

        graphics.moveTo(xOffset,100);
        graphics.lineTo(xOffset,525);
        graphics.lineStyle();
    }

    /* Clear */
    public function dispose() : void
    {
        this.gameSprite_ = null;

        /* Remove all children */
        for (var i:int = numChildren - 1; i >= 0; i--)
        {
            removeChildAt(i);
        }
    }
}
}
