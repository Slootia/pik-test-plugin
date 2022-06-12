using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PikTestPlugin.Models
{
    internal class ApartmentLayout
    {
        public ApartmentLayout(List<SpatialElement> spatialElements)
        {
            Initialize(spatialElements);
        }

        private const string _roomPurposeParameterName = "ROM_Зона";
        private const string _roomNumberOfRoomsParameterName = "ROM_Подзона";

        public string NumberOfRooms { get; set; }
        public int RoomsCount { get; set; }
        public List<Apartment> Apartments { get; set; } = new List<Apartment>();

        private void Initialize(List<SpatialElement> spatialElements)
        {
            NumberOfRooms = GetNumberOfRooms(spatialElements.FirstOrDefault());
            RoomsCount = spatialElements.Count;
            FillAppartments(spatialElements);
        }

        private void FillAppartments(List<SpatialElement> spatialElements)
        {
            var appartmentsGroup = spatialElements
                .GroupBy(a => a.GetParameters(_roomPurposeParameterName).FirstOrDefault().AsString());
            
            foreach (var apartments in appartmentsGroup)
            {
                Apartments.Add(new Apartment(apartments.ToList()));
            }
        }

        private string GetNumberOfRooms(SpatialElement spatialElement) =>
            spatialElement.GetParameters(_roomNumberOfRoomsParameterName).FirstOrDefault().AsString();
    }
}
