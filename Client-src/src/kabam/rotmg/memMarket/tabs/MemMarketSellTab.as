package kabam.rotmg.memMarket.tabs {
import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.ui.DeprecatedTextButton;
import com.company.assembleegameclient.ui.MarketInput;
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.ui.Scrollbar;
import com.company.assembleegameclient.ui.DeprecatedTextButton;
import com.company.assembleegameclient.ui.dropdown.DropDown;
import com.company.assembleegameclient.util.Currency;

import flash.display.Shape;
import flash.display.Sprite;
import flash.events.Event;
import flash.events.MouseEvent;

import kabam.rotmg.core.StaticInjectorContext;


import kabam.rotmg.memMarket.content.MemMarketInventoryItem;
import kabam.rotmg.memMarket.content.MemMarketItem;
import kabam.rotmg.memMarket.content.MemMarketSellItem;
import kabam.rotmg.memMarket.signals.MemMarketAddSignal;
import kabam.rotmg.memMarket.signals.MemMarketMyOffersSignal;
import kabam.rotmg.memMarket.signals.MemMarketRemoveSignal;
import kabam.rotmg.memMarket.utils.DialogUtils;
import kabam.rotmg.memMarket.utils.SortUtils;
import kabam.rotmg.messaging.impl.data.MarketData;
import kabam.rotmg.messaging.impl.incoming.market.MarketAddResult;
import kabam.rotmg.messaging.impl.incoming.market.MarketMyOffersResult;
import kabam.rotmg.messaging.impl.incoming.market.MarketRemoveResult;

import robotlegs.bender.framework.api.ILogger;


public class MemMarketSellTab extends MemMarketTab
{
    private static const SLOT_X_OFFSET:int = 33;
    private static const SLOT_Y_OFFSET:int = 109;

    private static const RESULT_X_OFFSET:int = 270;
    private static const RESULT_Y_OFFSET:int = 105;

    private static const ICON_X:int = 212;
    private static const ICON_Y:int = 356;

    private static const FAME:String = "Fame";
    private static const GOLD:String = "Fame";
    private static const CURRENCY_CHOICES:Vector.<String> = new <String> /* List of available sell currencies */
    [
        FAME,
        GOLD
    ];

    private static const HOURS_3:String = "3 Hours";
    private static const HOURS_6:String = "6 Hours";
    private static const HOURS_12:String = "12 Hours";
    private static const HOURS_72:String = "72 Hours";
    private static const UPTIME_CHOICES:Vector.<String> = new <String> /* List of available uptime values */
    [
        HOURS_72
    ];

    /* Signals */
    public const addSignal_:MemMarketAddSignal = new MemMarketAddSignal();
    public const removeSignal_:MemMarketRemoveSignal = new MemMarketRemoveSignal();
    public const myOffersSignal_:MemMarketMyOffersSignal = new MemMarketMyOffersSignal();

    private var inventorySlots_:Vector.<MemMarketInventoryItem>;
    private var priceField_:MarketInput;
    //private var currencyChoice_:DropDown;
    //private var currencyFame_:Bitmap;
    //private var currencyGold_:Bitmap;
    private var uptimeChoice_:DropDown;
    private var sellButton_:DeprecatedTextButton;
    private var shape_:Shape;
    private var resultMask_:Sprite;
    private var resultBackground_:Sprite;
    private var resultItems_:Vector.<MemMarketSellItem>;
    private var resultScroll_:Scrollbar;
    private var sortChoices_:DropDown;
    private var uptime_:int;
    private var slots_:Vector.<int>;
    private var price_:int;
    //private var selectedCurrency_:int;

    public function MemMarketSellTab(gameSprite:GameSprite)
    {
        super(gameSprite);
        /* Initialize signals */
        this.addSignal_.add(this.onAdd);
        this.removeSignal_.add(this.onRemove);
        this.myOffersSignal_.add(this.onMyOffers);

        this.inventorySlots_ = new Vector.<MemMarketInventoryItem>();
        for (var i:int = 4; i < this.gameSprite_.map.player_.equipment_.length; i++) /* Start at index 4 so we dont include equipment */
        {
            var item:MemMarketInventoryItem = new MemMarketInventoryItem(this.gameSprite_, this.gameSprite_.map.player_.equipment_[i], i);
            this.inventorySlots_.push(item);
        }

        var space:int = 0;
        var index:int = 0;
        for each (var o:MemMarketInventoryItem in this.inventorySlots_)
        {
            o.x = MemMarketItem.SLOT_WIDTH * int(index % 4) + SLOT_X_OFFSET;
            o.y = MemMarketItem.SLOT_HEIGHT * int(index / 4) + SLOT_Y_OFFSET + space;
            index++; /* Increase before we check spacing */
            if (index % 8 == 0) /* Add small space between each inventory */
            {
                space = 10;
            }
            addChild(o);
        }

        this.priceField_ = new MarketInput("", false, "");
        this.priceField_.inputText_.restrict = "0-9";
        this.priceField_.x = 13;
        this.priceField_.y = 330;
        addChild(this.priceField_);

        /*this.currencyChoice_ = new DropDown(CURRENCY_CHOICES, 133, 26, "Currency");
        this.currencyChoice_.x = 10;
        this.currencyChoice_.y = 368;
        this.currencyChoice_.setValue(FAME);
        this.currencyChoice_.addEventListener(Event.CHANGE, this.onCurrencyChanged);
        addChild(this.currencyChoice_);

        this.currencyFame_ = new Bitmap(IconUtils.getFameIcon(68));
        this.currencyFame_.x = ICON_X;
        this.currencyFame_.y = ICON_Y;
        this.currencyFame_.visible = false;
        addChild(this.currencyFame_);

        this.currencyGold_ = new Bitmap(IconUtils.getCoinIcon(68));
        this.currencyGold_.x = ICON_X;
        this.currencyGold_.y = ICON_Y;
        this.currencyGold_.visible = false;
        addChild(this.currencyGold_); */

        //this.updateIcon();

        this.uptimeChoice_ = new DropDown(UPTIME_CHOICES, 176, 26, "Uptime");
        this.uptimeChoice_.x = 10;
        this.uptimeChoice_.y = 368;
        this.uptimeChoice_.setValue(HOURS_72);
        this.uptimeChoice_.addEventListener(Event.CHANGE, this.onUptimeChanged);
        addChild(this.uptimeChoice_);

        this.uptime_ = this.getHours();

        this.sellButton_ = new DeprecatedTextButton(16, "Sell", 243);
        this.sellButton_.x = 10;
        this.sellButton_.y = 405;
        this.sellButton_.addEventListener(MouseEvent.CLICK, this.onSell);
        addChild(this.sellButton_);

        this.shape_ = new Shape();
        this.shape_.graphics.beginFill(0);
        this.shape_.graphics.drawRect(RESULT_X_OFFSET, RESULT_Y_OFFSET, 500, 415);
        this.shape_.graphics.endFill();
        this.resultMask_ = new Sprite();
        this.resultMask_.addChild(this.shape_);
        this.resultMask_.mask = this.shape_;
        addChild(this.resultMask_);
        this.resultBackground_ = new Sprite();
        this.resultMask_.addChild(this.resultBackground_);

        this.resultItems_= new Vector.<MemMarketSellItem>();

        this.sortChoices_ = new DropDown(SortUtils.SORT_CHOICES, 200, 26);
        this.sortChoices_.x = 597;
        this.sortChoices_.y = 71;
        //this.sortChoices_.setValue(SortUtils.JUST_ADDED);
        this.sortChoices_.addEventListener(Event.CHANGE, this.onSortChoicesChanged);
        addChild(this.sortChoices_);

        this.gameSprite_.gsc_.marketMyOffers();
    }

    private function onSortChoicesChanged(event:Event) : void
    {
        this.sortOffers();
    }

    private function onSell(event:MouseEvent) : void
    {
        this.price_ = int(this.priceField_.text());
        if (this.price_ <= 0)
        {
            DialogUtils.makeSimpleDialog(this.gameSprite_, "Error", "Invalid price.");
            return;
        }

        this.slots_ = new Vector.<int>();
        for each (var i:MemMarketInventoryItem in this.inventorySlots_)
        {
            if (!i.selected_)
            {
                continue;
            }
            this.slots_.push(i.slot_);
        }

        if (this.slots_.length <= 0)
        {
            DialogUtils.makeSimpleDialog(this.gameSprite_, "Error", "You must select at least 1 item.");
            return;
        }

        /*switch (CURRENCY_CHOICES[this.currencyChoice_.getIndex()])
        {
            case FAME:
                this.selectedCurrency_ = Currency.FAME;
                break;
            case GOLD:
                this.selectedCurrency_ = Currency.GOLD;
                break;
        } */

        DialogUtils.makeCallbackDialog(this.gameSprite_, "Verification", "Sell these items?", "Yes", "No", this.onVerified);
    }

    private function onVerified(event:Event) : void
    {
        //this.gameSprite_.gsc_.marketAdd(this.slots_, this.price_, Currency.FAME, this.uptime_);
    }

    /*private function onCurrencyChanged(event:Event) : void
    {
        this.updateIcon();
    } */

    /*private function updateIcon() : void
    {
        switch (CURRENCY_CHOICES[this.currencyChoice_.getIndex()])
        {
            case FAME:
                this.currencyFame_.visible = true;
                this.currencyGold_.visible = false;
                break;
            case GOLD:
                this.currencyFame_.visible = false;
                this.currencyGold_.visible = true;
                break;
        }
    } */

    private function onUptimeChanged(event:Event) : void
    {
        this.uptime_ = this.getHours();
    }

    private function getHours() : int
    {
        var str:String = UPTIME_CHOICES[this.uptimeChoice_.getIndex()];
        var hours:int = int(str.split(' ')[0]); /* Pretty scuffed, but i didnt want to make a switch statement for this */
        return hours;
    }

    /* Adds and refresh all of our offers */
    private function onAdd(result:MarketAddResult) : void
    {
        if (result.code_ != -1)
        {
            this.slots_.length = 0;
            DialogUtils.makeSimpleDialog(this.gameSprite_, "Error", result.description_);
            return;
        }

        /* Reset sold inventory slots */
        for each (var i:MemMarketInventoryItem in this.inventorySlots_)
        {
            for each (var x:int in this.slots_)
            {
                if (i.slot_ == x)
                {
                    i.reset();
                }
            }
        }

        /* Reset slots */
        this.slots_.length = 0;

        /* Request our items back */
        this.gameSprite_.gsc_.marketMyOffers();

        DialogUtils.makeSimpleDialog(this.gameSprite_, "Success", result.description_);
    }

    /* This really only gets called when there's an error */
    private function onRemove(result:MarketRemoveResult) : void
    {
        if (result.description_ != "")
        {
            DialogUtils.makeSimpleDialog(this.gameSprite_, "Error", result.description_);
        }
    }

    /* Refresh all of our offers */
    private function onMyOffers(result:MarketMyOffersResult) : void
    {
        /* Remove old scrollbar */
        if (this.resultScroll_ != null)
        {
            this.resultScroll_.removeEventListener(Event.CHANGE, this.onResultScrollChanged);
            removeChild(this.resultScroll_);
            this.resultScroll_ = null;
        }

        this.clearPreviousResults();

        for each (var i:MarketData in result.results_)
        {
            var item:MemMarketSellItem = new MemMarketSellItem(this.gameSprite_, i);
            this.resultItems_.push(item);
        }

        this.sortOffers();

        for each (var o:MemMarketSellItem in this.resultItems_)
        {
            this.resultBackground_.addChild(o);
        }

        this.resultBackground_.y = 0; /* Reset height */
        if (this.resultBackground_.height > 436)
        {
            this.resultScroll_ = new Scrollbar(22, 415);
            this.resultScroll_.x = 774;
            this.resultScroll_.y = RESULT_Y_OFFSET;
            this.resultScroll_.setIndicatorSize(415, this.resultBackground_.height);
            this.resultScroll_.addEventListener(Event.CHANGE, this.onResultScrollChanged);
            addChild(this.resultScroll_);
        }
    }

    private function onResultScrollChanged(event:Event) : void
    {
        this.resultBackground_.y = -this.resultScroll_.pos() * (this.resultBackground_.height - 418);
    }

    private function clearPreviousResults() : void
    {
        for each (var i:MemMarketSellItem in this.resultItems_)
        {
            i.dispose();
            this.resultBackground_.removeChild(i);
            i = null;
        }
        this.resultItems_.length = 0;
    }

    private function sortOffers() : void
    {
        switch (SortUtils.SORT_CHOICES[this.sortChoices_.getIndex()])
        {
            case SortUtils.LOWEST_TO_HIGHEST:
                this.resultItems_.sort(SortUtils.lowestToHighest);
                break;
            case SortUtils.HIGHEST_TO_LOWEST:
                this.resultItems_.sort(SortUtils.highestToLowest);
                break;
            /*case SortUtils.FAME_TO_GOLD:
                this.resultItems_.sort(SortUtils.fameToGold);
                break;
            case SortUtils.GOLD_TO_FAME:
                this.resultItems_.sort(SortUtils.goldToFame);
                break; */
        }

        var index:int = 0;
        for each (var i:MemMarketSellItem in this.resultItems_)
        {
            i.x = MemMarketItem.OFFER_WIDTH * int(index % 5) + RESULT_X_OFFSET;
            i.y = MemMarketItem.OFFER_HEIGHT * int(index / 5) + RESULT_Y_OFFSET;
            index++;
        }
    }

    /* Clear */
    public override function dispose() : void
    {
        this.addSignal_.remove(this.onAdd);
        this.removeSignal_.remove(this.onRemove);
        this.myOffersSignal_.remove(this.onMyOffers);

        for each (var i:MemMarketInventoryItem in this.inventorySlots_)
        {
            i.dispose();
            i = null;
        }
        this.inventorySlots_.length = 0;
        this.inventorySlots_ = null;

        this.priceField_ = null;
        /*this.currencyChoice_.removeEventListener(Event.CHANGE, this.onCurrencyChanged);
        this.currencyChoice_ = null;
        this.currencyFame_ = null;
        this.currencyGold_ = null; */
        this.uptimeChoice_.removeEventListener(Event.CHANGE, this.onUptimeChanged);
        this.uptimeChoice_ = null;
        this.sellButton_.removeEventListener(MouseEvent.CLICK, this.onSell);
        this.sellButton_ = null;

        this.shape_.parent.removeChild(this.shape_);
        this.shape_ = null;

        this.clearPreviousResults();
        this.resultItems_ = null;

        this.resultMask_.removeChild(this.resultBackground_);
        this.resultMask_ = null;
        this.resultBackground_ = null;

        if (this.resultScroll_ != null)
        {
            this.resultScroll_.removeEventListener(Event.CHANGE, this.onResultScrollChanged);
            this.resultScroll_ = null;
        }

        this.sortChoices_.removeEventListener(Event.CHANGE, this.onSortChoicesChanged);
        this.sortChoices_ = null;

        if (this.slots_ != null)
        {
            this.slots_.length = 0;
            this.slots_ = null;
        }

        super.dispose();
    }
}
}
