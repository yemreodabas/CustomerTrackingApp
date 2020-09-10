using CustomerTrackingApp.Entities;
using CustomerTrackingApp.Enums;
using CustomerTrackingApp.Persistence.EF;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerTrackingApp.Persistence.EF
{
	public class LogRepository : BaseEFRepository, ILogRepository
	{
		public void Log(LogType type, string message)
		{
			using (SQLiteDBContext dbConnection = this.OpenConnection())
			{
				Log tempLog = null;

				tempLog.Message = message;
				tempLog.Type = type;
				tempLog.Timestamp = DateTime.UtcNow.Ticks;

				dbConnection.Log.Add(tempLog);
				dbConnection.SaveChanges();

				//dbConnection.Execute("INSERT INTO Log (Type, Message, Timestamp) VALUES(@Type, @Message, @Timestamp)", new { Type = type, Message = message, Timestamp = DateTime.UtcNow.Ticks });
			}
		}
	}
}
