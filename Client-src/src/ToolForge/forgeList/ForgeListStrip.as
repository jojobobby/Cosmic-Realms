package ToolForge.forgeList {
import com.company.assembleegameclient.game.GameSprite;

import flash.display.Sprite;

import kabam.rotmg.messaging.impl.incoming.ForgeListResult;

public class ForgeListStrip extends Sprite {

    public function ForgeListStrip(gs:GameSprite, list:ForgeListResult) {

        var rX:int = 0;
        var ln:int = 0;

        for each(var r:String in list.Recipes) {
            ln++;

            var recipe:ForgeListItem = new ForgeListItem(gs, r, true);
            recipe.x = rX;

            rX += recipe.width + 5;

            if (ln < list.Recipes.length) {
                var cross:Sprite = crossGraphics();
                cross.x = rX;
                cross.y = 20;

                rX += cross.width + 5;

                this.addChild(cross);
            }

            this.addChild(recipe);
        }

        var result:ForgeListItem = new ForgeListItem(gs, list.Result);
        result.x = 450;
        this.addChild(result);
    }

    private function crossGraphics():Sprite {
        var cross:Sprite = new Sprite();

        cross.graphics.lineStyle(3, 0xFFFFFF);
        cross.graphics.moveTo(0, 5);
        cross.graphics.lineTo(10, 5);
        cross.graphics.moveTo(5, 0);
        cross.graphics.lineTo(5, 10);

        return cross;
    }
}
}
