/**
 * Created by Gurk on 2/4/2017.
 */
package kabam.rotmg.messaging.impl.incoming {
import flash.utils.IDataInput;

public class WeatherPropertiesPacket extends IncomingMessage {

    public static const NONE:int = 255;
    public static const ADD_PARTICLES:int = 0;
    public static const REMOVE_PARTICLES:int = 1;
    public static const CHANGE_WEATHER:int = 2;
    public static const WIND_SPEED:int = 3;
    public static const PARTICLE_DIRECTION:int = 4;

    public var _type:int;
    public var _particles:int;
    public var _weather:int;
    public var _windSpeed:Vector.<Number>;
    public var _particleDirection:int; //<- = 0 ;; down = 1 ;; -> = 2
    public var _currentTime:int;
    public var _atmosphere:String;

    public function WeatherPropertiesPacket(_arg1:uint,_arg2:Function) {
        super(_arg1, _arg2);
        _windSpeed = new Vector.<Number>();
    }

    override public function parseFromInput(_arg1:IDataInput):void {
        _type = _arg1.readUnsignedByte();
        _atmosphere = _arg1.readUTF();
        _particles = _arg1.readShort();
        _weather = _arg1.readUnsignedByte();
        _windSpeed.push(_arg1.readFloat(), _arg1.readFloat());
        _particleDirection = _arg1.readByte();
        _currentTime = _arg1.readInt();
    }
}
}
