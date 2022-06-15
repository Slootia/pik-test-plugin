using PikTestPlugin.Interfaces;

namespace PikTestPlugin
{
    internal class Painter : IPainter
    {
        public void Paint<T>(T objectToPaint) where T : IPaintable
        {
            objectToPaint.Paint();
        }
    }
}
