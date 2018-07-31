using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SampleDataGenerator
{
    static class UserIdDataGenerator          
    {
        static Random rnd = new Random();
        static string[] firstNames = new string[] { "Robert", "Hannah", "Jessica", "Terry", "Paul", "Aaron", "Daniel" };
        static string[] lastNames = new string[] { "Mackie", "Montana", "McKlein", "Adams", "Jackson", "Ojai", "Michelson" };
        static int[] userTypeIds = new int[] { 1, 2, 3, 4, 5 };
        static string[] passwords = new string[] { "auto_generated_user" };
        static bool[] subscribeToNewsLetter = new bool[] { false, true };

        static string GenerateEmail()
        {
            return "test-" + Guid.NewGuid().ToString("N") + "@mailator.com";
        }

        static string GenerateAvatarUrl()
        {
            return "https://api.adorable.io/avatars/128/" + Guid.NewGuid().ToString("N") + "@adorable.io.png";
        }

        static void Main(string[] args)
        {               

            using (var con = new SqlConnection(Utils.ConnectionString))
            {
                con.Open();

                var cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Users_Create_Sample";

                cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar);
                cmd.Parameters.Add("@LastName", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar);
                cmd.Parameters.Add("@UserTypeId", SqlDbType.Int);
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar);
                cmd.Parameters.Add("@AvatarUrl", SqlDbType.NVarChar);
                cmd.Parameters.Add("@SubscribeToNewsLetter", SqlDbType.Bit);

                cmd.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.Prepare();
                List<int> result = new List<int>();

                for (var i = 0; i < 10000; i++)
                {
                    while (true)
                    {     
                        var firstname = firstNames.PickRandom();
                        var lastname = lastNames[rnd.Next(0, lastNames.Length - 1)];
                        var email = GenerateEmail();
                        var avatarurl = GenerateAvatarUrl();
                        var usertypeid = userTypeIds[rnd.Next(0, userTypeIds.Length - 1)];
                        var password = passwords[rnd.Next(0, passwords.Length - 1)];
                        var subscribetonewsletter = subscribeToNewsLetter[rnd.Next(0, subscribeToNewsLetter.Length - 1)];

                        Console.WriteLine($"{firstname}, {lastname}, {email}, {usertypeid}, {subscribetonewsletter}, {avatarurl}"); 

                        cmd.Parameters["@FirstName"].Value = firstname;
                        cmd.Parameters["@LastName"].Value = lastname;
                        cmd.Parameters["@Email"].Value = email;
                        cmd.Parameters["@UserTypeId"].Value = usertypeid;
                        cmd.Parameters["@Password"].Value = password;
                        cmd.Parameters["@AvatarUrl"].Value = avatarurl;
                        cmd.Parameters["@SubscribeToNewsLetter"].Value = subscribetonewsletter;

                        try
                        {
                            cmd.ExecuteNonQuery();

                            int id = (int)cmd.Parameters["@Id"].Value;

                            if (usertypeid == 1)
                                AthleteGenerator.CreateAthlete(con, id);
                            else if (usertypeid == 2)
                                CoachGenerator.CreateCoach(con, id);
                            else if (usertypeid == 3)
                                AdvocateGenerator.CreateAdvocate(con, id);
                            else if (usertypeid == 4)
                                AthleticDirectorGenerator.CreateAthleticDirector(con, id);                            
                            else if (usertypeid == 5)
                                CoachForHireGenerator.CreateCoachForHire(con, id);

                            break;
                        }
                        catch (SqlException ex) when (ex.Number == 2627)
                        {

                        }
                    }
                }
            }
        }
    }
}


