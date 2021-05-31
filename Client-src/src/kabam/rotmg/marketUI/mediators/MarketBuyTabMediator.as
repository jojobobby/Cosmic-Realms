package kabam.rotmg.marketUI.mediators {
import kabam.rotmg.marketUI.signals.MarketSearchSignal;
import kabam.rotmg.marketUI.view.MarketBuyTab;

import robotlegs.bender.bundles.mvcs.Mediator;

public class MarketBuyTabMediator extends Mediator {

    [Inject]
    public var view:MarketBuyTab;
    [Inject]
    public var onSearch:MarketSearchSignal;

    override public function initialize():void
    {
        //this.onSearch.add(this.view.)
    }

    override public function destroy():void
    {
    }

}
}
