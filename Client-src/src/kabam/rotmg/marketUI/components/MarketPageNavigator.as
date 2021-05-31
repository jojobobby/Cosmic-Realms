package kabam.rotmg.marketUI.components {
import com.company.ui.BaseSimpleText;
import com.company.util.AssetLibrary;
import com.company.util.MoreColorUtil;

import flash.display.Bitmap;

import flash.display.BitmapData;
import flash.display.Sprite;
import flash.events.MouseEvent;
import flash.geom.ColorTransform;

import kabam.rotmg.marketUI.signals.NewPageSignal;
import kabam.rotmg.marketUI.signals.UpdatePageNagivator;

public class MarketPageNavigator extends Sprite {

    public const updateSignal:UpdatePageNagivator = new UpdatePageNagivator();

    private var _pageText:BaseSimpleText;
    private var _left:Sprite;
    private var _right:Sprite;

    private var _curPage:int;
    private var _maxPage:int;

    public function MarketPageNavigator() {
        this._left = makeLeftNav();
        this._right = makeRightNav();
        this._curPage = 0;
        this._pageText = new BaseSimpleText(16, 0xFFFFFF);
        this._pageText.setText(this._curPage.toString() + "/" + this._maxPage.toString());
        this._pageText.updateMetrics();

        this._left.x = 0;
        this._pageText.x = this._left.width + 5;
        this._right..x = this._pageText.width + this._pageText.x + 5;
        this._left.y = 23;
        this._right.y = 23;

        addChild(this._left);
        addChild(this._right);
        addChild(this._pageText);

        this.updateSignal.add(updateNavigator);
    }

    private function updateNavigator(cur:int, max:int):void {
        this._curPage = cur;
        this._maxPage = max;

        this._pageText.setText(this._curPage.toString() + "/" + this._maxPage.toString());
        this._pageText.updateMetrics();
    }


    private function makeLeftNav():Sprite {
        var _local1:BitmapData = AssetLibrary.getImageFromSet("lofiInterface", 54);
        var _local2:Bitmap = new Bitmap(_local1);
        _local2.scaleX = 4;
        _local2.scaleY = 4;
        _local2.rotation = -90;
        var _local3:Sprite = new Sprite();
        _local3.addChild(_local2);
        _local3.addEventListener(MouseEvent.MOUSE_OVER, this.onArrowHover);
        _local3.addEventListener(MouseEvent.MOUSE_OUT, this.onArrowHoverOut);
        _local3.addEventListener(MouseEvent.CLICK, this.onClick);
        return (_local3);
    }

    private function onClick(e:MouseEvent):void {
        switch (e.currentTarget) {
            case this._right:
                if ((this._curPage + 1) <= this._maxPage) {
                    NewPageSignal._staticInstance.dispatch(NewPageSignal.forward);
                }
                return;
            case this._left:
                if ((this._curPage - 1) >= 1) {
                    NewPageSignal._staticInstance.dispatch(NewPageSignal.backward);
                }
                return;
        }
    }

    private function makeRightNav():Sprite {
        var _local1:BitmapData = AssetLibrary.getImageFromSet("lofiInterface", 55);
        var _local2:Bitmap = new Bitmap(_local1);
        _local2.scaleX = 4;
        _local2.scaleY = 4;
        _local2.rotation = -90;
        var _local3:Sprite = new Sprite();
        _local3.addChild(_local2);
        _local3.addEventListener(MouseEvent.MOUSE_OVER, this.onArrowHover);
        _local3.addEventListener(MouseEvent.MOUSE_OUT, this.onArrowHoverOut);
        _local3.addEventListener(MouseEvent.CLICK, this.onClick);
        return (_local3);
    }


    private function onArrowHover(_arg1:MouseEvent):void {
        _arg1.currentTarget.transform.colorTransform = new ColorTransform(1, (220 / 0xFF), (133 / 0xFF));
    }

    private function onArrowHoverOut(_arg1:MouseEvent):void {
        _arg1.currentTarget.transform.colorTransform = MoreColorUtil.identity;
    }
}
}
