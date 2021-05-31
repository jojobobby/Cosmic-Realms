package kabam.rotmg.marketUI.mediators {
import com.company.assembleegameclient.ui.tooltip.ToolTip;

import kabam.rotmg.core.signals.ShowTooltipSignal;
import kabam.rotmg.marketUI.components.items.MarketItemBase;

import robotlegs.bender.bundles.mvcs.Mediator;

public class MarketItemBaseMediator extends Mediator {
    [Inject]
    public var showToolTip:ShowTooltipSignal;
    [Inject]
    public var view:MarketItemBase;

    override public function initialize():void
    {
        this.view.ShowTooltip.add(showToolTipCallback);
    }
    override public function destroy():void
    {
    }
    private function showToolTipCallback(tip:ToolTip):void
    {
        showToolTip.dispatch(tip);
    }

}
}
