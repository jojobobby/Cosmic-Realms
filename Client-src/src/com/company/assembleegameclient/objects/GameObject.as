package com.company.assembleegameclient.objects
{
    import com.company.assembleegameclient.engine3d.Model3D;
    import com.company.assembleegameclient.engine3d.Object3D;
import kabam.rotmg.messaging.impl.incoming.Damage;
    import com.company.assembleegameclient.map.Camera;
    import com.company.assembleegameclient.map.Map;
    import com.company.assembleegameclient.map.Square;
    import com.company.assembleegameclient.map.mapoverlay.CharacterStatusText;
    import com.company.assembleegameclient.objects.animation.Animations;
    import com.company.assembleegameclient.objects.animation.AnimationsData;
    import com.company.assembleegameclient.objects.particles.ExplosionEffect;
    import com.company.assembleegameclient.objects.particles.HitEffect;
    import com.company.assembleegameclient.objects.particles.ParticleEffect;
    import com.company.assembleegameclient.objects.particles.ShockerEffect;
    import com.company.assembleegameclient.objects.particles.SpritesProjectEffect;
    import com.company.assembleegameclient.parameters.Parameters;
    import com.company.assembleegameclient.sound.SoundEffectLibrary;
    import com.company.assembleegameclient.util.AnimatedChar;
    import com.company.assembleegameclient.util.BloodComposition;
    import com.company.assembleegameclient.util.ConditionEffect;
    import com.company.assembleegameclient.util.MaskedImage;
    import com.company.assembleegameclient.util.TextureRedrawer;
    import com.company.assembleegameclient.util.redrawers.GlowRedrawer;
    import com.company.util.AssetLibrary;
    import com.company.util.BitmapUtil;
    import com.company.util.CachingColorTransformer;
    import com.company.util.ConversionUtil;
    import com.company.util.GraphicsUtil;
    import com.company.util.MoreColorUtil;
    import flash.display.BitmapData;
    import flash.display.GradientType;
    import flash.display.GraphicsBitmapFill;
    import flash.display.GraphicsGradientFill;
    import flash.display.GraphicsPath;
    import flash.display.GraphicsSolidFill;
    import flash.display.IGraphicsData;
    import flash.filters.ColorMatrixFilter;
    import flash.geom.ColorTransform;
    import flash.geom.Matrix;
    import flash.geom.Point;
    import flash.geom.Vector3D;
    import flash.utils.Dictionary;
    import flash.utils.getQualifiedClassName;
    import flash.utils.getTimer;
    import kabam.rotmg.core.StaticInjectorContext;
    import kabam.rotmg.messaging.impl.data.WorldPosData;
    import kabam.rotmg.messaging.impl.incoming.Damage;
    import kabam.rotmg.pets.data.PetVO;
    import kabam.rotmg.pets.data.PetsModel;
    import kabam.rotmg.stage3D.GraphicsFillExtra;
    import kabam.rotmg.stage3D.Object3D.Object3DStage3D;
    import kabam.rotmg.text.model.TextKey;
    import kabam.rotmg.text.view.BitmapTextFactory;
    import kabam.rotmg.text.view.stringBuilder.LineBuilder;
    import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;
    import kabam.rotmg.text.view.stringBuilder.StringBuilder;

    public class GameObject extends BasicObject
    {
        protected static const PAUSED_FILTER:ColorMatrixFilter = new ColorMatrixFilter(MoreColorUtil.greyscaleFilterMatrix);
        protected static const CURSED_FILTER:ColorMatrixFilter = new ColorMatrixFilter(MoreColorUtil.redFilterMatrix);
        protected static const SHOCKED_FILTER:ColorMatrixFilter = new ColorMatrixFilter(MoreColorUtil.greyscaleFilterMatrix);
        protected static const IDENTITY_MATRIX:Matrix = new Matrix();
        private static const ZERO_LIMIT:Number = 1E-5;
        private static const NEGATIVE_ZERO_LIMIT:Number = -(ZERO_LIMIT);
        public static const ATTACK_PERIOD:int = 300;
        private var objectXML:XML = null;
        public var nameBitmapData_:BitmapData = null;
        private var nameFill_:GraphicsBitmapFill = null;
        private var namePath_:GraphicsPath = null;
        public var shockEffect:ShockerEffect;
        private var isShocked:Boolean;
        private var isShockedTransformSet:Boolean = false;
        public var texture:BitmapData = null;
        private var isCharging:Boolean;
        private var isChargingTransformSet:Boolean = false;
        public var props_:ObjectProperties;
        public var name_:String;
        public var radius_:Number = 0.5;
        public var facing_:Number = 0;
        public var flying_:Boolean = false;
        public var attackAngle_:Number = 0;
        public var attackStart_:int = 0;
        public var animatedChar_:AnimatedChar = null;
        public var texture_:BitmapData = null;
        public var mask_:BitmapData = null;
        public var randomTextureData_:Vector.<TextureData> = null;
        public var obj3D_:Object3D = null;
        public var object3d_:Object3DStage3D = null;
        public var effect_:ParticleEffect = null;
        public var animations_:Animations = null;
        public var dead_:Boolean = false;
        protected var portrait_:BitmapData = null;
        protected var texturingCache_:Dictionary = null;
        public var maxHP_:int = 200;
        public var hp_:int = 200;
        public var size_:int = 100;
        public var rarity_:int = 0;
        public var level_:int = -1;
        public var defense_:int = 0;
        public var slotTypes_:Vector.<int> = null;
        public var equipment_:Vector.<int> = null;
        public var lockedSlot:Vector.<int> = null;
        public var condition_:Vector.<uint>;
        protected var tex1Id_:int = 0;
        protected var tex2Id_:int = 0;
        public var isInteractive_:Boolean = false;
        public var objectType_:int;
        private var nextBulletId_:uint = 1;
        private var sizeMult_:Number = 1;
        public var sinkLevel_:int = 0;
        public var hallucinatingTexture_:BitmapData = null;
        public var flash_:FlashDescription = null;
        public var connectType_:int = -1;
        private var isStunImmune_:Boolean = false;
        private var isParalyzeImmune_:Boolean = false;
        private var isDazedImmune_:Boolean = false;
        private var ishpScaleSet:Boolean = false;
        protected var lastTickUpdateTime_:int = 0;
        protected var myLastTickId_:int = -1;
        protected var posAtTick_:Point;
        protected var tickPosition_:Point;
        protected var moveVec_:Vector3D;
        protected var bitmapFill_:GraphicsBitmapFill;
        protected var path_:GraphicsPath;
        protected var vS_:Vector.<Number>;
        protected var uvt_:Vector.<Number>;
        protected var fillMatrix_:Matrix;
        private var hpbarBackFill_:GraphicsSolidFill = null;
        private var hpbarBackPath_:GraphicsPath = null;
        private var hpbarFill_:GraphicsSolidFill = null;
        private var hpbarPath_:GraphicsPath = null;
        private var icons_:Vector.<BitmapData> = null;
        private var iconFills_:Vector.<GraphicsBitmapFill> = null;
        private var iconPaths_:Vector.<GraphicsPath> = null;
        protected var shadowGradientFill_:GraphicsGradientFill = null;
        protected var shadowPath_:GraphicsPath = null;
        protected var glowColor_:int = 0;
        public var DamageDealt:int;
        public var spritesProjectEffect:SpritesProjectEffect;

        public function GameObject(objectXML:XML)
        {
            this.props_ = ObjectLibrary.defaultProps_;
            this.condition_ = new <uint>[0, 0];
            this.posAtTick_ = new Point();
            this.tickPosition_ = new Point();
            this.moveVec_ = new Vector3D();
            this.bitmapFill_ = new GraphicsBitmapFill(null, null, false, false);
            this.path_ = new GraphicsPath(GraphicsUtil.QUAD_COMMANDS, null);
            this.vS_ = new Vector.<Number>();
            this.uvt_ = new Vector.<Number>();
            this.fillMatrix_ = new Matrix();
            super();
            if (objectXML == null)
            {
                return;
            }

            this.objectType_ = int(objectXML.@type);
            this.props_ = ObjectLibrary.propsLibrary_[this.objectType_];
            hasShadow_ = this.props_.shadowSize_ > 0;
            var textureData:TextureData = ObjectLibrary.typeToTextureData_[this.objectType_];
            this.texture_ = textureData.texture_;
            this.mask_ = textureData.mask_;
            this.animatedChar_ = textureData.animatedChar_;
            this.randomTextureData_ = textureData.randomTextureData_;
            if (textureData.effectProps_ != null)
            {
                this.effect_ = ParticleEffect.fromProps(textureData.effectProps_, this);
            }
            if (this.texture_ != null)
            {
                this.sizeMult_ = this.texture_.height / 8;
            }
            if ("Model" in objectXML)
            {
                this.obj3D_ = Model3D.getObject3D(String(objectXML.Model));
                this.object3d_ = Model3D.getStage3dObject3D(String(objectXML.Model));
                if (this.texture_ != null)
                {
                    this.object3d_.setBitMapData(this.texture_);
                }
            }
            var animationsData:AnimationsData = ObjectLibrary.typeToAnimationsData_[this.objectType_];
            if (animationsData != null)
            {
                this.animations_ = new Animations(animationsData);
            }
            z_ = this.props_.z_;
            this.flying_ = this.props_.flying_;
            if ("MaxHitPoints" in objectXML)
            {
                this.hp_ = this.maxHP_ = int(objectXML.MaxHitPoints);
            }
            if ("Defense" in objectXML)
            {
                this.defense_ = int(objectXML.Defense);
            }
            if ("SlotTypes" in objectXML)
            {
                this.slotTypes_ = ConversionUtil.toIntVector(objectXML.SlotTypes);
                this.equipment_ = new Vector.<int>(this.slotTypes_.length);
                for (var i:int = 0; i < this.equipment_.length; i++)
                {
                    this.equipment_[i] = -1;
                }
                this.lockedSlot = new Vector.<int>(this.slotTypes_.length);
            }
            if ("Tex1" in objectXML)
            {
                this.tex1Id_ = int(objectXML.Tex1);
            }
            if ("Tex2" in objectXML)
            {
                this.tex2Id_ = int(objectXML.Tex2);
            }
            if ("StunImmune" in objectXML)
            {
                this.isStunImmune_ = true;
            }
            if ("ParalyzeImmune" in objectXML)
            {
                this.isParalyzeImmune_ = true;
            }
            if ("DazedImmune" in objectXML)
            {
                this.isDazedImmune_ = true;
            }
            this.props_.loadSounds();
            this.setRarity(0);
        }

        public static function damageWithDefense(origDamage:int, targetDefense:int, armorPiercing:Boolean, targetConditions:Vector.<uint>):int
        {
            var def:int = targetDefense;
            if (armorPiercing || (targetConditions[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.ARMORBROKEN_BIT) != 0)
            {
                def = 0;
            }
            else if ((targetConditions[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.ARMORED_BIT) != 0)
            {
                def *= 1.25;
            }

            if ((targetConditions[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.SOLID_BIT) != 0)
            {
                def *= 2;
            }
            if ((targetConditions[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.BARRIER_BIT) != 0)
            {
                def *= 1.5;
            }
            var min:int = origDamage * 3 / 20;
            var d:int = Math.max(min, origDamage - def);
            if ((targetConditions[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.INVULNERABLE_BIT) != 0)
            {
                d = 0;
            }
            if ((targetConditions[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.PETRIFIED_BIT) != 0)
            {
                d *= 0.9;
            }

            if ((targetConditions[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.CURSE_BIT) != 0)
            {
                d *= 1.2;
            }
            return d;
        }


        public function setObjectId(objectId:int):void
        {
            objectId_ = objectId;
            if (this.randomTextureData_ != null)
            {
                var textureData:TextureData = this.randomTextureData_[(objectId_ % this.randomTextureData_.length)];
                this.texture_ = textureData.texture_;
                this.mask_ = textureData.mask_;
                this.animatedChar_ = textureData.animatedChar_;
                if (this.object3d_ != null)
                {
                    this.object3d_.setBitMapData(this.texture_);
                }
            }
        }

        public function setAltTexture(altTextureId:int):void
        {
            var altTextureData:TextureData;
            var textureData:TextureData = ObjectLibrary.typeToTextureData_[this.objectType_];
            if (altTextureId == 0)
            {
                altTextureData = textureData;
            }
            else
            {
                altTextureData = textureData.getAltTextureData(altTextureId);
                if (altTextureData == null)
                {
                    return;
                }
            }
            this.texture_ = altTextureData.texture_;
            this.mask_ = altTextureData.mask_;
            this.animatedChar_ = altTextureData.animatedChar_;
            if (this.effect_ != null)
            {
                map_.removeObj(this.effect_.objectId_);
                this.effect_ = null;
            }
            if (altTextureData.effectProps_ != null)
            {
                this.effect_ = ParticleEffect.fromProps(altTextureData.effectProps_, this);
                if (map_ != null)
                {
                    map_.addObj(this.effect_, x_, y_);
                }
            }
        }

        public function setTex1(tex1Id:int):void
        {
            if (tex1Id == this.tex1Id_)
            {
                return;
            }
            this.tex1Id_ = tex1Id;
            this.clearCache();
        }

        public function setTex2(tex2Id:int):void
        {
            if (tex2Id == this.tex2Id_)
            {
                return;
            }
            this.tex2Id_ = tex2Id;
            this.clearCache();
        }

        public function setSize(size:int):void
        {
            this.size_ = size;
            if (this is Player)
            {
                this.clearCache();
            }
        }

        public function setRarity(rarity:int):void {
            if (this.rarity_ != rarity) {
                this.rarity_ = rarity;
                switch(this.rarity_) {
                    case 0: setGlow(0); break;
                    case 1: setGlow(0x919191); break;//gray
                    case 2: setGlow(0x3B5EAF);break;// blue
                    case 3: setGlow(0x6D2BD8);break;//purple
                    case 4: setGlow(0xD6A000);break; //legendary
                }
            }
        }

        public function setGlow(glow:int):void
        {
            if (this.glowColor_ == glow)
            {
                return;
            }
            this.glowColor_ = glow;
            this.clearCache();
        }

        public function clearCache():void
        {
            this.texturingCache_ = new Dictionary();
            this.portrait_ = null;
        }

        public function playSound(id:int):void
        {
            SoundEffectLibrary.play(this.props_.sounds_[id]);
        }

        override public function dispose():void
        {
            var obj:Object;
            var bitmapData:BitmapData;
            var dict:Dictionary;
            var obj2:Object;
            var bitmapData2:BitmapData;
            super.dispose();
            this.texture_ = null;
            if (this.portrait_ != null)
            {
                this.portrait_.dispose();
                this.portrait_ = null;
            }
            if (this.texturingCache_ != null)
            {
                for each (obj in this.texturingCache_)
                {
                    bitmapData = obj as BitmapData;
                    if (bitmapData != null) {
                        bitmapData.dispose();
                    }
                    else
                    {
                        dict = obj as Dictionary;
                        for each (obj2 in dict)
                        {
                            bitmapData2 = obj2 as BitmapData;
                            if (bitmapData2 != null)
                            {
                                bitmapData2.dispose();
                            }
                        }
                    }
                }
                this.texturingCache_ = null;
            }
            if (this.obj3D_ != null)
            {
                this.obj3D_.dispose();
                this.obj3D_ = null;
            }
            if (this.object3d_ != null)
            {
                this.object3d_.dispose();
                this.object3d_ = null;
            }
            this.slotTypes_ = null;
            this.equipment_ = null;
            this.lockedSlot = null;
            if (this.nameBitmapData_ != null)
            {
                this.nameBitmapData_.dispose();
                this.nameBitmapData_ = null;
            }
            this.nameFill_ = null;
            this.namePath_ = null;
            this.bitmapFill_ = null;
            this.path_.commands = null;
            this.path_.data = null;
            this.vS_ = null;
            this.uvt_ = null;
            this.fillMatrix_ = null;
            this.icons_ = null;
            this.iconFills_ = null;
            this.iconPaths_ = null;
            this.shadowGradientFill_ = null;
            if (this.shadowPath_ != null)
            {
                this.shadowPath_.commands = null;
                this.shadowPath_.data = null;
                this.shadowPath_ = null;
            }
        }

        public function isQuiet():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.QUIET_BIT) != 0;
        }

        public function isWeak():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.WEAK_BIT) != 0;
        }

        public function isSlowed():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.SLOWED_BIT) != 0;
        }

        public function isSick():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.SICK_BIT) != 0;
        }

        public function isDazed():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.DAZED_BIT) != 0;
        }

        public function isStunned():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.STUNNED_BIT) != 0;
        }

        public function isBlind():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.BLIND_BIT) != 0;
        }

        public function isDrunk():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.DRUNK_BIT) != 0;
        }

        public function isConfused():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.CONFUSED_BIT) != 0;
        }

        public function isStunImmune():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.STUN_IMMUNE_BIT) != 0 || this.isStunImmune_;
        }

        public function isInvisible():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.INVISIBLE_BIT) != 0;
        }

        public function isParalyzed():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.PARALYZED_BIT) != 0;
        }

        public function isSpeedy():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.SPEEDY_BIT) != 0;
        }

        public function isNinjaSpeedy():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.NINJA_SPEEDY_BIT) != 0;
        }

        public function isHallucinating():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.HALLUCINATING_BIT) != 0;
        }

        public function isHealing():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.HEALING_BIT) != 0;
        }

        public function isBerserk():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.BERSERK_BIT) != 0;
        }

        public function isDamaging():Boolean
           {
                return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.DAMAGING_BIT) != 0;
        }

        public function isPaused():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.PAUSED_BIT) != 0;
        }

        public function isStasis():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.STASIS_BIT) != 0;
        }

        public function isInvincible():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.INVINCIBLE_BIT) != 0;
        }

        public function isInvulnerable():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.INVULNERABLE_BIT) != 0;
        }

        public function isArmored():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.ARMORED_BIT) != 0;
        }

        public function isArmorBroken():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.ARMORBROKEN_BIT) != 0;
        }

        public function isArmorBrokenImmune():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.ARMORBROKEN_IMMUNE_BIT) != 0;
        }

        public function isSlowedImmune():Boolean
        {
            return (this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.SLOWED_IMMUNE_BIT) != 0;
        }

        public function isUnstable():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.UNSTABLE_BIT) != 0;
        }

        public function isShowPetEffectIcon():Boolean
        {
            return (this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.PET_EFFECT_ICON) != 0;
        }

        public function isDarkness():Boolean
        {
            return (this.condition_[ConditionEffect.CE_FIRST_BATCH] & ConditionEffect.DARKNESS_BIT) != 0;
        }

        public function isParalyzeImmune():Boolean
        {
            return this.isParalyzeImmune_ || (this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.PARALYZED_IMMUNE_BIT) != 0;
        }

        public function isDazedImmune():Boolean
        {
            return this.isDazedImmune_ || (this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.DAZED_IMMUNE_BIT) != 0;
        }

        public function isPetrified():Boolean
        {
            return (this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.PETRIFIED_BIT) != 0;
        }

        public function isPetrifiedImmune():Boolean
        {
            return (this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.PETRIFIED_IMMUNE_BIT) != 0;
        }

        public function isCursed():Boolean
        {
            return (this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.CURSE_BIT) != 0;
        }

        public function isCursedImmune():Boolean
        {
            return (this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.CURSE_IMMUNE_BIT) != 0;
        }

        public function isHidden() : Boolean
        {
            return (this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.HIDDEN_BIT) != 0;
        }


        public function isHaste():Boolean
        {
            return (this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.HASTE_BIT) != 0;
        }

        public function isDiminished():Boolean
        {
            return (this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.DIMINISHED_BIT) != 0;
        }

        public function isSwift():Boolean
        {
            return (this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.SWIFT_BIT) != 0;
        }
        public function isTired():Boolean
        {
            return (this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.TIRED_BIT) != 0;
        }
        public function isStrength():Boolean
        {
            return (this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.STRENGTH_BIT) != 0;
        }
        public function isBloodlust():Boolean
        {
            return (this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.BLOODLUST_BIT) != 0;
        }
        public function isSluggish():Boolean
        {
            return (this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.SLUGGISH_BIT) != 0;
        }
        public function isAwoken():Boolean
        {
            return (this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.AWOKEN_BIT) != 0;
        }
        public function isFrozen():Boolean
        {
            return (this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.FROZEN_BIT) != 0;
        }
        public function isSolid():Boolean
        {
            return (this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.SOLID_BIT) != 0;
        }
        public function isBarrier():Boolean
        {
            return (this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.BARRIER_BIT) != 0;
        }
        public function isEnchanted():Boolean
        {
            return (this.condition_[ConditionEffect.CE_SECOND_BATCH] & ConditionEffect.ENCHANTED_BIT) != 0;
        }



        public function getName():String
        {
            return this.name_ == null || this.name_ == "" ? ObjectLibrary.typeToDisplayId_[this.objectType_] : this.name_;
        }

        public function getColor():uint
        {
            return BitmapUtil.mostCommonColor(this.texture_);
        }

        public function getBulletId():uint
        {
            var ret:uint = this.nextBulletId_;
            this.nextBulletId_ = (this.nextBulletId_ + 1) % 128;
            return ret;
        }

        public function distTo(pos:WorldPosData):Number
        {
            var dx:Number = pos.x_ - x_;
            var dy:Number = pos.y_ - y_;
            return Math.sqrt((dx * dx) + (dy * dy));
        }

        public function distTo2():Number
        {
            var dx:Number = this.tickPosition_.x - x_;
            var dy:Number = this.tickPosition_.y - y_;
            return Math.sqrt((dx * dx) + (dy * dy));
        }

        public function toggleShockEffect(shock:Boolean):void {
            if (shock)
            {
                this.isShocked = true;
            }
            else
            {
                this.isShocked = false;
                this.isShockedTransformSet = false;
            }
        }

        public function toggleChargingEffect(charging:Boolean):void
        {
            if (charging)
            {
                this.isCharging = true;
            }
            else
            {
                this.isCharging = false;
                this.isChargingTransformSet = false;
            }
        }

        override public function addTo(map:Map, x:Number, y:Number):Boolean
        {
            map_ = map;
            this.posAtTick_.x = this.tickPosition_.x = x;
            this.posAtTick_.y = this.tickPosition_.y = y;
            if (!this.moveTo(x, y))
            {
                map_ = null;
                return false;
            }
            if (this.effect_ != null)
            {
                map_.addObj(this.effect_, x, y);
            }
            return true;
        }

        override public function removeFromMap():void
        {
            if (this.props_.static_ && square_ != null)
            {
                if (square_.obj_ == this)
                {
                    square_.obj_ = null;
                }
                square_ = null;
            }
            if (this.effect_ != null)
            {
                map_.removeObj(this.effect_.objectId_);
            }
            super.removeFromMap();
            this.dispose();
        }

        public function moveTo(x:Number, y:Number):Boolean
        {
            var square:Square = map_.getSquare(x, y);
            if (square == null) {
                return false;
            }
            x_ = x;
            y_ = y;
            if (this.props_.static_)
            {
                if (square_ != null)
                {
                    square_.obj_ = null;
                }
                square.obj_ = this;
            }
            square_ = square;
            if (this.obj3D_ != null)
            {
                this.obj3D_.setPosition(x_, y_, 0, this.props_.rotation_);
            }
            if (this.object3d_ != null)
            {
                this.object3d_.setPosition(x_, y_, 0, this.props_.rotation_);
            }
            return true;
        }

        public static function interpolate(number:Number, number2:Number, f:Number):Number
        {
            var result:Number = f * number + (1 - f) * number2;
            return result;
        }

        private var teleporting:Boolean = false;

        override public function update(time:int, dt:int):Boolean
        {
            var tickDT:Number;
            var pX:Number;
            var pY:Number;
            var moving:Boolean = false;

            if (distTo2() > 0.05 && !this.teleporting)
            {
                tickDT = dt * 0.0020666;
                pX = interpolate(this.tickPosition_.x, this.x_, tickDT);
                pY = interpolate(this.tickPosition_.y, this.y_, tickDT);
                this.moveTo(pX, pY);
                moving = true;
            }
            else
            {
                moveVec_.x = 0;
                moveVec_.y = 0;
            }
            if (this.props_.whileMoving_ != null)
            {
                if (!moving)
                {
                    z_ = this.props_.z_;
                    this.flying_ = this.props_.flying_;
                }
                else
                {
                    z_ = this.props_.whileMoving_.z_;
                    this.flying_ = this.props_.whileMoving_.flying_;
                }
            }
            return true;
        }

        public function onGoto(x:Number, y:Number, time:int):void
        {
            this.teleporting = true;
            this.moveTo(x, y);
            this.lastTickUpdateTime_ = time;
            this.tickPosition_.x = x;
            this.tickPosition_.y = y;
            this.posAtTick_.x = x;
            this.posAtTick_.y = y;
            this.moveVec_.x = 0;
            this.moveVec_.y = 0;
            this.teleporting = false;
        }

        public function onTickPos(x:Number, y:Number, tickTime:int, tickId:int):void
        {
            this.lastTickUpdateTime_ = map_.gs_.lastUpdate_;
            this.tickPosition_.x = x;
            this.tickPosition_.y = y;
            this.posAtTick_.x = x_;
            this.posAtTick_.y = y_;
            this.moveVec_.x = (this.tickPosition_.x - this.posAtTick_.x) / 237;
            this.moveVec_.y = (this.tickPosition_.y - this.posAtTick_.y) / 237;
            this.myLastTickId_ = tickId;
        }



        public function damage(_arg1:int, _arg2:int, _arg3:Vector.<uint>, _arg4:Boolean, _arg5:Projectile):void {
            var _local7:int;
            var _local8:uint;
            var _local9:ConditionEffect;
            var _local10:CharacterStatusText;
            var _local11:PetsModel;
            var _local12:PetVO;
            var _local13:String;
            var _local14:Vector.<uint>;
            var _local15:Boolean;
            var _local6:Boolean;
            if (_arg4) {
                this.dead_ = true;
            }
            else {
                if (_arg3 != null) {
                    _local7 = 0;
                    for each (_local8 in _arg3) {
                        _local9 = null;
                        if (((((!((_arg5 == null))) && (_arg5.projProps_.isPetEffect_))) && (_arg5.projProps_.isPetEffect_[_local8]))) {
                            _local11 = StaticInjectorContext.getInjector().getInstance(PetsModel);
                            _local12 = _local11.getActivePet();
                            if (_local12 != null) {
                                _local9 = ConditionEffect.effects_[_local8];
                                this.showConditionEffectPet(_local7, _local9.name_);
                                _local7 = (_local7 + 500);
                            }
                        }
                        else {
                            switch (_local8) {
                                    // case ConditionEffect.NOTHING:
                                    //   break;
                                case ConditionEffect.QUIET:
                                case ConditionEffect.WEAK:
                                case ConditionEffect.SICK:
                                case ConditionEffect.BLIND:
                                case ConditionEffect.HALLUCINATING:
                                case ConditionEffect.DRUNK:
                                case ConditionEffect.CONFUSED:
                                case ConditionEffect.STUN_IMMUNE:
                                case ConditionEffect.INVISIBLE:
                                case ConditionEffect.SPEEDY:
                                case ConditionEffect.BLEEDING:
                                case ConditionEffect.STASIS:
                                case ConditionEffect.STASIS_IMMUNE:
                                case ConditionEffect.NINJA_SPEEDY:
                                case ConditionEffect.UNSTABLE:
                                case ConditionEffect.DARKNESS:
                                case ConditionEffect.PETRIFIED_IMMUNE:
                                    _local9 = ConditionEffect.effects_[_local8];
                                    break;
                                case ConditionEffect.SLOWED:
                                    if (this.isSlowedImmune()) {
                                        _local10 = new CharacterStatusText(this, 0xFF0000, 3000);
                                        _local10.setStringBuilder(new LineBuilder().setParams(TextKey.GAMEOBJECT_IMMUNE));
                                        map_.mapOverlay_.addStatusText(_local10);
                                    }
                                    else {
                                        _local9 = ConditionEffect.effects_[_local8];
                                    }
                                    break;
                                case ConditionEffect.ARMORBROKEN:
                                    if (this.isArmorBrokenImmune()) {
                                        _local10 = new CharacterStatusText(this, 0xFF0000, 3000);
                                        _local10.setStringBuilder(new LineBuilder().setParams(TextKey.GAMEOBJECT_IMMUNE));
                                        map_.mapOverlay_.addStatusText(_local10);
                                    }
                                    else {
                                        _local9 = ConditionEffect.effects_[_local8];
                                    }
                                    break;
                                case ConditionEffect.STUNNED:
                                    if (this.isStunImmune()) {
                                        _local10 = new CharacterStatusText(this, 0xFF0000, 3000);
                                        _local10.setStringBuilder(new LineBuilder().setParams(TextKey.GAMEOBJECT_IMMUNE));
                                        map_.mapOverlay_.addStatusText(_local10);
                                    }
                                    else {
                                        _local9 = ConditionEffect.effects_[_local8];
                                    }
                                    break;
                                case ConditionEffect.DAZED:
                                    if (this.isDazedImmune()) {
                                        _local10 = new CharacterStatusText(this, 0xFF0000, 3000);
                                        _local10.setStringBuilder(new LineBuilder().setParams(TextKey.GAMEOBJECT_IMMUNE));
                                        map_.mapOverlay_.addStatusText(_local10);
                                    }
                                    else {
                                        _local9 = ConditionEffect.effects_[_local8];
                                    }
                                    break;
                                case ConditionEffect.PARALYZED:
                                    if (this.isParalyzeImmune()) {
                                        _local10 = new CharacterStatusText(this, 0xFF0000, 3000);
                                        _local10.setStringBuilder(new LineBuilder().setParams(TextKey.GAMEOBJECT_IMMUNE));
                                        map_.mapOverlay_.addStatusText(_local10);
                                    }
                                    else {
                                        _local9 = ConditionEffect.effects_[_local8];
                                    }
                                    break;
                                case ConditionEffect.PETRIFIED:
                                    if (this.isPetrifiedImmune()) {
                                        _local10 = new CharacterStatusText(this, 0xFF0000, 3000);
                                        _local10.setStringBuilder(new LineBuilder().setParams(TextKey.GAMEOBJECT_IMMUNE));
                                        map_.mapOverlay_.addStatusText(_local10);
                                    }
                                    else {
                                        _local9 = ConditionEffect.effects_[_local8];
                                    }
                                    break;
                                case ConditionEffect.CURSE:
                                    if (this.isCursedImmune()) {
                                        _local10 = new CharacterStatusText(this, 0xFF0000, 3000);
                                        _local10.setStringBuilder(new LineBuilder().setParams(TextKey.GAMEOBJECT_IMMUNE));
                                        map_.mapOverlay_.addStatusText(_local10);
                                    }
                                    else {
                                        _local9 = ConditionEffect.effects_[_local8];
                                    }
                                    break;
                                case ConditionEffect.GROUND_DAMAGE:
                                    _local6 = true;
                                    break;
                            }
                            if (_local9 != null) {
                                if (_local8 < ConditionEffect.NEW_CON_THREASHOLD) {
                                    if ((this.condition_[ConditionEffect.CE_FIRST_BATCH] | _local9.bit_) == this.condition_[ConditionEffect.CE_FIRST_BATCH]) continue;
                                    this.condition_[ConditionEffect.CE_FIRST_BATCH] = (this.condition_[ConditionEffect.CE_FIRST_BATCH] | _local9.bit_);
                                }
                                else {
                                    if ((this.condition_[ConditionEffect.CE_SECOND_BATCH] | _local9.bit_) == this.condition_[ConditionEffect.CE_SECOND_BATCH]) continue;
                                    this.condition_[ConditionEffect.CE_SECOND_BATCH] = (this.condition_[ConditionEffect.CE_SECOND_BATCH] | _local9.bit_);
                                }
                                _local13 = _local9.localizationKey_;
                                this.showConditionEffect(_local7, _local13);
                                _local7 = (_local7 + 500);
                            }
                        }
                    }
                }
            }
            if (!((this.props_.isEnemy_) && (Parameters.data_.disableEnemyParticles))) {
                _local14 = BloodComposition.getBloodComposition(this.objectType_, this.texture_, this.props_.bloodProb_, this.props_.bloodColor_);
                if (this.dead_) {
                    map_.addObj(new ExplosionEffect(_local14, this.size_, 30), x_, y_);
                }
                else {
                    if (_arg5 != null) {
                        map_.addObj(new HitEffect(_local14, this.size_, 10, _arg5.angle_, _arg5.projProps_.speed_), x_, y_);
                    }
                    else {
                        map_.addObj(new ExplosionEffect(_local14, this.size_, 10), x_, y_);
                    }
                }
            }
            if(!_arg1
                    && (Parameters.data_.noEnemyDamage && this.props_.isEnemy_
                            || Parameters.data_.noAllyDamage && this.props_.isPlayer_)) {
                return;
            }
            if (_arg2 > 0) {
                _local15 = ((((this.isArmorBroken()) || (((!((_arg5 == null))) && (_arg5.projProps_.armorPiercing_))))) || (_local6));
                this.showDamageText(_arg2, _local15, _arg5);
            }
        }

        public function statusTextMod(param1:int, param2:Boolean, arg1:Projectile = null) : void
        {
            if (arg1 == null) {
                var _loc5_:int = this.hp_ - param1;
                _loc5_ = _loc5_ < 0?0:int(int(_loc5_));
                var _loc7_:String = "";

                if (this.props_.isEnemy_) {
                    _loc7_ = ("-" + param1 +" HP: "+ _loc5_ + " (x1.00)");
                } else {
                    _loc7_ = ("-" + param1 );
                }

                var _loc8_:CharacterStatusText = new CharacterStatusText(this, param2 ? 9437439 : 0xFFD700, 1000);

                _loc8_.setStringBuilder(new StaticStringBuilder(_loc7_));
                map_.mapOverlay_.addStatusText(_loc8_);
                return;
            }
            if (arg1 != null && arg1._isCrit > 1) {

                var CritMultiplied:Number = arg1._isCrit;
                var Amount = param1 * CritMultiplied;
                var CritValue = CritMultiplied.toFixed(2);
                var TrueAmount = Amount.toFixed(0);
                var _loc5_:int = this.hp_ - param1;
                if (this.props_.isEnemy_) {
                    var _loc7_:String = ("-" + TrueAmount + " HP: "+ _loc5_ + " (x" + CritValue + ")"); //remove all the other stuff for optimization
                } else {
                    var _loc7_:String = ("-" + param1 );
                }

                var _loc8_:CharacterStatusText = new CharacterStatusText(this, param2 ? 9437439 : 0xF89232, 1250);

                _loc8_.setStringBuilder(new StaticStringBuilder(_loc7_));
                map_.mapOverlay_.addStatusText(_loc8_);
                return;

            }else {
                var _loc50_:int = this.hp_ - param1;
                _loc50_ = _loc50_ < 0?0:int(int(_loc50_));
                var _loc70_:String = "";

                if (this.props_.isEnemy_) {
                    _loc70_ = ("-" + param1 +" HP: "+ _loc50_ + " (x1.00)");
                } else {
                    _loc70_ = ("-" + param1 );
                }
                var _loc80_:CharacterStatusText = new CharacterStatusText(this, param2 ? 9437439 : 0xFFD700, 1000);
                _loc80_.setStringBuilder(new StaticStringBuilder(_loc70_));
                map_.mapOverlay_.addStatusText(_loc80_);
                return;
            }

        }


        public function showDamageText(param1:int, param2:Boolean, arg1:Projectile = null) : void
        {
            this.statusTextMod(param1,param2, arg1);
        }

        public function showConditionEffect(time:int, name:String):void
        {
            var nameText:CharacterStatusText = new CharacterStatusText(this, 0xFF0000, 3000, time);
            nameText.setStringBuilder(new LineBuilder().setParams(name));
            map_.mapOverlay_.addStatusText(nameText);
        }

        public function showConditionEffectPet(time:int, name:String):void
        {
            var nameText:CharacterStatusText = new CharacterStatusText(this, 0xFF0000, 3000, time);
            nameText.setStringBuilder(new StaticStringBuilder(("Pet " + name)));
            map_.mapOverlay_.addStatusText(nameText);
        }


        protected function makeNameBitmapData():BitmapData
        {
            var stringBuilder:StringBuilder = new StaticStringBuilder(this.name_);
            var bitmapTextFactory:BitmapTextFactory = StaticInjectorContext.getInjector().getInstance(BitmapTextFactory);
            return bitmapTextFactory.make(stringBuilder, 16, 0xFFFFFF, true, IDENTITY_MATRIX, true);
        }

        public function drawName(graphicsData:Vector.<IGraphicsData>, camera:Camera):void
        {
            if (this.nameBitmapData_ == null)
            {
                this.nameBitmapData_ = this.makeNameBitmapData();
                this.nameFill_ = new GraphicsBitmapFill(null, new Matrix(), false, false);
                this.namePath_ = new GraphicsPath(GraphicsUtil.QUAD_COMMANDS, new Vector.<Number>());
            }
            var w:int = (this.nameBitmapData_.width / 2) + 1;
            var nameVSs:Vector.<Number> = this.namePath_.data;
            nameVSs.length = 0;
            nameVSs.push(posS_[0] - w, posS_[1], posS_[0] + w, posS_[1], posS_[0] + w, posS_[1] + 30, posS_[0] - w, posS_[1] + 30);
            this.nameFill_.bitmapData = this.nameBitmapData_;
            var m:Matrix = this.nameFill_.matrix;
            m.identity();
            m.translate(nameVSs[0], nameVSs[1]);
            graphicsData.push(this.nameFill_);
            graphicsData.push(this.namePath_);
            graphicsData.push(GraphicsUtil.END_FILL);
        }

        protected function getHallucinatingTexture():BitmapData
        {
            if (this.hallucinatingTexture_ == null)
            {
                this.hallucinatingTexture_ = AssetLibrary.getImageFromSet("lofiChar8x8", int(Math.random() * 239));
            }
            return this.hallucinatingTexture_;
        }

        protected function getTexture(camera:Camera, time:int):BitmapData
        {
            if (this is Pet) {
                var pet:Pet = Pet(this);
                if (this.condition_[ConditionEffect.CE_FIRST_BATCH] != 0 && !this.isPaused()) {
                    if (pet.skinId != 32912) {
                        pet.setSkin(32912);
                    }
                } else if (!pet.isDefaultAnimatedChar)
                {
                    pet.setDefaultSkin();
                }
            }

            var texture:BitmapData = this.texture_;
            var size:int = this.size_;
            var mask:BitmapData;
            if (this.animatedChar_ != null) {
                var p:Number = 0;
                var action:int = AnimatedChar.STAND;
                if (time < (this.attackStart_ + ATTACK_PERIOD))
                {
                    if (!this.props_.dontFaceAttacks_) {
                        this.facing_ = this.attackAngle_;
                    }
                    p = ((time - this.attackStart_) % ATTACK_PERIOD) / ATTACK_PERIOD;
                    action = AnimatedChar.ATTACK;
                } else if (this.moveVec_.x != 0 || this.moveVec_.y != 0)
                {
                    var walkPer:int = (0.5 / this.moveVec_.length);
                    walkPer = walkPer + (400 - (walkPer % 400));
                    if (this.moveVec_.x > ZERO_LIMIT || this.moveVec_.x < NEGATIVE_ZERO_LIMIT || this.moveVec_.y > ZERO_LIMIT || this.moveVec_.y < NEGATIVE_ZERO_LIMIT)
                    {
                        this.facing_ = Math.atan2(this.moveVec_.y, this.moveVec_.x);
                        action = AnimatedChar.WALK;
                    }
                    else
                    {
                        action = AnimatedChar.STAND;
                    }
                    p = (time % walkPer) / walkPer;
                }
                var image:MaskedImage = this.animatedChar_.imageFromFacing(this.facing_, camera, action, p);
                texture = image.image_;
                mask = image.mask_;
            } else if (this.animations_ != null) {
                var animTexture:BitmapData = this.animations_.getTexture(time);
                if (animTexture != null) {
                    texture = animTexture;
                }
            }

            if (Parameters.isGpuRender()) {
				if (this.props_.drawOnGround_) {
					return texture;
				}
            }
            else {
                if (this.props_.drawOnGround_ || this.obj3D_ != null) {
                    return texture;
                }
            }

            if (camera.isHallucinating_)
            {
                var w:int = texture == null ? 8 : texture.width;
                texture = this.getHallucinatingTexture();
                mask = null;
                size = this.size_ * Math.min(1.5, w / texture.width);
            }

            if ((this.isStasis() || this.isPetrified()) && !(this is Pet))
            {
                texture = CachingColorTransformer.filterBitmapData(texture, PAUSED_FILTER);
            }

            if (this.isCursed()) {
                var newTexture:BitmapData = null;
                if (this.texturingCache_ == null)
                {
                    this.texturingCache_ = new Dictionary();
                } else {
                    newTexture = this.texturingCache_[texture];
                }

                if (newTexture == null) {
                    newTexture = TextureRedrawer.resize(texture, mask, size, false, this.tex1Id_, this.tex2Id_);
                    newTexture = GlowRedrawer.outlineGlow(newTexture, 0xFF0000, 1.4, true, 3 ,true);
                    this.texturingCache_[texture] = newTexture;
                }

                texture = newTexture;
            }
            else {
                if (this.tex1Id_ == 0 && this.tex2Id_ == 0) {
                    texture = TextureRedrawer.redraw(texture, size, false, this.glowColor_);
                } else {
                    var newTexture:BitmapData = null;
                    if (this.texturingCache_ == null) {
                        this.texturingCache_ = new Dictionary();
                    } else {
                        this.texturingCache_[texture] = newTexture;
                    }

                    if (newTexture == null) {
                        newTexture = TextureRedrawer.resize(texture, mask, size, false, this.tex1Id_, this.tex2Id_);
                        newTexture = GlowRedrawer.outlineGlow(newTexture, this.glowColor_);
                        this.texturingCache_[texture] = newTexture;
                    }

                    texture = newTexture;
                }
            }

            if (this.isInvisible() && !(this is Player))
            {
                texture = CachingColorTransformer.alphaBitmapData(texture, 70);
            }

            return texture;
        }

        public function useAltTexture(file:String, index:int):void
        {
            this.texture_ = AssetLibrary.getImageFromSet(file, index);
            this.sizeMult_ = this.texture_.height / 8;
        }

        public function getPortrait():BitmapData
        {
            var portraitTexture:BitmapData;
            var size:int;
            if (this.portrait_ == null)
            {
                portraitTexture = this.props_.portrait_ != null ? this.props_.portrait_.getTexture() : this.texture_;
                size = (4 / portraitTexture.width) * 100;
                this.portrait_ = TextureRedrawer.resize(portraitTexture, this.mask_, size, true, this.tex1Id_, this.tex2Id_);
                this.portrait_ = GlowRedrawer.outlineGlow(this.portrait_, 0);
            }
            return this.portrait_;
        }

        public function setAttack(containerType:int, attackAngle:Number):void
        {
            this.attackAngle_ = attackAngle;
            this.attackStart_ = getTimer();
        }

        override public function draw3d(obj3d:Vector.<Object3DStage3D>):void
        {
            if (this.object3d_ != null)
            {
                obj3d.push(this.object3d_);
            }
        }
        protected function drawHpBar(graphicsData:Vector.<IGraphicsData>, time:int):void
        {
            if (this.hpbarPath_ == null)
            {
                this.hpbarBackFill_ = new GraphicsSolidFill();
                this.hpbarBackPath_ = new GraphicsPath(GraphicsUtil.QUAD_COMMANDS, new Vector.<Number>());
                this.hpbarFill_ = new GraphicsSolidFill(0x10FF00);
                this.hpbarPath_ = new GraphicsPath(GraphicsUtil.QUAD_COMMANDS, new Vector.<Number>());
            }
            var maxHp:Number = this.maxHP_;
            if (!this.ishpScaleSet && (this.hp_ > this.maxHP_))
            {
                this.maxHP_ = this.hp_;
                maxHp = this.maxHP_;
                this.ishpScaleSet = true;
            }
            if (this.hp_ <= maxHp)
            {
                var hp:Number = (maxHp - this.hp_) / maxHp;
                this.hpbarBackFill_.color = MoreColorUtil.lerpColor(0x545454, 0xFF0000, Math.abs(Math.sin(time / 300)) * hp);
            }
            else
            {
                this.hpbarBackFill_.color = 0x545454;
            }
            this.hpbarBackPath_.data.length = 0;
            this.hpbarBackPath_.data.push(posS_[0] - 20, posS_[1] + 4, posS_[0] + 20, posS_[1] + 4, posS_[0] + 20, posS_[1] + 10, posS_[0] - 20, posS_[1] + 10);
            graphicsData.push(this.hpbarBackFill_);
            graphicsData.push(this.hpbarBackPath_);
            graphicsData.push(GraphicsUtil.END_FILL);
            if (this.hp_ > 0)
            {
                var hp:Number = ((this.hp_ / this.maxHP_) * 2) * 20;
                this.hpbarPath_.data.length = 0;
                this.hpbarPath_.data.push(posS_[0] - 20, posS_[1] + 4, posS_[0] - 20 + hp, posS_[1] + 4, posS_[0] - 20 + hp, posS_[1] + 10, posS_[0] - 20, posS_[1] + 10);
                graphicsData.push(this.hpbarFill_);
                graphicsData.push(this.hpbarPath_);
                graphicsData.push(GraphicsUtil.END_FILL);
            }
            GraphicsFillExtra.setSoftwareDrawSolid(this.hpbarFill_, true);
            GraphicsFillExtra.setSoftwareDrawSolid(this.hpbarBackFill_, true);

        }

        override public function draw(graphicsData:Vector.<IGraphicsData>, camera:Camera, time:int):void
        {
            var bitmapData:BitmapData;
            var texture:BitmapData = this.getTexture(camera, time);
            if (this.props_.drawOnGround_)
            {
                if (square_.faces_.length == 0)
                {
                    return;
                }
                this.path_.data = square_.faces_[0].face_.vout_;
                this.bitmapFill_.bitmapData = texture;
                square_.baseTexMatrix_.calculateTextureMatrix(this.path_.data);
                this.bitmapFill_.matrix = square_.baseTexMatrix_.tToS_;
                graphicsData.push(this.bitmapFill_);
                graphicsData.push(this.path_);
                graphicsData.push(GraphicsUtil.END_FILL);
                return;
            }
            if (this.obj3D_ != null)
            {
                if (!Parameters.isGpuRender())
                {
                    this.obj3D_.draw(graphicsData, camera, this.props_.color_, texture);
                    return;
                }
                if (Parameters.isGpuRender()) {
                    //this.obj3D_.draw(graphicsData, camera, this.props_.color_, texture);
                    graphicsData.push(null);
                  //  return; //todo
                }
            }
            var w:int = texture.width;
            var h:int = texture.height;
            var h2:int = square_.sink_ + this.sinkLevel_;
            if (h2 > 0 && (this.flying_ || square_.obj_ != null && square_.obj_.props_.protectFromSink_))
            {
                h2 = 0;
            }
            if (Parameters.isGpuRender())
            {
                if (h2 != 0)
                {
                    GraphicsFillExtra.setSinkLevel(this.bitmapFill_, Math.max(((h2 / h) * 1.65) - 0.02, 0)); //todo
                    h2 = -h2 + 0.02;
                }
                else if (h2 == 0 && GraphicsFillExtra.getSinkLevel(this.bitmapFill_) != 0)
                {
                        GraphicsFillExtra.clearSink(this.bitmapFill_);
                }
            }
            this.vS_.length = 0;
            this.vS_.push(posS_[3] - (w / 2), posS_[4] - h + h2, posS_[3] + w / 2, posS_[4] - h + h2, posS_[3] + (w / 2), posS_[4], posS_[3] - (w / 2), posS_[4]);
            this.path_.data = this.vS_;
            if (this.flash_ != null)
            {
                if (!this.flash_.doneAt(time))
                {
                    if (Parameters.isGpuRender())
                    {
                        this.flash_.applyGPUTextureColorTransform(texture, time);
                    }
                    else
                    {
                        texture = this.flash_.apply(texture, time);
                    }
                }
                else
                {
                    this.flash_ = null;
                }
            }
            if (this.isShocked && !this.isShockedTransformSet)
            {
                if (Parameters.isGpuRender())
                {
                    GraphicsFillExtra.setColorTransform(texture, new ColorTransform(-1, -1, -1, 1, 0xFF, 0xFF, 0xFF, 0));
                }
                else
                {
                    bitmapData = texture.clone();
                    bitmapData.colorTransform(bitmapData.rect, new ColorTransform(-1, -1, -1, 1, 0xFF, 0xFF, 0xFF, 0));
                    bitmapData = CachingColorTransformer.filterBitmapData(bitmapData, SHOCKED_FILTER);
                    texture = bitmapData;
                }
                this.isShockedTransformSet = true;
            }
            if (this.isCharging && !this.isChargingTransformSet)
            {
                if (Parameters.isGpuRender())
                {
                    GraphicsFillExtra.setColorTransform(texture, new ColorTransform(1, 1, 1, 1, 0xFF, 0xFF, 0xFF, 0));
                }
                else
                {
                    bitmapData = texture.clone();
                    bitmapData.colorTransform(bitmapData.rect, new ColorTransform(1, 1, 1, 1, 0xFF, 0xFF, 0xFF, 0));
                    texture = bitmapData;
                }
                this.isChargingTransformSet = true;
            }
            this.bitmapFill_.bitmapData = texture;
            this.fillMatrix_.identity();
            this.fillMatrix_.translate(this.vS_[0], this.vS_[1]);
            this.bitmapFill_.matrix = this.fillMatrix_;
            graphicsData.push(this.bitmapFill_);
            graphicsData.push(this.path_);
            graphicsData.push(GraphicsUtil.END_FILL);
            if (!this.isPaused() && (this.condition_[ConditionEffect.CE_FIRST_BATCH] || this.condition_[ConditionEffect.CE_SECOND_BATCH]) && !Parameters.screenShotMode_ && !(this is Pet))
            {
                this.drawConditionIcons(graphicsData, camera, time);
            }
            if (this.props_.showName_ && this.name_ != null && this.name_.length != 0)
            {
                this.drawName(graphicsData, camera);
            }
            if (this.props_ && (this.props_.isEnemy_ || this.props_.isPlayer_) && !this.isInvisible() && !this.isInvulnerable() && !this.props_.noMiniMap_)
            {
                var i:uint = texture.getPixel32(texture.width / 4, texture.height / 4) | texture.getPixel32(texture.width / 2, texture.height / 2) | texture.getPixel32((texture.width * 3) / 4, (texture.height * 3) / 4);
                var i2:uint = i >> 24;
                if (i2 != 0)
                {
                    hasShadow_ = true;
                    if (Parameters.data_.HPBar)
                    {
                        this.drawHpBar(graphicsData, time);
                    }
                }
                else
                {
                    hasShadow_ = false;
                }
            }
        }

        public function drawConditionIcons(graphicsData:Vector.<IGraphicsData>, camera:Camera, time:int):void
        {
            var icon:BitmapData;
            var fill:GraphicsBitmapFill;
            var path:GraphicsPath;
            var x:Number;
            var y:Number;
            var m:Matrix;
            if (this.icons_ == null)
            {
                this.icons_ = new Vector.<BitmapData>();
                this.iconFills_ = new Vector.<GraphicsBitmapFill>();
                this.iconPaths_ = new Vector.<GraphicsPath>();
            }
            this.icons_.length = 0;
            var index:int = (time / 500);
            ConditionEffect.getConditionEffectIcons(this.condition_[ConditionEffect.CE_FIRST_BATCH], this.icons_, index);
            ConditionEffect.getConditionEffectIcons2(this.condition_[ConditionEffect.CE_SECOND_BATCH], this.icons_, index);
            var centerX:Number = posS_[3];
            var centerY:Number = this.vS_[1];
            var len:int = this.icons_.length;
            for (var i:int = 0; i < len; i++)
            {
                icon = this.icons_[i];
                if (i >= this.iconFills_.length)
                {
                    this.iconFills_.push(new GraphicsBitmapFill(null, new Matrix(), false, false));
                    this.iconPaths_.push(new GraphicsPath(GraphicsUtil.QUAD_COMMANDS, new Vector.<Number>()));
                }
                fill = this.iconFills_[i];
                path = this.iconPaths_[i];
                fill.bitmapData = icon;
                x = (centerX - ((icon.width * len) / 2)) + (i * icon.width);
                y = centerY - (icon.height / 2);
                path.data.length = 0;
                path.data.push(x, y, x + icon.width, y, x + icon.width, y + icon.height, x, y + icon.height);
                m = fill.matrix;
                m.identity();
                m.translate(x, y);
                graphicsData.push(fill);
                graphicsData.push(path);
                graphicsData.push(GraphicsUtil.END_FILL);
            }
        }

        override public function drawShadow(graphicsData:Vector.<IGraphicsData>, camera:Camera, time:int):void
        {
            if (this.shadowGradientFill_ == null)
            {
                this.shadowGradientFill_ = new GraphicsGradientFill(GradientType.RADIAL, [this.props_.shadowColor_, this.props_.shadowColor_], [0.5, 0], null, new Matrix());
                this.shadowPath_ = new GraphicsPath(GraphicsUtil.QUAD_COMMANDS, new Vector.<Number>());
            }
            var s:Number = ((this.size_ / 100) * (this.props_.shadowSize_ / 100)) * this.sizeMult_;
            var w:Number = 30 * s;
            var h:Number = 15 * s;
            this.shadowGradientFill_.matrix.createGradientBox(w * 2, h * 2, 0, posS_[0] - w, posS_[1] - h);
            graphicsData.push(this.shadowGradientFill_);
            this.shadowPath_.data.length = 0;
            this.shadowPath_.data.push(posS_[0] - w, posS_[1] - h, posS_[0] + w, posS_[1] - h, posS_[0] + w, posS_[1] + h, posS_[0] - w, posS_[1] + h);
            graphicsData.push(this.shadowPath_);
            graphicsData.push(GraphicsUtil.END_FILL);
        }

        public function clearTextureCache():void
        {
            this.texturingCache_ = new Dictionary();
        }

        public function toString():String
        {
            return "[" + getQualifiedClassName(this) + " id: " + objectId_ + " type: " + ObjectLibrary.typeToDisplayId_[this.objectType_] + " pos: " + x_ + ", " + y_ + "]";
        }
    }
}
