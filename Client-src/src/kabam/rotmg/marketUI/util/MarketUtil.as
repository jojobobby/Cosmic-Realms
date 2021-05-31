package kabam.rotmg.marketUI.util {
import kabam.rotmg.messaging.impl.data.MarketData;

import mx.collections.Sort;

public class MarketUtil {
    public static const TAB_WIDTH:int = 630;
    public static const TAB_HEIGHT:int = 550;

    public static const MAX_OFFERS:int = 9;

    public static const BUY_TAB:String = "BUY";
    public static const SELL_TAB:String = "SELL";

    public static var CURTYPESORT:int = 0;
    public static var CURRARITYSORT:int = 0;

    public static const SORT_TYPE:Vector.<String> = new <String>[
        "All", "Weapons", "Abilities", "Armors", "Rings", "Misc"
    ];
    public static const SORT_RARITY:Vector.<String> = new <String>[
        "All", "Tier", "UT", "LG", "MY"
    ];
    public static const SORT_PRICE:Vector.<String> = new <String>[
        "Greatest", "Least"
    ];

    public static function GETSORTTYPE(sort:String):int {
        return SORT_TYPE.indexOf(sort);
    }

    public static function GETSORTRARITY(sort:String):int {
        return SORT_RARITY.indexOf(sort);
    }
    public static function GETSORTPRICE(sort:String):int {
        return SORT_PRICE.indexOf(sort);
    }

    public static function LowToHigh(price1:MarketData, price2:MarketData):int {
        if (price1.price_ < price2.price_) {
            return -1;
        }
        else if (price1.price_ > price2.price_) {
            return 1;
        }
        else return 0;

    }

    public static function HighToLow(price1:MarketData, price2:MarketData):int {
        if (price1.price_ < price2.price_) {
            return 1;
        }
        else if (price1.price_ > price2.price_) {
            return -1;
        }
        else return 0;
    }
}
}
