using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using PikTestPlugin.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PikTestPlugin
{
    [Transaction(TransactionMode.Manual)]
    public sealed class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                RevitStaticData.Document = commandData.Application.ActiveUIDocument.Document;

                var spatialElementsFromModel =
                    new FilteredElementCollector(RevitStaticData.Document).OfClass(typeof(SpatialElement)).Cast<SpatialElement>().ToList();

                var rooms = GetSpatialElementsByParameterAndPurpose(spatialElementsFromModel, "ROM_Зона", "Квартира");

                ApartmentComplex.Initialize(rooms);
                var sections = ApartmentComplex.Sections;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failed;
            }

            return Result.Succeeded;
        }

        private static List<SpatialElement> GetSpatialElementsByParameterAndPurpose(List<SpatialElement> spatialElements, string parameterName, string purpose)
        {
            return spatialElements
                .Where(s => s.GetParameters(parameterName).ToList().Any(p => p.AsString().Contains(purpose))).ToList();
        }
    }
}
