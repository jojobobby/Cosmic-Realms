package kabam.rotmg.memMarket {
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.appengine.SavedCharactersList;
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.ui.panels.ButtonPanel;
import com.company.assembleegameclient.ui.panels.Panel;

import flash.events.Event;

import flash.events.MouseEvent;

import flash.filters.DropShadowFilter;

import flash.text.TextFieldAutoSize;

public class MemMarketPanel extends ButtonPanel
{
    public function MemMarketPanel(gameSprite:GameSprite)
    {
        super(gameSprite, "Market", "Open");
    }

    override protected function onButtonClick(event:MouseEvent) : void
    {

        this.gs_.mui_.setEnablePlayerInput(false); /* Disable player movement */
        this.gs_.addChild(new MemMarket(this.gs_.mui_.gs_));

    }
}
}
