using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace PikTestPlugin.Models
{
    internal static class RevitStaticData
    {
        private static readonly string _roomZoneParameterName = "ROM_Зона";
        private static readonly string _roomDefinitionParameterName = "Квартира";

        public static Document Document { get; private set; }
        public static List<SpatialElement> SpatialElements { get; private set; }
        public static List<SpatialElement> FilteredSpatialElements { get; private set; }
        public static string RoomZoneParameterName => _roomZoneParameterName;
        public static string RoomDefinitionParameterName => _roomDefinitionParameterName;

        public static void Initialize(UIApplication uiApplication)
        {
            Document = uiApplication.ActiveUIDocument.Document;
            SpatialElements =
                    new FilteredElementCollector(Document).OfClass(typeof(SpatialElement)).Cast<SpatialElement>().ToList();
            FilteredSpatialElements = GetSpatialElementsByParameterAndPurpose(SpatialElements,
                _roomZoneParameterName, _roomDefinitionParameterName);
        }

        private static List<SpatialElement> GetSpatialElementsByParameterAndPurpose(
            List<SpatialElement> spatialElements, string parameterName, string purpose) =>
            spatialElements.Where(s => s.GetParameters(parameterName).ToList().Any(p => p.AsString().Contains(purpose))).ToList();
    }
}
