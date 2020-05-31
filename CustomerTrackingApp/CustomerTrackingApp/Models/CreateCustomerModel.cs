using CustomerTrackingApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Models
{
	public class CreateCustomerModel
	{
		public string Fullname { get; set; }
		public int Phone { get; set; }
	}
}
