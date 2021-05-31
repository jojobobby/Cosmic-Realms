package kabam.rotmg.marketUI.signals {
import kabam.rotmg.memMarket.signals.MemMarketMyOffersSignal;
import kabam.rotmg.messaging.impl.incoming.market.MarketMyOffersResult;

import org.osflash.signals.Signal;

public class MarketOffersSignal extends Signal {

    public static var _staticInstance:MarketOffersSignal;

    public function MarketOffersSignal() {
        super(MarketMyOffersResult);
        _staticInstance = this;
    }
}
}
