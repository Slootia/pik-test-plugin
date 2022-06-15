using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using PikTestPlugin.Models;
using PikTestPlugin.Views;
using System;

namespace PikTestPlugin
{
    [Transaction(TransactionMode.Manual)]
    public sealed class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                RevitStaticData.Initialize(commandData.Application);
                Painter painter = new Painter();
                painter.Paint(new ApartmentComplex().Initialize(RevitStaticData.FilteredSpatialElements));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorWindow errorWindow = new ErrorWindow(new ErrorModel(e.Message));
                errorWindow.Show();
                return Result.Failed;
            }

            return Result.Succeeded;
        }
    }
}