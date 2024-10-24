using System.Collections;
using System.Data.SqlClient;

namespace demo_part2.Models
{
    public class get_claims
    {
        public ArrayList email { get; set; } = new ArrayList();
        public ArrayList module { get; set; } = new ArrayList();
        public ArrayList id { get; set; } = new ArrayList();
        public ArrayList hours { get; set; } = new ArrayList();
        public ArrayList rate { get; set; } = new ArrayList();
        public ArrayList note { get; set; } = new ArrayList();
        public ArrayList total { get; set; } = new ArrayList();
        public ArrayList status { get; set; } = new ArrayList();
        public ArrayList filename { get; set; } = new ArrayList();

        connection connect = new connection();

        public get_claims()
        {
            try
            {
                using (SqlConnection connects = new SqlConnection(connect.connecting()))
                {
                    connects.Open();
                    using (SqlCommand prepare = new SqlCommand("SELECT user_id, email, module, hours, rate, note, total, status, files FROM claiming", connects))
                    {
                        using (SqlDataReader reader = prepare.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                email.Add(reader["email"].ToString());
                                module.Add(reader["module"].ToString());
                                id.Add(reader["user_id"].ToString());
                                hours.Add(reader["hours"].ToString());
                                rate.Add(reader["rate"].ToString());
                                note.Add(reader["note"].ToString());
                                total.Add(reader["total"].ToString());
                                status.Add(reader["status"].ToString());
                                filename.Add(reader["files"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
