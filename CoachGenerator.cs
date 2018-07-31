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
    public static class CoachGenerator
    {
        static Random rnd = new Random();
        static string[] titles = new string[] { "Head Coach", "Vice Coach", "Sub Coach" };
        static int[] schoolIds = new int[] { 1, 2, 3, 4, 5 };
        static string[] shortBios = new string[] { "I coach balls", "I coach to hit balls", "I am the mother of all coaches", "I pitch like that coach" };
        static string[] roles = new string[] { "Hitting Coach", "Pitching Coach", "Head Coach" };
        static string[] conferencesWithin10Yrs = new string[] { "Yes", "No" };
        static string[] conferences = new string[] {
            "ACC",
            "SEC",
            "Big 12",
            "Pac 12",
            "Conference USA",
            "American",
            "Big Ten",
            "Big West",
            "Sun Belt",
            "Missouri Valley",
            "WCC",
            "SouthLand",
            "Mountain West",
            "CAA",
            "Atlantic Sun",
            "Big East",
            "Big South",
            "SoCon",
            "Horizon",
            "America East",
            "Ohio Valley",
            "Summit League",
            "Mid-American",
            "Atlantic 10",
            "Ivy League",
            "Northeast",
            "Patriot League",
            "WAC",
            "MAAC",
            "MEAC",
            "SWAC",
        };

        public static void CreateCoach(SqlConnection con, int userId)
        {
            var title = titles[rnd.Next(0, titles.Length - 1)];
            var schoolId = schoolIds[rnd.Next(0, schoolIds.Length - 1)];
            var shortBio = shortBios[rnd.Next(0, shortBios.Length - 1)];

            var cmd = con.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "CoachProfiles_Create";

            cmd.Parameters.Add("@UserId", SqlDbType.Int);
            cmd.Parameters.Add("@Title", SqlDbType.NVarChar);
            cmd.Parameters.Add("@SchoolId", SqlDbType.Int);
            cmd.Parameters.Add("@ShortBio", SqlDbType.NVarChar);

            cmd.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;

            cmd.Prepare();

            Console.WriteLine($"{userId}, {title}, {schoolId}, {shortBio}");

            cmd.Parameters["@UserId"].Value = userId;
            cmd.Parameters["@Title"].Value = title;
            cmd.Parameters["@SchoolId"].Value = schoolId;
            cmd.Parameters["@ShortBio"].Value = shortBio;

            cmd.ExecuteNonQuery();

            var json =
                    new JObject(
                        new JProperty("UserId", userId),
                        new JProperty("CoachesStats", new JObject(
                            new JProperty("Division", rnd.Next(1, 3)),
                            new JProperty("Conference", conferences[rnd.Next(0, conferences.Length)]),
                            new JProperty("AverageGameWon", rnd.NextDouble().ToString("0.00")),
                            new JProperty("YearsInPlayOffs", rnd.Next(0, 10)),
                            new JProperty("Roles", roles[rnd.Next(0, roles.Length)]),
                            new JProperty("ConferenceWithin10Yrs", conferencesWithin10Yrs[rnd.Next(0, conferencesWithin10Yrs.Length)]),
                            new JProperty("YearsInPlayoffs", rnd.Next(0, 15))

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
