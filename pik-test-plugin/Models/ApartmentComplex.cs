using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;

namespace PikTestPlugin.Models
{
    internal sealed class ApartmentComplex
    {

        private const string _apartmentParameterName = "ROM_Зона";
        private const string _apartmentPurposeParameterName = "Квартира";
        private const string _sectionParameterName = "BS_Блок";

        public List<Section> Sections { get; set; } = new List<Section>();
        public List<SpatialElement> SpatialElements { get; set; }

        public ApartmentComplex Initialize(List<SpatialElement> spatialElements)
        {
            SpatialElements = spatialElements;
            FillSections(GetSpatialElementsByParameterAndPurpose(spatialElements, _apartmentParameterName, _apartmentPurposeParameterName));
            return this;
        }


        private List<SpatialElement> GetSpatialElementsByParameterAndPurpose(List<SpatialElement> spatialElements, string parameterName, string purpose)
        {
            return spatialElements
                .Where(s => s.GetParameters(parameterName).ToList().Any(p => p.AsString().Contains(purpose))).ToList();
        }

        private void FillSections(List<SpatialElement> spatialElements)
        {
            var roomsBySections = spatialElements
                .GroupBy(r => r.GetParameters(_sectionParameterName).FirstOrDefault().AsString()).OrderBy(g => g.Key);

            foreach (var sectionRooms in roomsBySections)
            {
                Sections.Add(new Section(sectionRooms.ToList()));
            }
        }
    }
}
