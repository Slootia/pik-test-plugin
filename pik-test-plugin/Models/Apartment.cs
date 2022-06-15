using Autodesk.Revit.DB;
using PikTestPlugin.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PikTestPlugin.Models
{
    internal sealed class Apartment : IApartment, IPaintable
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
        public bool IsPainted { get; private set; } = false;
        public List<SpatialElement> SpatialElements { get; set; } = new List<SpatialElement>();
        public List<Apartment> AdjacentApartments { get; set; }

        private Apartment Initialize(List<SpatialElement> spatialElements)
        {
            SpatialElements = spatialElements;
            Number = GetApartmentNumber(SpatialElements.FirstOrDefault());
            NumberOfRooms = GetNumberOfRooms(SpatialElements.FirstOrDefault());
            RoomsCount = SpatialElements.Count;
            return this;
        }

        public void Paint()
        {
            if (PluginTransaction.IsStarted)
            {
                if (AdjacentApartments.Count == 0 || IsPainted)
                {
                    return;
                }

                if (AdjacentApartments.Count > 1)
                {
                    if (AdjacentApartments.FirstOrDefault().IsPainted)
                    {
                        PaintApartment(AdjacentApartments.LastOrDefault());
                    }
                    else if (AdjacentApartments.LastOrDefault().IsPainted)
                    {
                        PaintApartment(AdjacentApartments.FirstOrDefault());
                    }
                }
                else
                {
                    PaintApartment(AdjacentApartments.LastOrDefault());
                }
            }
            else
            {
                using (PluginTransaction.RevitTransaction)
                {
                    PluginTransaction.Start();
                    if (AdjacentApartments.Count == 0 || IsPainted)
                    {
                        return;
                    }

                    if (AdjacentApartments.Count > 1)
                    {
                        if (AdjacentApartments.FirstOrDefault().IsPainted)
                        {
                            PaintApartment(AdjacentApartments.LastOrDefault());
                        }
                        else if (AdjacentApartments.LastOrDefault().IsPainted)
                        {
                            PaintApartment(AdjacentApartments.FirstOrDefault());
                        }
                    }
                    else
                    {
                        PaintApartment(AdjacentApartments.LastOrDefault());
                    }
                    PluginTransaction.Commit();
                }
            }
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

        private void PaintApartment(Apartment apartment)
        {
            using (SubTransaction subTransaction = new SubTransaction(RevitStaticData.Document))
            {
                subTransaction.Start();
                foreach (var room in apartment.SpatialElements)
                {
                    var calculatedSubZoneId = room.GetParameters(ParametersNames.RoomCalculatedSubzoneIdParameterName).FirstOrDefault().AsString();
                    room.GetParameters(ParametersNames.RoomSubzoneIndexParameterName).FirstOrDefault().Set(calculatedSubZoneId + ParametersNames.RoomSufixToPaint);
                }
                subTransaction.Commit();
            }
            apartment.IsPainted = true;
        }
    }
}
