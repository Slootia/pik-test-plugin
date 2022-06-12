using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;

namespace PikTestPlugin.Models
{
    internal sealed class Level
    {
        public Level(List<SpatialElement> spatialElements)
        {
            Initialize(spatialElements);
        }

        private const string _roomNumberOfRoomsParameterName = "ROM_Подзона";

        public string Name { get; set; }
        public List<SpatialElement> SpatialElements { get; set; }
        public List<ApartmentLayout> ApartmentLayouts { get; set; } = new List<ApartmentLayout>();

        private void Initialize(List<SpatialElement> spatialElements)
        {
            SpatialElements = spatialElements;
            Name = GetLevelName(spatialElements.FirstOrDefault());
            FillAppartmentLayouts(spatialElements);
        }

        private void FillAppartmentLayouts(List<SpatialElement> spatialElements)
        {
            var appartmentLayoutsGroup = spatialElements
                .GroupBy(r => r.GetParameters(_roomNumberOfRoomsParameterName).FirstOrDefault().AsString());

            foreach (var appartmentLayouts in appartmentLayoutsGroup)
            {
                ApartmentLayouts.Add(new ApartmentLayout(appartmentLayouts.ToList()));
            }
            ApartmentLayouts.OrderBy(g => g.RoomsCount);
        }

        private string GetLevelName(SpatialElement spatialElement) => spatialElement.Level.Name;
    }
}
