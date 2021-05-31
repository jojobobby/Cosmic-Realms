package ToolForge.Mediators {
import ToolForge.forgeList.ForgeListItem;

import com.company.assembleegameclient.ui.tooltip.ToolTip;

import kabam.rotmg.core.signals.ShowTooltipSignal;

import robotlegs.bender.bundles.mvcs.Mediator;

public class ForgeListItemMediator extends Mediator {

    [Inject]
    public var showToolTip:ShowTooltipSignal;
    [Inject]
    public var view:ForgeListItem;

    override public function initialize():void
    {
        this.view.toolTipSignal.add(showToolTipCallback);
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
