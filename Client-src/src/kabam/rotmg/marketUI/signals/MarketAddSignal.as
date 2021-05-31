package kabam.rotmg.marketUI.signals {
import kabam.rotmg.messaging.impl.incoming.market.MarketAddResult;

import org.osflash.signals.Signal;

public class MarketAddSignal extends Signal {

    public static var _staticInstance:Signal;

    public function MarketAddSignal() {
        super(MarketAddResult);
        _staticInstance = this;
    }

}
}
