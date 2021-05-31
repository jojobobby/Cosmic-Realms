package com.company.assembleegameclient.objects
{
   import com.company.assembleegameclient.game.GameSprite;
   import com.company.assembleegameclient.ui.panels.GuildBoardPanel;
   import com.company.assembleegameclient.ui.panels.Panel;

import kabam.rotmg.marketUI.MarketPanel;

import kabam.rotmg.memMarket.MemMarketPanel;

public class MarketObject extends GameObject implements IInteractiveObject
   {

      public function MarketObject(objectXML:XML)
      {
         super(objectXML);
         isInteractive_ = true;
      }
      
      public function getPanel(gs:GameSprite) : Panel
      {
         return new MarketPanel(gs);
      }
   }
}
