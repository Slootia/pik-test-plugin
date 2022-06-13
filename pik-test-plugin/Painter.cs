using Autodesk.Revit.DB;
using PikTestPlugin.Interfaces;
using PikTestPlugin.Models;
using System.Linq;

namespace PikTestPlugin
{
    internal class Painter : IPainter
    {
        private readonly string _roomCalculatedSubzoneIdParameterName = "ROM_Расчетная_подзона_ID";
        private readonly string _roomSubzoneIndexParameterName = "ROM_Подзона_Index";
        private readonly string _roomSufixToPaint = ".Полутон";
        private readonly Transaction _transaction = new Transaction(RevitStaticData.Document);
        private const string _transactionMessage = "Изменение тона у прилежащих типов помещений";

        public string RoomCalculatedSubzoneIdParameterName => _roomCalculatedSubzoneIdParameterName;
        public string RoomSubzoneIndexParameterName => _roomSubzoneIndexParameterName;
        public string RoomSufixToPaint => _roomSufixToPaint;

        public void PaintAdjacent(ApartmentComplex complex)
        {
            if (_transaction.GetStatus() == TransactionStatus.Started ||
                _transaction.GetStatus() == TransactionStatus.Committed)
            {
                foreach (var section in complex.Sections)
                {
                    PaintAdjacent(section);
                }
            }
            else
            {
                using (_transaction)
                {
                    _transaction.Start(_transactionMessage);
                    foreach (var section in complex.Sections)
                    {
                        PaintAdjacent(section);
                    }
                    _transaction.Commit();
                }
            }
        }

        public void PaintAdjacent(Section section)
        {
            if (_transaction.GetStatus() == TransactionStatus.Started ||
                _transaction.GetStatus() == TransactionStatus.Committed)
            {
                foreach (var level in section.Levels)
                {
                    PaintAdjacent(level);
                }
            }
            else
            {
                using (_transaction)
                {
                    _transaction.Start(_transactionMessage);
                    foreach (var level in section.Levels)
                    {
                        PaintAdjacent(level);
                    }
                    _transaction.Commit();
                }
            }
        }

        public void PaintAdjacent(Models.Level level)
        {
            if (_transaction.GetStatus() == TransactionStatus.Started ||
                _transaction.GetStatus() == TransactionStatus.Committed)
            {
                foreach (var apartmentLayout in level.ApartmentLayouts)
                {
                    PaintAdjacent(apartmentLayout);
                }
            }
            else
            {
                using (_transaction)
                {
                    _transaction.Start(_transactionMessage);
                    foreach (var apartmentLayout in level.ApartmentLayouts)
                    {
                        PaintAdjacent(apartmentLayout);
                    }
                    _transaction.Commit();
                }
            }
        }

        public void PaintAdjacent(ApartmentLayout apartmentLayout)
        {
            if (_transaction.GetStatus() == TransactionStatus.Started ||
                _transaction.GetStatus() == TransactionStatus.Committed)
            {
                foreach (var apartment in apartmentLayout.Apartments)
                {
                    PaintAdjacent(apartment);
                }
            }
            else
            {
                using (_transaction)
                {
                    _transaction.Start(_transactionMessage);
                    foreach (var apartment in apartmentLayout.Apartments)
                    {
                        PaintAdjacent(apartment);
                    }
                    _transaction.Commit();
                }
            }
        }

        public void PaintAdjacent(Apartment apartment)
        {
            if (_transaction.GetStatus() == TransactionStatus.Started ||
                _transaction.GetStatus() == TransactionStatus.Committed)
            {
                if (apartment.AdjacentApartments.Count == 0 || apartment.IsPainted)
                {
                    return;
                }

                if (apartment.AdjacentApartments.Count > 1)
                {
                    if (apartment.AdjacentApartments.FirstOrDefault().IsPainted)
                    {
                        PaintApartment(apartment.AdjacentApartments.LastOrDefault());
                    }
                    else if (apartment.AdjacentApartments.LastOrDefault().IsPainted)
                    {
                        PaintApartment(apartment.AdjacentApartments.FirstOrDefault());
                    }
                }
                else
                {
                    PaintApartment(apartment.AdjacentApartments.LastOrDefault());
                }
            }
            else
            {
                using (_transaction)
                {
                    _transaction.Start(_transactionMessage);
                    if (apartment.AdjacentApartments.Count == 0 || apartment.IsPainted)
                    {
                        return;
                    }

                    if (apartment.AdjacentApartments.Count > 1)
                    {
                        if (apartment.AdjacentApartments.FirstOrDefault().IsPainted)
                        {
                            PaintApartment(apartment.AdjacentApartments.LastOrDefault());
                        }
                        else if (apartment.AdjacentApartments.LastOrDefault().IsPainted)
                        {
                            PaintApartment(apartment.AdjacentApartments.FirstOrDefault());
                        }
                    }
                    else
                    {
                        PaintApartment(apartment.AdjacentApartments.LastOrDefault());
                    }
                    _transaction.Commit();
                }
            }
        }

        private void PaintApartment(Apartment apartment)
        {
            using (SubTransaction subTransaction = new SubTransaction(RevitStaticData.Document))
            {
                subTransaction.Start();
                foreach (var room in apartment.Rooms)
                {
                    var calculatedSubZoneId = room.GetParameters(_roomCalculatedSubzoneIdParameterName).FirstOrDefault().AsString();
                    room.GetParameters(_roomSubzoneIndexParameterName).FirstOrDefault().Set(calculatedSubZoneId + _roomSufixToPaint);
                }
                subTransaction.Commit();
            }
            apartment.IsPainted = true;
        }
    }
}
