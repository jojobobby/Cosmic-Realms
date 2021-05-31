package kabam.rotmg.memMarket.content {
import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.ui.DeprecatedTextButton;
import com.company.assembleegameclient.util.Currency;
import com.company.assembleegameclient.util.FameUtil;
import com.company.ui.BaseSimpleText;

import flash.display.Bitmap;

import flash.events.Event;

import flash.events.MouseEvent;
import flash.system.System;
import flash.text.TextFieldAutoSize;

import kabam.rotmg.assets.services.IconFactory;

import kabam.rotmg.memMarket.tabs.MemMarketBuyTab;
import kabam.rotmg.memMarket.utils.DialogUtils;
import kabam.rotmg.memMarket.utils.IconUtils;

import kabam.rotmg.messaging.impl.data.MarketData;

public class MemMarketBuyItem extends MemMarketItem
{
    private var buyButton_:DeprecatedTextButton;
    //private var priceText_:BaseSimpleText;
    private var currency_:Bitmap;
    public var price_:int;

    public function MemMarketBuyItem(gameSprite:GameSprite, data:MarketData)
    {
        super(gameSprite, OFFER_WIDTH, OFFER_HEIGHT, 80, data.itemType_, data, true);
        this.icon_.x = 22;
        this.icon_.y = -6;
        this.price_ = data.price_;
        this.buyButton_ = new DeprecatedTextButton(10, "", 90);
        this.buyButton_.x = 5;
        this.buyButton_.y = 58;

        this.buyButton_.setText(this.data_.price_.toString());
        this.buyButton_.setColor(0);

        this.updateButton();
        addChild(this.buyButton_);

        /*this.priceText_ = new BaseSimpleText(10, 0xFFFFFF, false, width, 0);
        this.priceText_.setBold(true);
        this.priceText_.htmlText = "<p align=\"center\">" + this.data_.price_ + "</p>";
        this.priceText_.wordWrap = true;
        this.priceText_.multiline = true;
        this.priceText_.autoSize = TextFieldAutoSize.CENTER;
        this.priceText_.y = 47;
        addChild(this.priceText_);*/

        this.currency_ = new Bitmap(IconUtils.getFameIcon(32));
        this.currency_.x = 70;
        this.currency_.y = -5;
        addChild(this.currency_);
    }

    private function onBuyClick(event:MouseEvent) : void
    {
        DialogUtils.makeCallbackDialog(this.gameSprite_, "Verification", "Buy this item?", "Yes", "No", this.onVerified);
    }

    private function onVerified(event:Event) : void
    {
        this.gameSprite_.gsc_.marketBuy(this.id_);
    }

    public function updateButton() : void
    {
        var currencyAmount:int = this.data_.currency_ == Currency.FAME ? this.gameSprite_.map.player_.fame_ : this.gameSprite_.map.player_.credits_;
        if (currencyAmount >= this.data_.price_) /* Only add this event listener if we can afford the item */
        {
            this.buyButton_.addEventListener(MouseEvent.CLICK, this.onBuyClick);
            this.buyButton_.setEnabled(true);
        }
        else
        {
            this.buyButton_.removeEventListener(MouseEvent.CLICK, this.onBuyClick);
            this.buyButton_.setEnabled(false);
        }
    }
    /* Clear */
    public override function dispose() : void
    {
        this.buyButton_.removeEventListener(MouseEvent.CLICK, this.onBuyClick);
        this.buyButton_ = null;
        //this.priceText_ = null;
        this.currency_ = null;

        super.dispose();
    }
}
}
