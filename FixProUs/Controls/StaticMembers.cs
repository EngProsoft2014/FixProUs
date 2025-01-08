using FFImageLoading.Work;
using FixProUs.Models;
using System;
using System.Collections.Generic;
using System.Text;


namespace FixProUs.Controls
{
    public class StaticMembers
    {
        public static DateTime SelectedDate { get; set; } = DateTime.Now;
        public static string StaticDate { get; set; }
        public static int EmployeesPages { get; set; }
        public static int WayAfterChooseCust { get; set; } = 0; //Create New Schedule (NewSchedulePage)
        public static int WayCreateCust { get; set; } = 0; //Create New Cust from CustomerPage // 1= Create New Cust from CallPage
        public static int ScheduleIdStatic { get; set; } = 0;
        public static int ScheduleDateIdStatic { get; set; } = 0;
        public static string OldProfileImageSt { get; set; }
        public static int PayCashOrCredit { get; set; }
        public static ImageSource AccountImg { get; set; }
        public static int CreateOrDetailsCall { get; set; }
        public static CallModel FilterCallModel { get; set; }
        public static int YesOrNoInternet { get; set; } = 0;
    }
}
