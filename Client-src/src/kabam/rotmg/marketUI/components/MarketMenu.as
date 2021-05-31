package kabam.rotmg.marketUI.components {
import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.util.FameUtil;
import com.company.ui.BaseSimpleText;

import flash.display.Bitmap;

import flash.display.Sprite;
import flash.events.MouseEvent;

import kabam.rotmg.marketUI.util.MarketUtil;

import org.osflash.signals.Signal;

public class MarketMenu extends Sprite {

    public var CurrentMenu:String;
    public var MenuChanged:Signal;

    private var _buyBG:Sprite;
    private var _sellBG:Sprite;

    private var _buyMenu:MarketBuyMenu;
    private var _sellMenu:MarketSellMenu;

    private var _navigator:MarketPageNavigator;

    private var _nameTitle:BaseSimpleText;
    private var _fameIcon:Bitmap;
    private var _fameTitle:BaseSimpleText;

    private var _gameSprite:AGameSprite;

    public function MarketMenu(gs:AGameSprite) {
        this.MenuChanged = new Signal(String);
        this.CurrentMenu = MarketUtil.BUY_TAB;
        this._gameSprite = gs;

        initialize();

        graphics.lineStyle(3, 0x161616);
        graphics.beginFill(0x565656);
        graphics.drawRect(0, 0, 150, 560);
        graphics.endFill();
        graphics.moveTo(0, 30);
        graphics.lineTo(150, 30);

        graphics.moveTo(0, 400);
        graphics.lineTo(150, 400);
    }

    private function initialize():void {
        this._buyBG = createBG(0, 0, MarketUtil.BUY_TAB);
        this._sellBG = createBG(75, 0, MarketUtil.SELL_TAB);

        this._buyBG.graphics.clear();
        this._buyBG.graphics.lineStyle(1, 0x161616);
        this._buyBG.graphics.beginFill(0x262626);
        this._buyBG.graphics.drawRect(0, 0, 75, 30);
        this._buyBG.graphics.endFill();

        this._buyMenu = new MarketBuyMenu(this._gameSprite);
        this._buyMenu.y = 35;
        this._sellMenu = new MarketSellMenu(this._gameSprite);
        this._sellMenu.y = 35;

        this._nameTitle = new BaseSimpleText(18, 0xFFFFFF);
        this._nameTitle.setText(this._gameSprite.model.getName());
        this._nameTitle.setBold(true);
        this._nameTitle.updateMetrics();

        this._fameIcon = new Bitmap(FameUtil.getFameIcon());
        this._fameTitle = new BaseSimpleText(16, 0xFFFFFF);
        this._fameTitle.setText(this._gameSprite.model.getFame().toString());
        this._fameTitle.updateMetrics();

        this._navigator = new MarketPageNavigator();
        this._navigator.y = 365;
        this._navigator.x = 28;

        this._nameTitle.x = (150 - this._nameTitle.width) / 2;
        this._nameTitle.y = 400;
        this._fameIcon.x = 0;
        this._fameIcon.y = 420;
        this._fameTitle.x = 35;
        this._fameTitle.y = 428;

        addChild(this._buyBG);
        addChild(this._sellBG);
        addChild(this._buyMenu);
        addChild(this._nameTitle);
        addChild(this._fameIcon);
        addChild(this._fameTitle);
        addChild(this._navigator);
    }

    private function createBG(x:int, y:int, nameStr:String):Sprite {
        var bg:Sprite = new Sprite();
        var name:BaseSimpleText = new BaseSimpleText(12, 0xFFFFFF);
        name.setText(nameStr);
        name.setBold(true);
        name.updateMetrics();
        name.x = (75 - name.width) / 2;
        name.y = (30 - name.height) / 2;
        bg.graphics.lineStyle(1, 0x161616);
        bg.graphics.beginFill(0x363636);
        bg.graphics.drawRect(0, 0, 75, 30);
        bg.graphics.endFill();
        bg.x = x;
        bg.y = y;
        bg.addEventListener(MouseEvent.CLICK, onBGClicked);
        bg.addEventListener(MouseEvent.MOUSE_OVER, onBGOver);
        bg.addEventListener(MouseEvent.MOUSE_OUT, onBGOut);
        bg.addChild(name);
        return bg;
    }

    private function onBGClicked(e:MouseEvent):void {
        var bg:Sprite = (e.target as Sprite);
        var result:String = (bg.getChildAt(0) as BaseSimpleText).getRawText();

        if (result == this.CurrentMenu) {
            return;
        }

        bg.graphics.clear();
        bg.graphics.lineStyle(1, 0x161616);
        bg.graphics.beginFill(0x262626);
        bg.graphics.drawRect(0, 0, 75, 30);
        bg.graphics.endFill();

        switch(result)
        {
            case MarketUtil.BUY_TAB:
                    if (contains(this._sellMenu)) {
                        this._sellMenu.reset();
                        removeChild(this._sellMenu);
                    }
                    addChild(this._buyMenu);
                    this._sellBG.graphics.clear();
                    this._sellBG.graphics.lineStyle(1, 0x161616);
                    this._sellBG.graphics.beginFill(0x363636);
                    this._sellBG.graphics.drawRect(0, 0, 75, 30);
                    this._sellBG.graphics.endFill();
                break;
            case MarketUtil.SELL_TAB:
                if (contains(this._buyMenu)) {
                    this._buyMenu.reset();
                    removeChild(this._buyMenu);
                }
                addChild(this._sellMenu);
                this._buyBG.graphics.clear();
                this._buyBG.graphics.lineStyle(1, 0x161616);
                this._buyBG.graphics.beginFill(0x363636);
                this._buyBG.graphics.drawRect(0, 0, 75, 30);
                this._buyBG.graphics.endFill();
                break;
        }

        this.CurrentMenu = result;
        this.MenuChanged.dispatch(this.CurrentMenu);
    }

    private function onBGOver(e:MouseEvent):void {
        var bg:Sprite = (e.target as Sprite);
        var text:String = (bg.getChildAt(0) as BaseSimpleText).getRawText();

        if (text == this.CurrentMenu) {
            return;
        }

        bg.graphics.clear();
        bg.graphics.lineStyle(1, 0x161616);
        bg.graphics.beginFill(0x262626);
        bg.graphics.drawRect(0, 0, 75, 30);
        bg.graphics.endFill();
    }

    private function onBGOut(e:MouseEvent):void {
        var bg:Sprite = (e.target as Sprite);
        var text:String = (bg.getChildAt(0) as BaseSimpleText).getRawText();

        if (text == this.CurrentMenu) {
            return;
        }
        bg.graphics.clear();
        bg.graphics.lineStyle(1, 0x161616);
        bg.graphics.beginFill(0x363636);
        bg.graphics.drawRect(0, 0, 75, 30);
        bg.graphics.endFill();
    }
}
}
