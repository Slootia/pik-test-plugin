using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Linq;

namespace PikTestPlugin.Models
{
    internal sealed class Section
    {
        public Section(List<SpatialElement> spatialElements)
        {
            Initialize(spatialElements);
        }

        public string Number { get; set; }
        public List<SpatialElement> SpatialElements { get; set; }
        public List<Level> Levels { get; set; } = new List<Level>();

        private Section Initialize(List<SpatialElement> spatialElements)
        {
            SpatialElements = spatialElements;
            Number = GetSectionNumber(SpatialElements.FirstOrDefault());
            FillLevels(SpatialElements);
            return this;
        }


        private void FillLevels(List<SpatialElement> spatialElements)
        {
            var roomsByLevel = spatialElements.GroupBy(r => r.get_Parameter(BuiltInParameter.LEVEL_NAME).AsString()).OrderBy(g => g.Key);

            foreach (var level in roomsByLevel)
            {
                Levels.Add(new Level(level.ToList()));
            }
        }

        private string GetSectionNumber(SpatialElement spatialElement) => 
            spatialElement.GetParameters("BS_Блок").FirstOrDefault().AsString();
    }
}
