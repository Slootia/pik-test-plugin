using PikTestPlugin.Models;

namespace PikTestPlugin.Interfaces
{
    internal interface IPainter
    {
        void Paint<T>(T objectToPaint) where T : IPaintable;
    }
}