using CustomerTrackingApp.Entities;
using CustomerTrackingApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Models
{
	public class ActivityModel
	{
		public int Id { get; set; }
		public string Description { get; set; }
		public decimal Amount { get; set; }
		public int UserId { get; set; }
		public int CustomerId { get; set; }
		public ActivityType ActivityType { get; set; }
		public decimal CurrentDept { get; set; }

		public ActivityModel() { }

		public ActivityModel(Activity activity)
		{
			this.Id = activity.Id;
			this.Description = activity.Description;
			this.Amount = activity.Amount;
			this.UserId = activity.UserId;
			this.CustomerId = activity.CustomerId;
			this.CurrentDept = activity.CurrentDept;
			this.ActivityType = activity.ActivityType;
		}
	}
}
