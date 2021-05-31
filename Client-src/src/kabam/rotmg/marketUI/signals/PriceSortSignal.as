package kabam.rotmg.marketUI.signals {
import org.osflash.signals.Signal;

public class PriceSortSignal extends Signal{
    public static var _staticInstance:PriceSortSignal;

    public function PriceSortSignal() {
        super(int);
        _staticInstance = this;
    }
}
}
