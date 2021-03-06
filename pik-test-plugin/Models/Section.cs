using Autodesk.Revit.DB;
using PikTestPlugin.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PikTestPlugin.Models
{
    internal sealed class Section : ISection, IPaintable
    {
        public Section(List<SpatialElement> spatialElements)
        {
            Initialize(spatialElements);
        }

        public string Number { get; set; }
        public bool IsPainted { get; private set; } = false;
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
            spatialElement.GetParameters(ParametersNames.SectionParameterName).FirstOrDefault().AsString();

        public void Paint()
        {
            if (PluginTransaction.IsStarted)
            {
                foreach (var level in Levels)
                {
                    level.Paint();
                }
            }
            else
            {
                using (PluginTransaction.RevitTransaction)
                {
                    PluginTransaction.Start();
                    foreach (var level in Levels)
                    {
                        level.Paint();
                    }
                    PluginTransaction.Commit();
                }
            }
        }
    }
}
