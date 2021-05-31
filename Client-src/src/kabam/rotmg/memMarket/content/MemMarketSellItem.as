package kabam.rotmg.memMarket.content {
import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.ui.DeprecatedTextButton;
import com.company.assembleegameclient.ui.DeprecatedTextButton;
import com.company.assembleegameclient.util.Currency;
import com.company.ui.BaseSimpleText;

import flash.display.Bitmap;
import flash.events.Event;
import flash.events.MouseEvent;
import flash.text.TextFieldAutoSize;

import kabam.rotmg.memMarket.utils.DialogUtils;

import kabam.rotmg.memMarket.utils.IconUtils;

import kabam.rotmg.messaging.impl.data.MarketData;

public class MemMarketSellItem extends MemMarketItem
{
    private var removeButton_:DeprecatedTextButton;
    private var priceText_:BaseSimpleText;
    private var timeText_:BaseSimpleText;
    private var currency_:Bitmap;

    public function MemMarketSellItem(gameSprite:GameSprite, data:MarketData)
    {
        super(gameSprite, OFFER_WIDTH, OFFER_HEIGHT, 80, data.itemType_, data);
        this.icon_.x = 25;
        this.icon_.y = -3;

        this.removeButton_ = new DeprecatedTextButton(10, "Remove", 96);
        this.removeButton_.x = 2;
        this.removeButton_.y = 62;
        this.removeButton_.addEventListener(MouseEvent.CLICK, this.onRemoveClick);
        addChild(this.removeButton_);

        this.priceText_ = new BaseSimpleText(10, 0xFFFFFF, false, width, 0);
        this.priceText_.setBold(true);
        this.priceText_.htmlText = "<p align=\"center\">" + this.data_.price_ + "</p>";
        this.priceText_.wordWrap = true;
        this.priceText_.multiline = true;
        this.priceText_.autoSize = TextFieldAutoSize.CENTER;
        this.priceText_.y = 45;
        addChild(this.priceText_);

        this.currency_ = new Bitmap(IconUtils.getFameIcon(32));
        this.currency_.x = 70;
        this.currency_.y = 0;
        addChild(this.currency_);
    }

    private function onRemoveClick(event:MouseEvent) : void
    {
        DialogUtils.makeCallbackDialog(this.gameSprite_, "Verification", "Remove this item?", "Yes", "No", this.onVerified);
    }

    private function onVerified(event:Event) : void
    {
        this.gameSprite_.gsc_.marketRemove(this.id_);
    }

    /* Clear */
    public override function dispose() : void
    {
        this.removeButton_.removeEventListener(MouseEvent.CLICK, this.onRemoveClick);
        this.removeButton_ = null;
        this.priceText_ = null;
        this.timeText_ = null;
        this.currency_ = null;

        super.dispose();
    }
}
}
