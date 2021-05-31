package com.company.assembleegameclient.game {
import com.company.assembleegameclient.map.AbstractMap;
import com.company.assembleegameclient.map.Camera;
import com.company.assembleegameclient.objects.GameObject;
import com.company.assembleegameclient.tutorial.Tutorial;

import flash.display.Sprite;

import kabam.rotmg.core.model.PlayerModel;
import kabam.rotmg.messaging.impl.GameServerConnection;
import kabam.rotmg.messaging.impl.incoming.MapInfo;
import kabam.rotmg.ui.view.HUDView;

import org.osflash.signals.Signal;
import flash.display.GraphicsBitmapFill;
import flash.display.GraphicsEndFill;
import flash.display.GraphicsPath;
import flash.display.GraphicsPathCommand;
import flash.display.IGraphicsData;
import flash.geom.Matrix;
import com.company.assembleegameclient.map.Camera;
import flash.display.BitmapData;
import com.company.util.GraphicsUtil;
import WeatherStuff.AtmosphereHandler;

public class AGameSprite extends Sprite {

    public const closed:Signal = new Signal();
    public var isEditor:Boolean;
    public var tutorial_:Tutorial;
    public var mui_:MapUserInput;
    public var lastUpdate_:int;
    public var moveRecords_:MoveRecords;
    public var map:AbstractMap;
    public var model:PlayerModel;
    public var hudView:HUDView;
    public var camera_:Camera;
    public var gsc_:GameServerConnection;
    public const lgDropped:Signal = new Signal();
    public const myDropped:Signal = new Signal();
    public var atmosphere_:AtmosphereHandler;

    public function AGameSprite() {
        this.moveRecords_ = new MoveRecords();
        this.camera_ = new Camera();
        super();
    }

    public function initialize():void {
    }

    public function setFocus(_arg1:GameObject):void {
    }

    public function applyMapInfo(_arg1:MapInfo):void {
    }

    public function evalIsNotInCombatMapArea():Boolean {
        return (false);
    }


}
}
