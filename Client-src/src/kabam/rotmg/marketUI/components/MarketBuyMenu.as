package kabam.rotmg.marketUI.components {
import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.ui.DeprecatedTextButton;
import com.company.assembleegameclient.ui.MarketInput;
import com.company.assembleegameclient.ui.dropdown.DropDown;
import com.company.ui.BaseSimpleText;

import flash.display.Sprite;
import flash.events.KeyboardEvent;

import kabam.rotmg.marketUI.signals.PriceSortSignal;
import kabam.rotmg.marketUI.signals.SearchSortSignal;

import kabam.rotmg.marketUI.util.MarketUtil;

import org.osflash.signals.Signal;

public class MarketBuyMenu extends Sprite {

    private var _gs:AGameSprite;

    private var _typeTitle:BaseSimpleText;
    private var _rarityTitle:BaseSimpleText;
    private var _priceTitle:BaseSimpleText;

    private var _typeDropDown:DropDown;
    private var _rarityDropDown:DropDown;
    private var _priceDropDown:DropDown;

    private var _searchBar:MarketInput;

    public var NewSearchSignal:Signal;

    private function onTypeChanged(val:String):void {
        this._gs.gsc_.marketSearch(MarketUtil.GETSORTTYPE(val), MarketUtil.GETSORTRARITY(this._rarityDropDown.getValue()));
        MarketUtil.CURTYPESORT = MarketUtil.GETSORTTYPE(val);
    }
    private function onRarityChanged(val:String):void {
        this._gs.gsc_.marketSearch(MarketUtil.GETSORTTYPE(this._typeDropDown.getValue()), MarketUtil.GETSORTRARITY(val));
        MarketUtil.CURRARITYSORT =  MarketUtil.GETSORTRARITY(val);
    }

    private function onPriceChanged(val:String):void {
        PriceSortSignal._staticInstance.dispatch(MarketUtil.GETSORTPRICE(val));
    }

    private function onSearchChanged(val:KeyboardEvent):void {
        if (this._searchBar.text() != null) {
            SearchSortSignal._staticInstance.dispatch(_searchBar.text());
        } else {
            SearchSortSignal._staticInstance.dispatch("");
        }
    }

    public function reset():void {
        this._searchBar.inputText_.setText("");
        this._searchBar.inputText_.updateMetrics();
        this._typeDropDown.setIndex(0);
        this._rarityDropDown.setIndex(0);
        this._typeDropDown.setIndex(0);
        this._priceDropDown.setIndex(0);
    }

    public function MarketBuyMenu(gs:AGameSprite) {
        this._gs = gs;
        this.NewSearchSignal = new Signal();

        this._typeTitle = new BaseSimpleText(12, 0xFFFFFF);
        this._rarityTitle = new BaseSimpleText(12, 0xFFFFFF);
        this._priceTitle = new BaseSimpleText(12, 0xFFFFFF);

        this._typeDropDown = new DropDown(MarketUtil.SORT_TYPE, 140, 25);
        this._rarityDropDown = new DropDown(MarketUtil.SORT_RARITY, 140, 25);
        this._priceDropDown = new DropDown(MarketUtil.SORT_PRICE, 140, 25);
        this._searchBar = new MarketInput("Search By Name:", false, "", 16, 140, 20);

        this._typeDropDown.addCallBack(onTypeChanged);
        this._rarityDropDown.addCallBack(onRarityChanged);
        this._priceDropDown.addCallBack(onPriceChanged);
        this._searchBar.addEventListener(KeyboardEvent.KEY_UP, onSearchChanged);

        this._typeTitle.setText("Sort By Type:");
        this._rarityTitle.setText("Sort By Rarity:");
        this._priceTitle.setText("Sort By Price:");

        this._typeTitle.updateMetrics();
        this._rarityTitle.updateMetrics();
        this._priceTitle.updateMetrics();

        this._typeTitle.x = 5;
        this._rarityTitle.x = 5;
        this._priceTitle.x = 5;

        this._typeDropDown.x = 5;
        this._rarityDropDown.x = 5;
        this._priceDropDown.x = 5;
        this._searchBar.x = 5;

        this._typeTitle.y = 7;
        this._rarityTitle.y = 57;
        this._priceTitle.y = 107;

        this._typeDropDown.y = 25;
        this._rarityDropDown.y = 75;
        this._priceDropDown.y = 125;
        this._searchBar.y = 230;

        addChild(this._typeTitle);
        addChild(this._rarityTitle);
        addChild(this._priceTitle);

        addChild(this._typeDropDown);
        addChild(this._rarityDropDown);
        addChild(this._priceDropDown);
        addChild(this._searchBar);
    }
}
}
