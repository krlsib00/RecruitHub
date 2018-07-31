using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDataGenerator
{
    public static class AdvocateGenerator
    {
        static Random rnd = new Random();
        static string[] titles = new string[] { "Famous Advocate", "Whatever Advocate", "No No Advocate" };
        static int[] schoolIds = /*Utils.GetIdsFromTable("Schools");*/ new int[] { 1, 2, 3, 4, 5 };
        static string[] shortBios = new string[] { "I advocate balls", "I advocate to hit balls", "I am the mother of all advocates", "I pitch like that advocate" };

        public static void CreateAdvocate(SqlConnection con, int userId)
        {
            var title = titles[rnd.Next(0, titles.Length - 1)];
            var schoolId = schoolIds[rnd.Next(0, schoolIds.Length - 1)];
            var shortBio = shortBios[rnd.Next(0, shortBios.Length - 1)];            

                var cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "AdvocateProfiles_Create";

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
            }
        }
    }