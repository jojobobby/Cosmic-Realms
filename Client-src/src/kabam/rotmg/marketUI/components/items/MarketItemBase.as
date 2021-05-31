package kabam.rotmg.marketUI.components.items {
import com.company.assembleegameclient.constants.InventoryOwnerTypes;
import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.ui.tooltip.EquipmentToolTip;
import com.company.assembleegameclient.ui.tooltip.ToolTip;
import com.company.assembleegameclient.util.FameUtil;
import com.company.ui.BaseSimpleText;
import com.company.util.BitmapUtil;

import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.display.PixelSnapping;
import flash.display.Sprite;
import flash.events.MouseEvent;
import flash.filters.GlowFilter;

import org.osflash.signals.Signal;

public class MarketItemBase extends Sprite {

    public const WIDTH:int = 625;
    public const HEIGHT:int = 55;

    private var _itemBitmap:Sprite;
    private var _itemTitle:BaseSimpleText;
    private var _curLogo:Bitmap;
    private var _price:BaseSimpleText;

    protected var _itemType:int;
    protected var _gs:AGameSprite;
    protected var _id:int;

    public var ShowTooltip:Signal;
    private var _toolTip:ToolTip;

    public function MarketItemBase(gs:AGameSprite, type:int, price:int, id:int) {
        this._itemType = type;
        this._gs = gs;
        this._id = id;
        this.ShowTooltip = new Signal(ToolTip);

        var bitmapData:BitmapData = BitmapUtil.resize(ObjectLibrary.getBitmapData(this._itemType), 50, 50);
        var itemID:String = ObjectLibrary.getIdFromType(this._itemType);
        this._itemBitmap = new Sprite();
        this._itemBitmap.graphics.beginBitmapFill(bitmapData);
        this._itemBitmap.graphics.drawRect(0, 0, 50, 50);
        this._itemBitmap.graphics.endFill();
        this._itemBitmap.filters = [new GlowFilter(0, 1, 2, 2, 10)];
        this._itemBitmap.x = 3;
        this._itemBitmap.y = 3;
        this._itemBitmap.addEventListener(MouseEvent.MOUSE_OVER, onToolTip);
        this._itemBitmap.addEventListener(MouseEvent.MOUSE_OUT, removeToolTip);

        this._itemTitle = new BaseSimpleText(14, 0xFFFFFF);
        this._itemTitle.setText(itemID);
        this._itemTitle.setBold(true);
        this._itemTitle.updateMetrics();
        this._itemTitle.filters = [new GlowFilter(0, 1, 2, 2, 10)];
        this._itemTitle.x = 55;
        this._itemTitle.y = 1;

        this._curLogo = new Bitmap(BitmapUtil.resize(FameUtil.getFameIcon(), 40, 40));
        this._curLogo.x = 58;
        this._curLogo.y = 17;

        this._price = new BaseSimpleText(13, 0xFFFFFF);
        this._price.setText(price.toString());
        this._price.updateMetrics();
        this._price.x = 90;
        this._price.y = 27;

        addChild(this._itemBitmap);
        addChild(this._itemTitle);
        addChild(this._curLogo);
        addChild(this._price);

        graphics.beginFill(0x565656);
        graphics.drawRect(0, 0, WIDTH, HEIGHT);
        graphics.endFill();

        addEventListener(MouseEvent.MOUSE_OVER, mouseOver);
        addEventListener(MouseEvent.MOUSE_OUT, mouseOut);
    }

    private function onToolTip(e:MouseEvent):void {
        //add toolTip
        if (this._toolTip == null) {
            this._toolTip = new EquipmentToolTip(this._itemType, this._gs.map.player_,
                    this._gs.map.player_.objectType_, InventoryOwnerTypes.NPC);
        }

        this._toolTip.attachToTarget(this);
        this.ShowTooltip.dispatch(this._toolTip);
    }

    private function removeToolTip(e:MouseEvent):void {
        if (this._toolTip) {
            this._toolTip.detachFromTarget();
        }
    }

    private function mouseOver(e:MouseEvent):void {
        graphics.clear();
        graphics.beginFill(0x666666);
        graphics.drawRect(0, 0, WIDTH, HEIGHT);
        graphics.endFill();
    }

    private function mouseOut(e:MouseEvent):void {
        graphics.clear();
        graphics.beginFill(0x565656);
        graphics.drawRect(0, 0, WIDTH, HEIGHT);
        graphics.endFill();
    }
}
}
