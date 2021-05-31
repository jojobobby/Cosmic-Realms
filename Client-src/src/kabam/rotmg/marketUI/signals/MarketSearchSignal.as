package kabam.rotmg.marketUI.signals {
import kabam.rotmg.messaging.impl.incoming.market.MarketSearchResult;

import org.osflash.signals.Signal;

public class MarketSearchSignal extends Signal {

    public static var _staticInstance:MarketSearchSignal;

    public function MarketSearchSignal() {
        super(MarketSearchResult);
        _staticInstance = this;

    }

}
}
