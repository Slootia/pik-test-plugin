using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;

namespace PikTestPlugin.Models
{
    internal static class ApartmentComplex
    {

        private const string _apartmentParameterName = "ROM_Зона";
        private const string _apartmentPurposeParameterName = "Квартира";

        public static List<Section> Sections { get; set; } = new List<Section>();

        public static void Initialize(List<SpatialElement> spatialElements)
        {
            FillSections(GetSpatialElementsByParameterAndPurpose(spatialElements, _apartmentParameterName, _apartmentPurposeParameterName));
        }


        private static List<SpatialElement> GetSpatialElementsByParameterAndPurpose(List<SpatialElement> spatialElements, string parameterName, string purpose)
        {
            return spatialElements
                .Where(s => s.GetParameters(parameterName).ToList().Any(p => p.AsString().Contains(purpose))).ToList();
        }

        private static void FillSections(List<SpatialElement> spatialElements)
        {
            var roomsBySections = spatialElements
                .GroupBy(r => r.GetParameters("BS_Блок").FirstOrDefault().AsString()).OrderBy(g => g.Key);

            foreach (var sectionRooms in roomsBySections)
            {
                Sections.Add(new Section(sectionRooms.ToList()));
            }
        }
    }
}
