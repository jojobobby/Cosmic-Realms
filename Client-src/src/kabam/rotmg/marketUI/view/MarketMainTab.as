package kabam.rotmg.marketUI.view {

import com.company.assembleegameclient.game.AGameSprite;
import com.company.ui.BaseSimpleText;

import flash.display.Sprite;
import flash.filters.GlowFilter;
import flash.utils.Dictionary;


import kabam.rotmg.marketUI.components.MarketMenu;

import kabam.rotmg.marketUI.util.MarketUtil;
import kabam.rotmg.pets.view.components.DialogCloseButton;

public class MarketMainTab extends Sprite {

    private var _gameSprite:AGameSprite;
    private var _currentTab:String;

    private var titleText:BaseSimpleText;
    private var marketMenu:MarketMenu;
    private var closeBtn:DialogCloseButton;

    private var views:Dictionary;

    public function MarketMainTab(gs:AGameSprite) {
        this._gameSprite = gs;
        this._currentTab = MarketUtil.BUY_TAB;

        initialize();

        graphics.lineStyle(3, 0x161616);
        graphics.beginFill(0x363636);
        graphics.drawRect(1, 1, 798, 598);
        graphics.endFill();
        graphics.moveTo(0, 35);
        graphics.lineTo(800, 35);
    }

    private function initialize():void {
        this.titleText = new BaseSimpleText(27, 0xFFFFFF);
        this.titleText.setText("CR MARKET PLACE");
        this.titleText.filters = [new GlowFilter(0, 1, 3, 3, 30)];
        this.titleText.updateMetrics();
        this.titleText.x = (800 - this.titleText.width) / 2;
        this.titleText.y = 2;

        addChild(this.titleText);

        this.closeBtn = new DialogCloseButton();
        this.closeBtn.x = 765;
        this.closeBtn.y = 8;
        this.closeBtn.clicked.add(closeView);

        addChild(this.closeBtn);

        this.marketMenu = new MarketMenu(this._gameSprite);
        this.marketMenu.x = 4;
        this.marketMenu.y = 37;
        this.marketMenu.MenuChanged.add(onMenuChanged);
        addChild(this.marketMenu);

        this.views = new Dictionary();

        this.views[MarketUtil.BUY_TAB] = new MarketBuyTab(this._gameSprite, MarketUtil.BUY_TAB);
        this.views[MarketUtil.BUY_TAB].x = 160;
        this.views[MarketUtil.BUY_TAB].y = 40;
        this.views[MarketUtil.SELL_TAB] = new MarketSellTab(this._gameSprite, MarketUtil.SELL_TAB);
        this.views[MarketUtil.SELL_TAB].x = 160;
        this.views[MarketUtil.SELL_TAB].y = 40;

        this.views[MarketUtil.BUY_TAB].OnTabOpen();
        addChild(this.views[MarketUtil.BUY_TAB]);
    }

    private function closeView():void {
        //for now
        this.views[MarketUtil.SELL_TAB].Dispose();
        this.views[MarketUtil.SELL_TAB].Dispose();
        this.views.length = 0;
        this._gameSprite.removeChild(this);
        this._gameSprite.mui_.setEnablePlayerInput(true);
    }

    private function onMenuChanged(menu:String):void {
        if (this._currentTab == name) {
            return;
        }

        //handle menu change
        removeChild(this.views[this._currentTab]);
        this.views[this._currentTab].OnTabClosed();
        this.views[menu].OnTabOpen();
        addChild(this.views[menu]);

        this._currentTab = menu;
    }
}
}
