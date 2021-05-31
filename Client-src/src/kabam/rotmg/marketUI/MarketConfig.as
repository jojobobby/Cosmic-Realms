package kabam.rotmg.marketUI {
import kabam.rotmg.marketUI.components.items.MarketItemBase;
import kabam.rotmg.marketUI.mediators.MarketItemBaseMediator;

import org.swiftsuspenders.Injector;

import robotlegs.bender.extensions.mediatorMap.api.IMediatorMap;
import robotlegs.bender.extensions.signalCommandMap.api.ISignalCommandMap;

import robotlegs.bender.framework.api.IConfig;

public class MarketConfig implements IConfig {
    [Inject]
    public var injector:Injector;
    [Inject]
    public var mediatorMap:IMediatorMap;
    [Inject]
    public var commandMap:ISignalCommandMap;

    public function configure():void {
        this.mediatorMap.map(MarketItemBase).toMediator(MarketItemBaseMediator);
    }


}
}
