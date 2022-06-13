using Autodesk.Revit.DB;
using PikTestPlugin.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PikTestPlugin.Models
{
    internal sealed class ApartmentLayout : IApartmentLayout
    {
        public ApartmentLayout(List<SpatialElement> spatialElements)
        {
            Initialize(spatialElements);
        }

        public string NumberOfRooms { get; set; }
        public int RoomsCount { get; set; }
        public List<SpatialElement> SpatialElements { get; set; }
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
                .GroupBy(a => a.GetParameters(ParametersNames.ApartmentParameterName).FirstOrDefault().AsString());

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
            spatialElement.GetParameters(ParametersNames.RoomNumberOfRoomsParameterName).FirstOrDefault().AsString();
    }
}
