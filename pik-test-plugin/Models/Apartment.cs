using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PikTestPlugin.Models
{
    internal sealed class Apartment
    {
        public Apartment(List<SpatialElement> spatialElements)
        {
            Initialize(spatialElements);
        }
       
        public Apartment(List<SpatialElement> spatialElements, List<Apartment> adjacentApartments) : this (spatialElements)
        {
            AdjacentApartments = adjacentApartments;
        }

        private const string _roomPurposeParameterName = "ROM_Зона";
        private const string _roomNumberOfRoomsParameterName = "ROM_Подзона";
        private readonly Regex _onlyDigits = new Regex(@"^\d+$");

        public int Number { get; set; }
        public string NumberOfRooms { get; set; }
        public int RoomsCount { get; set; }
        public List<SpatialElement> Rooms { get; set; } = new List<SpatialElement>();
        public List<Apartment> AdjacentApartments { get; set; } = new List<Apartment>();

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
            var roomNumber = spatialElement.GetParameters(_roomPurposeParameterName).FirstOrDefault().AsString();
            var onlyNumbers = new String(roomNumber.Where(Char.IsDigit).ToArray());
            int.TryParse(onlyNumbers, out int result);
            return result;
        }
        
        private string GetNumberOfRooms(SpatialElement spatialElement) =>
            spatialElement.GetParameters(_roomNumberOfRoomsParameterName).FirstOrDefault().AsString();
    }
}
