package com.company.assembleegameclient.map {
import com.company.assembleegameclient.objects.GameObject;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.util.RandomUtil;
import com.company.util.PointUtil;

import flash.display.StageScaleMode;

import flash.geom.Matrix3D;
import flash.geom.PerspectiveProjection;
import flash.geom.Point;
import flash.geom.Rectangle;
import flash.geom.Vector3D;

public class Camera {

    public static const lN_:Vector3D = new Vector3D(0, 0, 1);
    public static const CENTER_SCREEN_RECT:Rectangle = new Rectangle(-300, -325, 600, 600);
    public static const OFFSET_SCREEN_RECT:Rectangle = new Rectangle(-300, -450, 600, 600);
    public static var MapRectFSCentered:Rectangle = new Rectangle(-300,-325,600,600);
    public static var MapRectFSNonCentered:Rectangle = new Rectangle(-300,-450,600,600);

    private const MAX_JITTER:Number = 0.5;
    private const JITTER_BUILDUP_MS:int = 10000;

    public var x_:Number;
    public var y_:Number;
    public var z_:Number;
    public var angleRad_:Number;
    public var clipRect_:Rectangle;
    public var pp_:PerspectiveProjection = new PerspectiveProjection();
    public var maxDist_:Number;
    public var maxDistSq_:Number;
    public var isHallucinating_:Boolean = false;
    public var wToS_:Matrix3D = new Matrix3D();
    public var wToV_:Matrix3D = new Matrix3D();
    public var vToS_:Matrix3D = new Matrix3D();
    private var nonPPMatrix_:Matrix3D = new Matrix3D();
    private var p_:Vector3D = new Vector3D();
    private var f_:Vector3D = new Vector3D();
    private var u_:Vector3D = new Vector3D();
    private var r_:Vector3D = new Vector3D();
    private var isJittering_:Boolean = false;
    private var jitter_:Number = 0;
    private var rd_:Vector.<Number> = new Vector.<Number>(16, true);

    public function Camera()
    {
        super();
        this.pp_.focalLength = 3;
        this.pp_.fieldOfView = 48;
       // this.ppMatrix_ = this.pp_.toMatrix3D();
        this.nonPPMatrix_.appendScale(50,50,50);
        this.f_.x = 0;
        this.f_.y = 0;
        this.f_.z = -1;
    }

    public static function resetDimensions() : void
    {
        var _loc1_:Number = Parameters.data_.mscale;
        var _loc2_:Number = WebMain.sWidth / _loc1_;
        var _loc3_:Number = WebMain.sHeight / _loc1_;
        var _loc4_:Number = Number(_loc3_ / 3);
        MapRectFSCentered = new Rectangle((_loc4_ - _loc2_) / 2,-_loc3_ * 13 / 24,_loc2_,_loc3_);
        MapRectFSNonCentered = new Rectangle((_loc4_ - _loc2_) / 2,-_loc3_ * 3 / 4,_loc2_,_loc3_);
    }
    public static function correctViewingArea(param1:Boolean) : Rectangle
    {
        var _loc2_:Number = NaN;
        var _loc3_:Number = NaN;
        var _loc4_:Number = NaN;
        var _loc5_:Number = NaN;
        if(Parameters.data_.stageScale == StageScaleMode.NO_SCALE)
        {
            _loc2_ = Parameters.data_.mscale;
            _loc3_ = WebMain.sWidth / _loc2_;
            _loc4_ = WebMain.sHeight / _loc2_;
            _loc5_ = Number(200 * WebMain.sHeight / 600 / _loc2_);
            if(param1)
            {
                return new Rectangle(-((_loc3_ - _loc5_) / 2),-(_loc4_ * 13 / 24),_loc3_,_loc4_);
            }
            return new Rectangle(-((_loc3_ - _loc5_) / 2),-(_loc4_ * 3 / 4),_loc3_,_loc4_);
        }
        if(param1)
        {
            return MapRectFSCentered;
        }
        return MapRectFSNonCentered;
    }

    public function configureCamera(_arg1:GameObject, _arg2:Boolean):void {
        var scale:Number = Parameters.data_["mscale"];
        var width:Number = WebMain.sWidth / scale;
        var height:Number = WebMain.sHeight / scale;
        var camera:Rectangle = Parameters.data_.centerOnPlayer ? MapRectFSCentered = new Rectangle((height / 3 - width) / 2,-height * 13 / 24,width,height) : MapRectFSNonCentered = new Rectangle((height / 3 - width) / 2,-height * 3 / 4,width,height);
        var cameraAngle:Number = Parameters.data_.cameraAngle;
        this.configure(_arg1.x_, _arg1.y_, 12, cameraAngle, camera);
        this.isHallucinating_ = _arg2;
    }

    public function configureCameraXYZ(number:Number, number2:Number, number3:Number, point:Point, hallucinating:Boolean) : void
    {
        var distance:Number = PointUtil.distanceXY(number, number2, point.x, point.y) * Parameters.data_.mouseCameraMultiplier;
        var atan2:Number = Math.atan2(point.y - number2,point.x - number);
        var cps:Number = Math.cos(atan2) * distance + number;
        var sin:Number = Math.sin(atan2) * distance + number2;
        var scale:Number = Parameters.data_["mscale"];
        var width:Number = WebMain.sWidth / scale;
        var height:Number = WebMain.sHeight / scale;
        this.configure(cps,sin,number3,Parameters.data_.cameraAngle, Parameters.data_.centerOnPlayer ? MapRectFSCentered = new Rectangle((height / 3 - width) / 2,-height * 13 / 24,width,height) : MapRectFSNonCentered = new Rectangle((height / 3 - width) / 2,-height * 3 / 4,width,height));
        //this.configure(cps,sin,number3,Parameters.data_.cameraAngle,Parameters.data_.centerOnPlayer ? MapRectFSCentered : MapRectFSNonCentered);
        this.isHallucinating_ = hallucinating;
    }

    public function startJitter():void {
        this.isJittering_ = true;
        this.jitter_ = 0;
    }

    public function update(_arg1:Number):void {
        if (((this.isJittering_) && ((this.jitter_ < this.MAX_JITTER)))) {
            this.jitter_ = (this.jitter_ + ((_arg1 * this.MAX_JITTER) / this.JITTER_BUILDUP_MS));
            if (this.jitter_ > this.MAX_JITTER) {
                this.jitter_ = this.MAX_JITTER;
            }
        }
    }

    public function configure(_arg1:Number, _arg2:Number, _arg3:Number, _arg4:Number, _arg5:Rectangle):void {
        if (this.isJittering_) {
            _arg1 = (_arg1 + RandomUtil.plusMinus(this.jitter_));
            _arg2 = (_arg2 + RandomUtil.plusMinus(this.jitter_));
        }
        this.x_ = _arg1;
        this.y_ = _arg2;
        this.z_ = _arg3;
        this.angleRad_ = _arg4;
        this.clipRect_ = _arg5;
        this.p_.x = _arg1;
        this.p_.y = _arg2;
        this.p_.z = _arg3;
        this.r_.x = Math.cos(this.angleRad_);
        this.r_.y = Math.sin(this.angleRad_);
        this.r_.z = 0;
        this.u_.x = Math.cos((this.angleRad_ + (Math.PI / 2)));
        this.u_.y = Math.sin((this.angleRad_ + (Math.PI / 2)));
        this.u_.z = 0;
        this.rd_[0] = this.r_.x;
        this.rd_[1] = this.u_.x;
        this.rd_[2] = this.f_.x;
        this.rd_[3] = 0;
        this.rd_[4] = this.r_.y;
        this.rd_[5] = this.u_.y;
        this.rd_[6] = this.f_.y;
        this.rd_[7] = 0;
        this.rd_[8] = this.r_.z;
        this.rd_[9] = -1;
        this.rd_[10] = this.f_.z;
        this.rd_[11] = 0;
        this.rd_[12] = -(this.p_.dotProduct(this.r_));
        this.rd_[13] = -(this.p_.dotProduct(this.u_));
        this.rd_[14] = -(this.p_.dotProduct(this.f_));
        this.rd_[15] = 1;
        this.wToV_.rawData = this.rd_;
        this.vToS_ = this.nonPPMatrix_;
        this.wToS_.identity();
        this.wToS_.append(this.wToV_);
        this.wToS_.append(this.vToS_);
        //var _local6:Number = (1200 / 100);
        //var _local7:Number = (1200 / 100);
        var _local6:Number = (this.clipRect_.width / 100);
        var _local7:Number = (this.clipRect_.height / 100);
        this.maxDist_ = (Math.sqrt(((_local6 * _local6) + (_local7 * _local7))) + 1);
        this.maxDistSq_ = (this.maxDist_ * this.maxDist_);
    }


}
}

