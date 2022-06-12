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
        public static List<SpatialElement> SpatialElements { get; set; }
        public List<Apartment> Apartments { get; set; } = new List<Apartment>();

        private ApartmentLayout Initialize(List<SpatialElement> spatialElements)
        {
            SpatialElements = spatialElements;
            NumberOfRooms = GetNumberOfRooms(SpatialElements.FirstOrDefault());
            RoomsCount = SpatialElements.Count;
            FillAppartments(SpatialElements);
            return this;
        }

        private void FillAppartments(List<SpatialElement> spatialElements)
        {
            var appartmentsGroup = spatialElements
                .GroupBy(a => a.GetParameters(_roomPurposeParameterName).FirstOrDefault().AsString());
            
            foreach (var apartments in appartmentsGroup)
            {
                Apartments.Add(new Apartment(apartments.ToList()));
            }

            Apartments = Apartments.OrderBy(g => g.Number).ToList();

            foreach (var apartment in Apartments)
            {
                apartment.AdjacentApartments = FindAdjacentApartments(apartment);
            }
        }

        private List<Apartment> FindAdjacentApartments(Apartment apartment)
        {
            var adjacentApartments = new List<Apartment>();

            for (int i = 0; i < Apartments.Count; i++)
            {
                var currentApartment = apartment;
                Apartment comparableApartment;
                
                if (i + 1 >= Apartments.Count)
                {
                    comparableApartment = Apartments[0];
                }
                else
                {
                    comparableApartment = Apartments[i + 1];
                }



                if (IsApartmentsAdjacent(currentApartment, comparableApartment))
                {
                    adjacentApartments.Add(comparableApartment);
                }
            }

            return adjacentApartments;
        }

        private bool IsApartmentsAdjacent(Apartment firstApartment, Apartment secondApartment)
        {
            var difference = firstApartment.Number - secondApartment.Number;
            if (difference == 1 || difference == -1)
            {
                return true;
            }
            return false;
        }

        private string GetNumberOfRooms(SpatialElement spatialElement) =>
            spatialElement.GetParameters(_roomNumberOfRoomsParameterName).FirstOrDefault().AsString();
    }
}
