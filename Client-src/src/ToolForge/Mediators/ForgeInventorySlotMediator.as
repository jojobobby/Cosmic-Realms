package ToolForge.Mediators
{
    import ToolForge.ForgeInventorySlot;
    import com.company.assembleegameclient.ui.tooltip.ToolTip;
    import kabam.rotmg.core.signals.ShowTooltipSignal;
    import robotlegs.bender.bundles.mvcs.Mediator;

    public class ForgeInventorySlotMediator extends Mediator
    {
        [Inject]
        public var showToolTip:ShowTooltipSignal;
        [Inject]
        public var view:ForgeInventorySlot;

        override public function initialize():void
        {
            this.view.tooltipSignal.add(this.showToolTipCallback);
        }
        override public function destroy():void
        {
            this.view.tooltipSignal.remove(this.showToolTipCallback);
        }
        private function showToolTipCallback(tip:ToolTip):void
        {
            this.showToolTip.dispatch(tip);
        }
    }
}