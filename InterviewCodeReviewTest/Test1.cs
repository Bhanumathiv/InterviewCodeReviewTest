using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace InterviewCodeReviewTest
{
	public class Test1
	{
		// Called by web API and returns list of strongly typed customer address for given status
		// CustomerAddress is populated by external import and could be dirty
		public IEnumerable<Address> GetCustomerNumbers(string status)
		{
			/*use a config file to configure the Database connection properties,do not hardcode in code 
			Read connectionstring  from config file using Configuration class. 
			using (string connStr = ConfigurationManager. ConnectionStrings["connection"])
			{
				opening a new sqlconnection by using "using" keyword will automatically manage the connection (closing connections)
			}*/
			var connection = new SqlConnection("data source=TestServer;initial catalog=CustomerDB;Trusted_Connection=True");
			var cmd = new SqlCommand($"SELECT CustomerAddress FROM dbo.Customer WHERE Status = '{status}'", connection);

			try
			{
				var addressStrings = new List<string>();
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
				connection.Open();
				var reader = cmd.ExecuteReader();

				while (reader.Read())
				{
					addressStrings.Add(reader.GetString(0));
				}

				return addressStrings
					.Select(StringToAddress)
					.Where(x => x != null)
					.ToList();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private static Address StringToAddress(string addressString)
		{
			return new Address(addressString);
		}
	}

	public class Address
	{
		// Some members...

		public Address(string addressString)
		{
			// Assume there are logic here to parse address and return strongly typed object
		}
	}
}
