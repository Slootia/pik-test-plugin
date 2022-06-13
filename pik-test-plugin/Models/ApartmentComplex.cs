using Autodesk.Revit.DB;
using PikTestPlugin.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PikTestPlugin.Models
{
    internal sealed class ApartmentComplex : IApartmentComplex
    {
        public List<Section> Sections { get; set; } = new List<Section>();
        public List<SpatialElement> SpatialElements { get; set; }

        public ApartmentComplex Initialize(List<SpatialElement> spatialElements)
        {
            SpatialElements = spatialElements;
            FillSections(GetSpatialElementsByParameterAndPurpose(spatialElements, ParametersNames.ApartmentParameterName, ParametersNames.ApartmentPurposeParameterName));
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
                .GroupBy(r => r.GetParameters(ParametersNames.SectionParameterName).FirstOrDefault().AsString()).OrderBy(g => g.Key);

            foreach (var sectionRooms in roomsBySections)
            {
                Sections.Add(new Section(sectionRooms.ToList()));
            }
        }
    }
}
