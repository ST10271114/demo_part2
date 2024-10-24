using System.Data.SqlClient;
namespace demo_part2.Models

{
    public class check_login
    {


        public string email { get; set; }

        public string role { get; set; }

        public string password { get; set; }

        //connection
        connection connect = new connection();

        //method to check the user

        public string login_user(string emails, string roles, string password)
        {
            //temp message
            string message = "";
            Console.WriteLine(emails + "and" + password);
            try
            {
                //connect
                using (SqlConnection connects = new SqlConnection(connect.connecting()))
                {
                    //oprn connection
                    connects.Open();

                    //query
                    string query = "select * from users where email='" + emails + "' and password='" + password + "';";

                    //execute 
                    using (SqlCommand prepare = new SqlCommand(query, connects))
                    {

                        using (SqlDataReader find_user = prepare.ExecuteReader())
                        {
                            //then check if the use is found
                            if (find_user.HasRows)
                            {
                                //then assign message
                                message = "found";


                            }
                            else
                            {
                                message = "not";
                            }
                        }
                    }
                    connects.Close();
                    if (message == "found")
                    {
                        update_active(emails);
                    }
                }



            }
            catch (IOException error_db)
            {

                //return error
                message = error_db.Message;
            }
            return message;
        }

        //update active method
        public void update_active(string emails)
        {
            try
            {
                using (SqlConnection connects = new SqlConnection(connect.connecting()))
                {
                    connects.Open();

                    string query = "update active set email='" + emails + "'";

                    using (SqlCommand done = new SqlCommand(query, connects))
                    {

                        done.ExecuteNonQuery();
                    }

                    connects.Close();

                }

            }
            catch (IOException error)
            {
                Console.WriteLine("error" + error.Message);
            }
        }






    }
}