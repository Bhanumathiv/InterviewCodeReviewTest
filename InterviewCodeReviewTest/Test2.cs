using System;
using System.Data.SqlClient;

namespace InterviewCodeReviewTest
{
	public class Test2
	{
		// Record customer purchase and update customer reward programme
		public Result UpdateCustomerHistory(Purchase customerPurchase)
		{
			
			/*use a config file to configure the Database connection properties,do not hardcode in code 
			Read connectionstring  from config file using Configuration class. 
			using (string connStr = ConfigurationManager. ConnectionStrings["connection"])
			{
				opening a new sqlconnection by using "using" keyword will automatically manage the connection (closing connections)
			}*/
			var connPruchase = new SqlConnection("data source=TestPurchaseServer;initial catalog=PurchaseDB;Trusted_Connection=True");
			var connReward = new SqlConnection("data source=TestRewardServer;initial catalog=RewardDB;Trusted_Connection=True");

			var cmdPurchase = new SqlCommand("INSERT INTO dbo.Purchase..."); // omitted the columns
			var cmdReward = new SqlCommand("INSERT INTO dbo.Reward..."); // omitted the columns

			SqlTransaction tranPurchase = null;
			SqlTransaction tranReward = null;

			try
			{
				/* Seoarating two try-catch blocks for sql connection opener and sql command execution will separate the 2 different exceptions.
			or any outer exception handling for all of the sql operations with an internal exception handling for sql command execution also works
			ex :
			try{
			--code for sqlconnection opener
				try {
				--sql querying code--
				}
				
				catch(SQLExeption ex){
				--catch actions--
				}
			catch(Exception ex){
			--handling sql connection exception--
			}
			*/
				connPruchase.Open();
				tranPurchase = connPruchase.BeginTransaction();
				cmdPurchase.ExecuteNonQuery();

				connReward.Open();
				tranReward = connReward.BeginTransaction();
				cmdReward.ExecuteNonQuery();

				tranPurchase.Commit();
				tranReward.Commit();

				return Result.Success();
			}
			catch (Exception ex)
			{
				tranPurchase.Rollback();
				tranReward.Rollback();

				return Result.Failed();
			}
		}
	}

	public class Purchase
	{
		// Some members
	}

	public class Result
	{
		//IsSuccessful can be made private as teh value is not supposed to be changed externally, get & set mothods will give success & failure values.
		public bool IsSuccessful { get; private set; }

		public static Result Success()
		{
			return new Result { IsSuccessful = true };
		}

		public static Result Failed()
		{
			return new Result { IsSuccessful = false };
		}
	}
}
