using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace PikTestPlugin.Interfaces
{
    internal interface ISection
    {
        string Number { get; set; }
        List<Models.Level> Levels { get; set; }
    }
}