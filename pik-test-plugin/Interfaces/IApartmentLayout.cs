using Autodesk.Revit.DB;
using PikTestPlugin.Models;
using System.Collections.Generic;

namespace PikTestPlugin.Interfaces
{
    internal interface IApartmentLayout
    {
        string NumberOfRooms { get; set; }
        int RoomsCount { get; set; }
        List<Apartment> Apartments { get; set; }
        List<SpatialElement> SpatialElements { get; set; }
    }
}