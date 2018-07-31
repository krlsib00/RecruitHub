using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace SampleDataGenerator
{
    class AthletesDataGenerator
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();

            var userId = new int[] { 101, 111, 112 }; //Random ids
            var dateOfBirth = new DateTime[] { new DateTime(1994, 1, 18), new DateTime(1995, 8, 18), new DateTime(1996, 6, 18), };
            var birthPlace = new string[] { "LA, California", "Houston, Texas", "Encino, California" };
            var schoolId = new int[] { 1, 2, 3 };
            var sportsLevelId = new int[] { 1, 2, 3 };
            var educationLevelId = new int[] { 1, 2, 3 };
            var classYearId =
                Enumerable.Range(0, 100)
                .Select(n => rnd.Next(2010, 2025))
                .ToArray();
            var graduationYear = new int[] { 2018, 2019, 2020 };
            var shortBio =
                Enumerable.Range(0, 1000)
                .Select(n => "tester1" + rnd.Next(1, 10000) + "@gmail.com")
                .ToArray();

            using (var con = new SqlConnection("server = 13.64.246.7; database = C54_RecruitHub; user id = C54_User; password = Sabiopass1!54"))
            {
                con.Open();

                var cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Athletes_Create";

                cmd.Parameters.Add("@UserId", SqlDbType.Int);
                cmd.Parameters.Add("@DOB", SqlDbType.DateTime);
                cmd.Parameters.Add("@BirthPlace", SqlDbType.NVarChar);
                cmd.Parameters.Add("@SchoolId", SqlDbType.Int);
                cmd.Parameters.Add("@SportsLevelId", SqlDbType.Int);
                cmd.Parameters.Add("@EducationLevelId", SqlDbType.Int);
                cmd.Parameters.Add("@ClassYearId", SqlDbType.Int);
                cmd.Parameters.Add("@GraduationYear", SqlDbType.Int);
                cmd.Parameters.Add("@ShortBio", SqlDbType.NVarChar);

                cmd.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.Prepare();

                foreach (var userid in userId)
                {
                    foreach (var dob in dateOfBirth)
                    {
                        foreach (var birthplace in birthPlace)
                        {
                            foreach (var schoolid in schoolId)
                            {
                                foreach (var sportslevelid in sportsLevelId)
                                {
                                    foreach (var educationlevelid in educationLevelId)
                                    {
                                        foreach (var classyearid in classYearId)
                                        {
                                            foreach (var graduationyear in graduationYear)
                                            {
                                                foreach (var shortbio in shortBio)
                                                {
                                                    {
                                                        Console.WriteLine($"{userid}, {schoolid}, {dob}, {birthplace}, {schoolid}, {sportslevelid}, {educationlevelid}, {classyearid}, {graduationyear}, {shortbio}");
                                                        cmd.Parameters["@UserId"].Value = userid;
                                                        cmd.Parameters["@DOB"].Value = dob;
                                                        cmd.Parameters["@BirthPlace"].Value = birthplace;
                                                        cmd.Parameters["@SchoolId"].Value = schoolid;
                                                        cmd.Parameters["@SportsLevelId"].Value = sportslevelid;
                                                        cmd.Parameters["@EducationLevelId"].Value = educationlevelid;
                                                        cmd.Parameters["@ClassYearId"].Value = classyearid;
                                                        cmd.Parameters["@GraduationYear"].Value = graduationyear;
                                                        cmd.Parameters["@ShortBio"].Value = shortbio;

                                                        cmd.ExecuteNonQuery();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

