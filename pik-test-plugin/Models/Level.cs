using Autodesk.Revit.DB;
using PikTestPlugin.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PikTestPlugin.Models
{
    internal sealed class Level : ILevel, IPaintable
    {
        public Level(List<SpatialElement> spatialElements)
        {
            Initialize(spatialElements);
        }

        public string Name { get; set; }
        public bool IsPainted { get; private set; } = false;
        public List<SpatialElement> SpatialElements { get; set; }
        public List<ApartmentLayout> ApartmentLayouts { get; set; } = new List<ApartmentLayout>();

        private Level Initialize(List<SpatialElement> spatialElements)
        {
            SpatialElements = spatialElements;
            Name = GetLevelName(SpatialElements.FirstOrDefault());
            FillAppartmentLayouts(SpatialElements);
            return this;
        }

        private void FillAppartmentLayouts(List<SpatialElement> spatialElements)
        {
            var appartmentLayoutsGroup = spatialElements
                .GroupBy(r => r.GetParameters(ParametersNames.RoomNumberOfRoomsParameterName).FirstOrDefault().AsString());

            foreach (var appartmentLayouts in appartmentLayoutsGroup)
            {
                ApartmentLayouts.Add(new ApartmentLayout(appartmentLayouts.ToList()));
            }
            ApartmentLayouts = ApartmentLayouts.OrderBy(g => g.RoomsCount).ToList();
        }

        private string GetLevelName(SpatialElement spatialElement) => spatialElement.Level.Name;

        public void Paint()
        {
            if (PluginTransaction.IsStarted)
            {
                foreach (var apartmentLayout in ApartmentLayouts)
                {
                    apartmentLayout.Paint();
                }
            }
            else
            {
                using (PluginTransaction.RevitTransaction)
                {
                    PluginTransaction.Start();
                    foreach (var apartmentLayout in ApartmentLayouts)
                    {
                        apartmentLayout.Paint();
                    }
                    PluginTransaction.Commit();
                }
            }
            IsPainted = true;
        }
    }
}
