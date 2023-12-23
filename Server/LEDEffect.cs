//+--------------------------------------------------------------------------
//
// NightDriver.Net - (c) 2023 Dave Plummer.  All Rights Reserved.
//
// File:        LEDEffect.cs
//
// Description:
//
//   Represents an effect object that knows how to Render itself
//   
// History:     Dec-18-2023        Davepl      Cleanup
//
//---------------------------------------------------------------------------
using NightDriver;

[Serializable]
public class LEDEffect 
{
    public virtual string EffectName
    {
        get
        {
            return this.GetType().Name;
        }
    }

    protected virtual void Render(ILEDGraphics graphics)
    {
        // BUGBUG class and this methoi would be abstract except for serialization requiremets // BUGBUG What?  What serialization?
        throw new ApplicationException("Render Base Class called - This is abstract");
    }
    
    public void DrawFrame(ILEDGraphics graphics)
    {
        //lock(graphics)
        {
            Render(graphics);
        }
    }

}
   