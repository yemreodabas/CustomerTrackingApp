using CustomerTrackingApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace CustomerTrackingApp.Models
{
	public class CreateActivityModel
	{
		public string Description { get; set; }
		public decimal Amount { get; set; }
		public int UserId { get; set; }
		public int CustomerId { get; set; }
		public ActivityType ActivityType { get; set; }
		public decimal CurrentDept { get; set; }
	}
}
