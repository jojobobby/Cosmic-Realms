package kabam.rotmg.marketUI.signals {
import kabam.rotmg.messaging.impl.incoming.market.MarketRemoveResult;

import org.osflash.signals.Signal;

public class MarketRemoveSignal extends Signal {

    public static var _staticInstance:MarketRemoveSignal;

    public function MarketRemoveSignal() {
        super(MarketRemoveResult);
        _staticInstance = this;
    }

}
}
