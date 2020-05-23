using CustomerTrackingApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Persistence
{
	public interface ILogRepository
	{
		void Log(LogType type, string message);
	}
}
