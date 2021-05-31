package kabam.rotmg.marketUI {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.ui.panels.ButtonPanel;

import flash.events.MouseEvent;

import kabam.rotmg.marketUI.view.MarketMainTab;

public class MarketPanel extends ButtonPanel {
    public function MarketPanel(gs:GameSprite) {
        super(gs, "MarketPlace", "Open");
    }

    override protected function onButtonClick(event:MouseEvent) : void
    {
        this.gs_.mui_.setEnablePlayerInput(false);
        this.gs_.addChild(new MarketMainTab(this.gs_));
    }
}
}
