/**
 * Created by Gurk on 2/4/2017.
 */
package WeatherStuff {
import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.map.Map;

import flash.display.BlendMode;
import flash.display.Sprite;
import flash.errors.IllegalOperationError;
import com.gskinner.motion.GTween;
import com.company.assembleegameclient.parameters.Parameters;
//import Sounds.Music;

public class AtmosphereHandler extends Sprite {

    public static const SUN:String = "WEATHER_SUN";
    public static const HIGH_CLOUDY:String = "WEATHER_HIGH_CLOUDY";
    //public static const LIGHT_CLOUDY:String = "WEATHER_LIGHT_CLOUDY";

    public static const DAY:String = "DAYTIME_DAY";
    public static const NIGHT:String = "DAYTIME_NIGHT";

    public var CurrentAtmosphereString:String;
    public var OldAtmosphereString:String;

    private var gs_:AGameSprite;
    private var map_:Map;

    private var atmOvl:Sprite;

    public function AtmosphereHandler(_arg1:AGameSprite, map:Map) {
        this.gs_ = _arg1;
        this.map_ = map;
        this.atmOvl = new Sprite();
        this.addChild(atmOvl);

        this.mouseEnabled = false;
        this.atmOvl.mouseEnabled = false;
    }

    public function init(_arg1:int):void {
        if (_arg1 != -1) {
            CurrentAtmosphereString = OldAtmosphereString = getAtmosphereString(_arg1);
            var color = getCurrentAtmosphereColor();
            this.atmOvl.graphics.beginFill(color, 0.5);
            this.atmOvl.graphics.drawRect(-1200, -1625, 5000, 5000);
            this.atmOvl.graphics.endFill();
            this.blendMode = BlendMode.OVERLAY;

            if (color != uint.MAX_VALUE) {
                this.atmOvl.alpha = 0.8
            }
        }
    }

    public function getCurrentAtmosphereColor():uint {
        switch (CurrentAtmosphereString) {
            case NIGHT:
                return 0x0004FF;
            case DAY:
                return uint.MAX_VALUE;
            default:
                throw new IllegalOperationError("Invalid atmosphere status!");
        }
    }

    public function tick(_arg1:int):void {
        OldAtmosphereString = CurrentAtmosphereString;
        CurrentAtmosphereString = getAtmosphereString(_arg1);

        if (CurrentAtmosphereString != OldAtmosphereString) {
            var color = getCurrentAtmosphereColor();
            var _local1:GTween;
            if (color == uint.MAX_VALUE) {
                _local1 = new GTween(this.atmOvl, 50, {"alpha": -1.0});
                _local1.onChange = endIfAlphaChanged;
            }
            else {
                this.atmOvl.graphics.beginFill(color, 0.5);
                this.atmOvl.graphics.drawRect(-1200, -1625, 5000, 5000);
                this.atmOvl.graphics.endFill();
                this.atmOvl.alpha = 0.0;
                _local1 = new GTween(this.atmOvl, 50, {"alpha": 1.0});
                _local1.onChange = endIfAlphaChanged;
            }
        }
    }

    public function update():void {
        if (atmOvl.alpha > 0.8) atmOvl.alpha = 0.8;
        if (atmOvl.alpha < 0.0) atmOvl.alpha = 0.0;

        this.y = Parameters.data_.centerOnPlayer ? 0 : -125;
    }

    public function switchTo(_arg1:String, _arg2:int):void {
        if (_arg2 < 30000 && CurrentAtmosphereString != _arg1) {
            CurrentAtmosphereString = OldAtmosphereString = _arg1;
            var color = getCurrentAtmosphereColor();
            if (color == uint.MAX_VALUE) {
                var _local1:GTween = new GTween(this, 50, {"alpha": -1.0});
                _local1.onChange = endIfAlphaChanged;
            }
            else {
                atmOvl.graphics.beginFill(color, 0.5);
                atmOvl.graphics.drawRect(-1200, -1625, 5000, 5000);
                atmOvl.graphics.endFill();
                atmOvl.alpha = 0.0;
                var _local1:GTween = new GTween(this, 50, {"alpha": 1.0});
                _local1.onChange = endIfAlphaChanged;
            }
        }
    }

    public function getAtmosphereString(_arg1:int):String {
        if (_arg1 >= 40000 && CurrentAtmosphereString == SUN) {
            return DAY;
        }
        else if (_arg1 >= 48000) {
            return NIGHT;
        }
        else {
            return DAY;
        }
    }

    private function endIfAlphaChanged(_arg1:GTween):void {
        if (atmOvl.alpha > 0.8 || atmOvl.alpha < 0.0) {
            _arg1.end();
        }
    }
}
}