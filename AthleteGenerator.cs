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
    public static class AthleteGenerator
    {
        static Random rnd = new Random();
        static DateTime[] dobs = new DateTime[] { new DateTime(1994, 1, 18), new DateTime(1995, 8, 18), new DateTime(1996, 6, 18) };
        static string[] birthPlaces = new string[] { "LA, California", "Houston, Texas", "Encino, California" };
        static int[] schoolIds = new int[] { 1, 2, 3, 4, 5 };
        static int[] educationLevelIds = new int[] { 1, 2, 3, 4, 5 };
        static int[] classYearIds = new int[] { 1, 2, 3, 4, 5 };
        static int[] graduationYears = new int[] { 2018, 2019, 2020, 2021, 2022 };
        static string[] shortBios = new string[] { "I throw balls", "I hit balls", "I am the mother of all balls", "I pitch like hell" };
        static string[] handednesses = new string[] { "Right", "Left", "Ambi" };

        public static void CreateAthlete(SqlConnection con, int userId)
        {
            var dob = dobs[rnd.Next(0, dobs.Length - 1)];
            var birthPlace = birthPlaces[rnd.Next(0, birthPlaces.Length - 1)];
            var schoolId = schoolIds[rnd.Next(0, schoolIds.Length - 1)];
            var educationLevelId = educationLevelIds[rnd.Next(0, educationLevelIds.Length - 1)];
            var classYearId = classYearIds[rnd.Next(0, classYearIds.Length - 1)];
            var graduationYear = graduationYears[rnd.Next(0, graduationYears.Length - 1)];
            var shortBio = shortBios[rnd.Next(0, shortBios.Length - 1)];

            var cmd = con.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "Athletes_Create";

            cmd.Parameters.Add("@UserId", SqlDbType.Int);
            cmd.Parameters.Add("@DOB", SqlDbType.DateTime);
            cmd.Parameters.Add("@BirthPlace", SqlDbType.NVarChar);
            cmd.Parameters.Add("@SchoolId", SqlDbType.Int);
            cmd.Parameters.Add("@EducationLevelId", SqlDbType.Int);
            cmd.Parameters.Add("@ClassYearId", SqlDbType.Int);
            cmd.Parameters.Add("@GraduationYear", SqlDbType.Int);
            cmd.Parameters.Add("@ShortBio", SqlDbType.NVarChar);

            cmd.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;

            cmd.Prepare();

            Console.WriteLine($"{userId}, {schoolId}, {dob}, {birthPlace}, {educationLevelId}, {classYearId}, {graduationYear}, {shortBio}");

            cmd.Parameters["@UserId"].Value = userId;
            cmd.Parameters["@DOB"].Value = dob;
            cmd.Parameters["@BirthPlace"].Value = birthPlace;
            cmd.Parameters["@SchoolId"].Value = schoolId;
            cmd.Parameters["@EducationLevelId"].Value = educationLevelId;
            cmd.Parameters["@ClassYearId"].Value = classYearId;
            cmd.Parameters["@GraduationYear"].Value = graduationYear;
            cmd.Parameters["@ShortBio"].Value = shortBio;

            cmd.ExecuteNonQuery();

            var json =
                    new JObject(
                            new JProperty("UserId", userId),
                            new JProperty("Education", new JObject(
                                new JProperty("GPA", (rnd.NextDouble() * 5).ToString("0.00")),
                                new JProperty("SAT", rnd.Next(400, 1600)),
                                new JProperty("ACT", rnd.Next(4, 144))
                            )),                            

                            rnd.Next(0, 2) == 0
                            ?
                            new JProperty("Hitter", new JObject(
                                new JProperty("Handedness", handednesses[rnd.Next(0, handednesses.Length)]),
                                new JProperty("HighSchoolBattingAverage", rnd.NextDouble().ToString("0.00")),
                                new JProperty("ExtraBaseHits", rnd.Next(0, 100)),
                                new JProperty("RBIs", rnd.Next(0, 250)),
                                new JProperty("OnBasePercentage", rnd.NextDouble().ToString("0.00")),
                                new JProperty("StolenBases", rnd.Next(0, 2500))
                            ))
                            :
                            new JProperty("Pitcher", new JObject(
                                new JProperty("Handedness", handednesses[rnd.Next(0, handednesses.Length)]),
                                new JProperty("ERA", rnd.Next(0, 300)),
                                new JProperty("InningsPitcher", rnd.Next(0, 300)),
                                new JProperty("Strikeouts", rnd.Next(0, 30)),
                                new JProperty("Walks", rnd.Next(0, 30))

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
