using System.Data.SqlClient;
using System.Runtime.ConstrainedExecution;

namespace demo_part2.Models
{
    public class claim
    {
        public string user_email { get; set; }
        public string user_id { get; set; }
        public string hours_worked { get; set; }
        public string rate { get; set; }
  
       
        public string description { get; set; }

        //connection
        connection connect= new connection();

        
        public string insert_claim(string module,string hour_work,string rate, string note, string filename)
        {
            //temp variable message
            string message = "";
            string user_ID = get_id();
            string user_EMAIL = get_email();
            string total = "" + (int.Parse(hour_work) * int.Parse(rate));

            string query = "insert into claiming values('" + user_EMAIL + "', '" + module + "','" + user_ID + "', '" + hour_work + "','" + rate + "','" + note + "','none','none', '" + total + "','"+filename+"','pending');";

            try
            {
                using (SqlConnection connects = new SqlConnection(connect.connecting()))
                {
                    connects.Open();
                    using(SqlCommand done = new SqlCommand(query, connects))
                    {
                        done.ExecuteNonQuery();
                        message = "done";
                    }
                    connects.Close();
                }

            } 
            catch(IOException error)
            {
                message = error.Message;
            }
            return message;
        }
        public string get_id()
        {
            //hold id variable 
            string hold_id = "";

            try
            {
                using (SqlConnection connects = new SqlConnection(connect.connecting()))
                {
                    connects.Open();
                    using (SqlCommand prepare = new SqlCommand("select * from active", connects))
                    {
                        using (SqlDataReader getID = prepare.ExecuteReader())
                        {
                            if (getID.HasRows)
                            {
                                //check all but get one
                                while(getID.Read())
                                {
                                    //then get it
                                    hold_id = getID["id"].ToString();
                                }
                            }
                            getID.Close();
                        }
                    }

                        connects.Close();
                }

            } catch(IOException error)
            {
                Console.WriteLine(error.Message);
                hold_id = error.Message;
        
    }
            return hold_id;
        }
        public string get_email()
        {
            //hold id variable 
            string hold_email = "";

            try
            {
                using (SqlConnection connects = new SqlConnection(connect.connecting()))
                {
                    connects.Open();
                    using (SqlCommand prepare= new SqlCommand("select * from active", connects))
                    {
                        using (SqlDataReader getemail = prepare.ExecuteReader())
                        {
                            if (getemail.HasRows)
                            {
                                //check all but get one
                                while (getemail.Read())
                                {
                                    //then get it
                                    hold_email = getemail["email"].ToString();
                                }
                            }
                           getemail.Close();

                        }
                    }

                    connects.Close();
                }

            }
            catch (IOException error)
            {
                Console.WriteLine(error.Message);
                hold_email = error.Message;

            }
            return hold_email;
        }

    }
}
