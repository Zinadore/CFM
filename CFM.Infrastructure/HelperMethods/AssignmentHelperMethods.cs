using System;
using System.Text;
using CFM.Data.Models;

namespace CFM.Infrastructure.HelperMethods
{
    public static class AssignmentHelperMethods
    {
        public static int DaysLeft(this Assignment assignment)
        {
            return (assignment.Deadline - DateTime.Now).Days;
        }

        public static string ShortDescription(this Assignment assignment)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(assignment.Title);
            sb.Append(" | ");
            sb.Append(assignment.Unit.Title);
            sb.Append(" | ");
            sb.Append(assignment.DaysLeft());
            sb.Append(" days left");
            return sb.ToString();
        }
    }
}
