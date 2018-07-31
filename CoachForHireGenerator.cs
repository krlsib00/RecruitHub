using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDataGenerator
{
    public static class CoachForHireGenerator
    {
        static Random rnd = new Random();
        static string[] coachTypes = new string[] { "Pitching Coach", "Hitting Coach", "Vice Coach" };

        public static void CreateCoachForHire(SqlConnection con, int userId)
        {
            var cmd = con.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "CoachForHire_Create";

            cmd.Parameters.Add("@UserId", SqlDbType.Int);

            cmd.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;

            cmd.Prepare();

            Console.WriteLine($"{userId}");

            cmd.Parameters["@UserId"].Value = userId;

            cmd.ExecuteNonQuery();

            var json =
                new JObject(
                    new JProperty("UserId", userId),
                    new JProperty("CoachForHire", new JObject(
                        new JProperty("StartingRate", rnd.Next(10, 35)),
                        new JProperty("YearsExperience", rnd.Next(0, 20)),
                        new JProperty("CoachType", coachTypes[rnd.Next(0, coachTypes.Length)])

                        ))).ToString(Newtonsoft.Json.Formatting.None);

            var cmd2 = con.CreateCommand();
            cmd2.CommandType = System.Data.CommandType.StoredProcedure;
            cmd2.CommandText = "PersonalStats_Create";

            cmd2.Parameters.Add("@StatsJson", SqlDbType.NVarChar);

            Console.WriteLine($"{json}");

            cmd2.Parameters["@StatsJson"].Value = json;

            cmd2.ExecuteNonQuery();
        }
    }
}
