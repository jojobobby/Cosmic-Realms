package com.company.assembleegameclient.objects
{
    import ToolForge.ToolForgePanel;
    import com.company.assembleegameclient.game.GameSprite;
    import com.company.assembleegameclient.ui.panels.Panel;

    public class ForgeStation extends GameObject implements IInteractiveObject
    {

        public function ForgeStation(_arg1:XML)
        {
            super(_arg1);
            isInteractive_ = true;
        }

        public function getPanel(_arg1:GameSprite):Panel
        {
            return new ToolForgePanel(_arg1);
        }
    }
}