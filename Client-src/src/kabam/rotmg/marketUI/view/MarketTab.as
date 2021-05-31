package kabam.rotmg.marketUI.view {

import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.ui.Scrollbar;

import flash.display.Sprite;

import kabam.rotmg.marketUI.components.items.MarketItemBase;

import kabam.rotmg.marketUI.util.MarketUtil;

public class MarketTab extends Sprite {
    protected var _gameSprite:AGameSprite;

    protected var _content:Vector.<MarketItemBase>;

    protected var _contentPanel:Sprite;
    protected var _scrollBar:Scrollbar;

    public function MarketTab(gs:AGameSprite, title:String) {
        this._gameSprite = gs;
        this._content = new Vector.<MarketItemBase>();

        this._contentPanel = new Sprite();
        this._contentPanel.graphics.beginFill(0x161616);
        this._contentPanel.graphics.drawRect(0, 0, MarketUtil.TAB_WIDTH, MarketUtil.TAB_HEIGHT);
        this._contentPanel.graphics.endFill();

        addChild(_contentPanel);
    }

    public function OnTabClosed():void {

    }

    public function OnTabOpen():void {

    }

    public function Dispose():void {
        while(this._contentPanel.numChildren > 0) {
            this._contentPanel.removeChildAt(0);
        }

    }
}
}
