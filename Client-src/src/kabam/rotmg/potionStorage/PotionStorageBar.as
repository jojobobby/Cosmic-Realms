package kabam.rotmg.potionStorage
{

import com.company.assembleegameclient.map.AbstractMap;
import com.company.assembleegameclient.objects.GameObject;
import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.objects.Player;

import flash.display.Sprite;
import flash.text.TextField;

import kabam.rotmg.messaging.impl.data.ObjectData;

import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;

import mx.controls.Text;

public class PotionStorageBar extends Sprite
{

    public function PotionStorageBar(innerFill:uint, type:int)
    {
        super();
        var _loc1_:TextFieldDisplayConcrete = new TextFieldDisplayConcrete().setSize(10).setBold(true).setColor(0xffffff);
        var _loc2_:int;

        this.graphics.beginFill(0x202020);
        this.graphics.drawRoundRect(0, 0, 75, 14, 5, 5);
        this.graphics.endFill();

        this.graphics.beginFill(innerFill);
        this.graphics.drawRoundRect(2, 2, 7.1 * (1), 10, 3, 3);
        this.graphics.endFill();

        /*_loc1_.x = 3;
        _loc1_.y = 3;
        _loc1_.setStringBuilder(new StaticStringBuilder(_loc2_.toString()));
        addChild(_loc1_);*/
    }
}
}
