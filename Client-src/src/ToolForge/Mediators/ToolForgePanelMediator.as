package ToolForge.Mediators
{
import ToolForge.ToolForgePanel;
import ToolForge.ForgeLock;
import ToolForge.ToolForgeFrame;
import com.company.assembleegameclient.game.GameSprite;
import kabam.rotmg.dialogs.control.OpenDialogSignal;
import robotlegs.bender.bundles.mvcs.Mediator;

public class ToolForgePanelMediator extends Mediator
{
    [Inject]
    public var dialogSignal:OpenDialogSignal;
    [Inject]
    public var view:ToolForgePanel;
    [Inject]
    public var forgeLock:ForgeLock;
    override public function initialize():void
    {
        this.view.openFrame.add(this.openFrame);
    }
    override public function destroy():void
    {
        this.view.openFrame.remove(this.openFrame);
    }
    private function openFrame():void
    {
        if (!this.forgeLock.isOpen)
        {
            this.dialogSignal.dispatch(new ToolForgeFrame(GameSprite(this.view.gs_)));
        }
    }
}
}