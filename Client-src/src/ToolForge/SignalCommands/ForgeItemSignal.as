package ToolForge.SignalCommands
{
    import com.company.assembleegameclient.game.AGameSprite;
    import ToolForge.ForgeItemInformation;
    import org.osflash.signals.Signal;

    public class ForgeItemSignal extends Signal
    {

        public function ForgeItemSignal()
        {
            super(AGameSprite, ForgeItemInformation);
        }
    }
}