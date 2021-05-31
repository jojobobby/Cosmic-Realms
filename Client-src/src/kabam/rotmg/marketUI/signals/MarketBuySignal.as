package kabam.rotmg.marketUI.signals {
import kabam.rotmg.messaging.impl.incoming.market.MarketBuyResult;

import org.osflash.signals.Signal;

public class MarketBuySignal extends Signal {
    public static var _staticInstance:MarketBuySignal;

    public function MarketBuySignal() {
        super(MarketBuyResult);
        _staticInstance = this;
    }
}
}
