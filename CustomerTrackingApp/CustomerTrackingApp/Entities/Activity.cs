﻿using CustomerTrackingApp.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Entities
{
	public class Activity
	{
		[Key]
		public int Id { get; set; }
		public int UserId { get; set; }
		public decimal Amount { get; set; }
		public int CustomerId { get; set; }
		public decimal CurrentDept { get; set; }
		public string Description { get; set; }
		public ActivityType ActivityType { get; set; }
	}
}
