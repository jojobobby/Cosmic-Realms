package kabam.rotmg.potionStorage {
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.ui.panels.ButtonPanel;
import flash.events.MouseEvent;
public class PotionStoragePanel extends ButtonPanel
{
    public function PotionStoragePanel(gameSprite:GameSprite)
    {
        super(gameSprite, "Potion Storage", "Open");
    }
    override protected function onButtonClick(event:MouseEvent) : void
    {
        this.gs_.mui_.setEnablePlayerInput(false);
        this.gs_.addChild(new PotionStorage(this.gs_.mui_.gs_));
    }
}
}