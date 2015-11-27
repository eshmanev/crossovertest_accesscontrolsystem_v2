using AccessControl.Contracts.Helpers;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace AccessControl.FunctionalTest.Rows
{
    [Binding]
    public class Transformations
    {
        [StepArgumentTransformation]
        public AccessPoint Row(Table table)
        {
            var row = table.CreateInstance<AccessPointRow>();
            return new AccessPoint(row.AccessPointId, row.Site, row.Department, row.Name) {Description = row.Description};
        }
    }
}