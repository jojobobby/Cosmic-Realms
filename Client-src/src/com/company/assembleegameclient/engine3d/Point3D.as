package com.company.assembleegameclient.engine3d {
import com.company.assembleegameclient.map.Camera;
import com.company.util.Trig;

import flash.display.BitmapData;
import com.company.assembleegameclient.parameters.Parameters;
import flash.display.GraphicsBitmapFill;
import flash.display.GraphicsEndFill;
import flash.display.GraphicsPath;
import flash.display.GraphicsPathCommand;
import flash.display.GraphicsSolidFill;
import flash.display.IGraphicsData;
import flash.geom.Matrix;
import flash.geom.Matrix3D;
import flash.geom.Vector3D;

public class Point3D {

    private static const commands_:Vector.<int> = new <int>[GraphicsPathCommand.MOVE_TO, GraphicsPathCommand.LINE_TO, GraphicsPathCommand.LINE_TO, GraphicsPathCommand.LINE_TO];
    private static const END_FILL:GraphicsEndFill = new GraphicsEndFill();

    private const data_:Vector.<Number> = new Vector.<Number>();
    private const path_:GraphicsPath = new GraphicsPath(commands_, data_);
    private const bitmapFill_:GraphicsBitmapFill = new GraphicsBitmapFill(null, new Matrix(), false, false);
    private const solidFill_:GraphicsSolidFill = new GraphicsSolidFill(0, 1);

    public var size_:Number;
    public var posS_:Vector3D;

    private var n:Vector.<Number>;

    public function Point3D(_arg1:Number) {
        this.size_ = _arg1;
        this.n = new Vector.<Number>(16, true);
        this.posS_ = new Vector3D();
    }

    public function setSize(_arg1:Number):void {
        this.size_ = _arg1;
    }

    public function draw(param1:Vector.<IGraphicsData>, param2:Vector3D, param3:Number, param4:Matrix3D, param5:Camera, param6:BitmapData, param7:uint = 0) : void
    {
        var _loc8_:Number = NaN;
        var _loc9_:Number = NaN;
        var _loc10_:Matrix = null;
        this.projectVector2posS(param4,param2);
        if(this.posS_.w < 0)
        {
            return;
        }
        var _loc11_:Number = this.posS_.w * Math.sin(param5.pp_.fieldOfView / 2 * Trig.toRadians);
        var _loc12_:Number = this.size_ / _loc11_;
        this.data_.length = 0;
        if(param3 == 0)
        {
            this.data_.push(this.posS_.x - _loc12_,this.posS_.y - _loc12_,this.posS_.x + _loc12_,this.posS_.y - _loc12_,this.posS_.x + _loc12_,this.posS_.y + _loc12_,this.posS_.x - _loc12_,this.posS_.y + _loc12_);
        }
        else
        {
            _loc8_ = Math.cos(param3);
            _loc9_ = Math.sin(param3);
            this.data_.push(this.posS_.x + (_loc8_ * -_loc12_ + _loc9_ * -_loc12_),this.posS_.y + (_loc9_ * -_loc12_ - _loc8_ * -_loc12_),this.posS_.x + (_loc8_ * _loc12_ + _loc9_ * -_loc12_),this.posS_.y + (_loc9_ * _loc12_ - _loc8_ * -_loc12_),this.posS_.x + (_loc8_ * _loc12_ + _loc9_ * _loc12_),this.posS_.y + (_loc9_ * _loc12_ - _loc8_ * _loc12_),this.posS_.x + (_loc8_ * -_loc12_ + _loc9_ * _loc12_),this.posS_.y + (_loc9_ * -_loc12_ - _loc8_ * _loc12_));
        }
        if(param6 != null)
        {
            this.bitmapFill_.bitmapData = param6;
            _loc10_ = this.bitmapFill_.matrix;
            _loc10_.identity();
            if(!Parameters.data_.projOutline)
            {
                _loc10_.scale(2 * _loc12_ / param6.width,2 * _loc12_ / param6.height);
                _loc10_.translate(-_loc12_,-_loc12_);
            }
            else
            {
                _loc10_.translate(-(param6.width / 2),-(param6.height / 2));
            }
            _loc10_.rotate(param3);
            _loc10_.translate(this.posS_.x,this.posS_.y);
            param1.push(this.bitmapFill_);
        }
        else
        {
            this.solidFill_.color = param7;
            param1.push(this.solidFill_);
        }
        param1.push(this.path_);
        param1.push(END_FILL);
    }



    private function projectVector2posS(param1:Matrix3D, param2:Vector3D) : void
    {
        param1.copyRawDataTo(this.n);
        this.posS_.x = param2.x * this.n[0] + param2.y * this.n[4] + param2.z * this.n[8] + this.n[12];
        this.posS_.y = param2.x * this.n[1] + param2.y * this.n[5] + param2.z * this.n[9] + this.n[13];
        this.posS_.z = param2.x * this.n[2] + param2.y * this.n[6] + param2.z * this.n[10] + this.n[14];
        this.posS_.w = param2.x * this.n[3] + param2.y * this.n[7] + param2.z * this.n[11] + this.n[15];
        this.posS_.project();
    }
}




}
