package ToolForge.Mediators
{
    import ToolForge.ForgeModifierButton;
    import com.company.assembleegameclient.ui.tooltip.ToolTip;
    import kabam.rotmg.core.signals.ShowTooltipSignal;
    import robotlegs.bender.bundles.mvcs.Mediator;

    public class ForgeModifierButtonMediator extends Mediator
    {
        [Inject]
        public var showToolTip:ShowTooltipSignal;
        [Inject]
        public var view:ForgeModifierButton;

        override public function initialize():void
        {
            this.view.tooltipSignal.add(this.showToolTipCallback);
        }

        private function showToolTipCallback(tip:ToolTip):void
        {
            this.showToolTip.dispatch(tip);
        }

        override public function destroy():void
        {
            this.view.tooltipSignal.remove(this.showToolTipCallback);
        }
    }
}