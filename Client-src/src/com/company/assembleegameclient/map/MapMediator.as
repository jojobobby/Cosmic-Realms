package com.company.assembleegameclient.map {
import kabam.rotmg.game.view.components.QueuedStatusText;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;
import com.company.assembleegameclient.background.Background;
import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.map.mapoverlay.MapOverlay;
import com.company.assembleegameclient.map.partyoverlay.PartyOverlay;
import com.company.assembleegameclient.objects.BasicObject;
import com.company.assembleegameclient.objects.GameObject;
import com.company.assembleegameclient.objects.Party;
import com.company.assembleegameclient.objects.Player;
import flash.display.Sprite;
import flash.geom.Point;
import flash.utils.Dictionary;
import robotlegs.bender.bundles.mvcs.Mediator;
import WeatherStuff.AtmosphereHandler;
public class MapMediator extends Mediator {

    [Inject]
    public var view:Map;
    [Inject]
    public var queueStatusText:QueueStatusTextSignal;


    override public function initialize():void {
        this.queueStatusText.add(this.onQueuedStatusText);
    }

    override public function destroy():void {
        this.queueStatusText.remove(this.onQueuedStatusText);
    }

    private function onQueuedStatusText(_arg1:String, _arg2:uint):void {
        ((this.view.player_) && (this.queueText(_arg1, _arg2)));
    }

    private function queueText(_arg1:String, _arg2:uint):void {
        var _local3:QueuedStatusText = new QueuedStatusText(this.view.player_, new LineBuilder().setParams(_arg1), _arg2, 2000, 0);
        this.view.mapOverlay_.addQueuedText(_local3);
    }


}
}
