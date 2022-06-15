using Autodesk.Revit.DB;
using PikTestPlugin.Models;
using System.Collections.Generic;

namespace PikTestPlugin.Interfaces
{
    internal interface IApartment
    {
        int Number { get; set; }
        string NumberOfRooms { get; set; }
        int RoomsCount { get; set; }
        List<Apartment> AdjacentApartments { get; set; }
    }
}