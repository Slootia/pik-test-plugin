using Autodesk.Revit.DB;
using PikTestPlugin.Models;
using System.Collections.Generic;

namespace PikTestPlugin.Interfaces
{
    internal interface ILevel
    {
        string Name { get; set; }
        List<SpatialElement> SpatialElements { get; set; }
        List<ApartmentLayout> ApartmentLayouts { get; set; }
    }
}