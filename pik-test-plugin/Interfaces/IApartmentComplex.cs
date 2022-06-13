using Autodesk.Revit.DB;
using PikTestPlugin.Models;
using System.Collections.Generic;

namespace PikTestPlugin.Interfaces
{
    internal interface IApartmentComplex
    {
        List<Section> Sections { get; set; }
        List<SpatialElement> SpatialElements { get; set; }

        ApartmentComplex Initialize(List<SpatialElement> spatialElements);
    }
}