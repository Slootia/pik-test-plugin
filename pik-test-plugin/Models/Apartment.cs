using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PikTestPlugin.Models
{
    internal sealed class Apartment
    {
        public Apartment(List<SpatialElement> spatialElements)
        {
            Initialize(spatialElements);
        }

        public Apartment(List<SpatialElement> spatialElements, List<Apartment> adjacentApartments) : this(spatialElements)
        {
            AdjacentApartments = adjacentApartments;
        }
        
        public int Number { get; set; }
        public string NumberOfRooms { get; set; }
        public int RoomsCount { get; set; }
        public bool IsPainted { get; set; } = false;
        public List<SpatialElement> Rooms { get; set; } = new List<SpatialElement>();
        public List<Apartment> AdjacentApartments { get; set; }

        private Apartment Initialize(List<SpatialElement> spatialElements)
        {
            Rooms = spatialElements;
            Number = GetApartmentNumber(Rooms.FirstOrDefault());
            NumberOfRooms = GetNumberOfRooms(Rooms.FirstOrDefault());
            RoomsCount = Rooms.Count;
            return this;
        }

        private int GetApartmentNumber(SpatialElement spatialElement)
        {
            var roomNumber = spatialElement.GetParameters(ParametersNames.ApartmentParameterName).FirstOrDefault().AsString();
            var onlyNumbers = new String(roomNumber.Where(Char.IsDigit).ToArray());
            int.TryParse(onlyNumbers, out int result);
            return result;
        }

        private string GetNumberOfRooms(SpatialElement spatialElement) =>
            spatialElement.GetParameters(ParametersNames.RoomNumberOfRoomsParameterName).FirstOrDefault().AsString();
    }
}
