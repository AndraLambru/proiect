using BEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BUtilities;
using MySql.Data.MySqlClient;

namespace BDataBase
{

    public class OperationsDB
    {
        
        public static List<Bag> Read()
        {
            if (BConstants.DEBUG) 
                return Dummy.Read();

            List<Bag> listBag = ReadBags();
            List<Pic> listPic = ReadPics();

            foreach (Bag b in listBag)
                b.pictures = listPic.Where(p => p.id_bag == b.id).ToList();
            return listBag;
        }

        public static List<Bag> ReadBags()
        {
            List<Bag> list = new List<Bag>();
            string sql = "SELECT * FROM bag";
            MySql.Data.MySqlClient.MySqlConnection conn= new MySql.Data.MySqlClient.MySqlConnection(BConstants.CONNECTION_MYSQL);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    //Console.WriteLine(rdr[0] + " -- " + rdr[1]);
                    list.Add(
                        new Bag()
                        {
                            id = (int)rdr["id"],
                            name = (string)rdr["name"],
                            description = (string)rdr["description"],
                            price = (float)rdr["price"]
                        }
                    );
                }

            }
            catch (MySql.Data.MySqlClient.MySqlException ex) { }
           
            finally
            {
                if (conn != null || conn.State == System.Data.ConnectionState.Open)
                     conn.Close();
            }

            return list;
        }

        public static List<Pic> ReadPics()
        {
            List<Pic> list = new List<Pic>();
            string sql = "SELECT * FROM pic";
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(BConstants.CONNECTION_MYSQL);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    //Console.WriteLine(rdr[0] + " -- " + rdr[1]);
                    list.Add(
                        new Pic()
                        {
                            id = (int)rdr["id"],
                            name = rdr["name"] is DBNull ? "" : (string)rdr["name"],
                            url = rdr["url"] is DBNull? "" : (string)rdr["url"],
                            id_bag = (int)rdr["id_bag"],
                            guid = rdr["guid"] is DBNull ? "" : (string)rdr["guid"],
                            rank = (int)rdr["rank"],
                        }
                    );
                }

            }
            catch (MySql.Data.MySqlClient.MySqlException ex) { }
            
            finally
            {
                if (conn != null || conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }

            return list;
        }
        public static bool Write()
        {
            return true;
        }

        public static bool WriteBag(string name, string description, float price,ref int id)
        {
            string sql = "INSERT INTO bag(NAME, DESCRIPTION, PRICE) VALUES (@name, @description, @price)";
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(BConstants.CONNECTION_MYSQL);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@price", price);

                bool b = cmd.ExecuteNonQuery() > 0;

                sql = "SELECT LAST_INSERT_ID()";
                cmd = new MySqlCommand(sql, conn);

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    id = Convert.ToInt32(result);
                }
                return b;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) { }

            finally
            {
                if (conn != null || conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }

            return false;
        }

        public static bool WritePic(int id_bag, string name, int rank, ref int id)
        {
            string sql = "INSERT INTO pic(NAME, ID_BAG, RANK) VALUES (@name, @idbag, @rank)";
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(BConstants.CONNECTION_MYSQL);
            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@idbag", id_bag);
                cmd.Parameters.AddWithValue("@rank", rank);

                bool b = cmd.ExecuteNonQuery() > 0;

                sql = "SELECT LAST_INSERT_ID()";
                cmd = new MySqlCommand(sql, conn);

                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    id = Convert.ToInt32(result);
                }
                return b;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex) { }

            finally
            {
                if (conn != null || conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }

            return false;
        }


        public class Classes
        {
            public static List<Class> Get()
            {
                List<Class> listC = ReadClasses();
                List<Student> listS = ReadStudents();

                if (listC != null)
                    foreach (Class c in listC)
                        if (listS != null)
                            c.Students = listS.Where(s => s.classid == c.id).ToList();
                return listC;
            }

            public static List<Class> ReadClasses()
            {
                List<Class> list = new List<Class>();
                string sql = "SELECT * FROM class";
                MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(BConstants.CONNECTION_MYSQL);
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        //Console.WriteLine(rdr[0] + " -- " + rdr[1]);
                        list.Add(
                            new Class()
                            {
                                id = (int)rdr["id"],
                                code = (string)rdr["code"]
                            }
                        );
                    }

                }
                catch (MySql.Data.MySqlClient.MySqlException ex) { }

                finally
                {
                    if (conn != null || conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }

                return list;
            }

            public static List<Student> ReadStudents()
            {
                List<Student> list = new List<Student>();
                string sql = "SELECT * FROM student";
                MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(BConstants.CONNECTION_MYSQL);
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        //Console.WriteLine(rdr[0] + " -- " + rdr[1]);
                        list.Add(
                            new Student()
                            {
                                id = (int)rdr["id"],
                                name = (string)rdr["name"],
                                classid = (int)rdr["classid"]
                            }
                        );
                    }

                }
                catch (MySql.Data.MySqlClient.MySqlException ex) { }

                finally
                {
                    if (conn != null || conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }

                return list;
            }

            public static bool Delete(int id, int tid)
            {
                string sql = "";
                switch (tid)
                {
                    case 0:
                        sql = "DELETE from class where id=" + id.ToString();
                        break;
                    case 1:
                        sql = "DELETE from student where id=" + id.ToString();
                        break;
                    default:
                        break;
                }
                if (string.IsNullOrEmpty(sql))
                    return false;
                MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(BConstants.CONNECTION_MYSQL);
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    

                    return cmd.ExecuteNonQuery() > 0;
                    
                }
                catch (MySql.Data.MySqlClient.MySqlException ex) { return false; }

                finally
                {
                    if (conn != null || conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }
            }

            public static int Add(int tid, string name, int classid)
            {
                string sql = "";
                int id = 0;
                switch (tid)
                {
                    case 0:
                        sql = "insert into class(code) values('" + name+"')";
                        break;
                    case 1:
                        sql = "insert into student(name,classid) values('" + name + "', " + classid.ToString() + ")";
                        break;
                    default:
                        break;
                }
                if (string.IsNullOrEmpty(sql))
                    return 0;

                MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(BConstants.CONNECTION_MYSQL);
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    bool b = cmd.ExecuteNonQuery() > 0;

                    sql = "SELECT LAST_INSERT_ID()";
                    cmd = new MySqlCommand(sql, conn);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        id = Convert.ToInt32(result);
                    }
                    return id;
                }
                catch (MySql.Data.MySqlClient.MySqlException ex) { return 0; }

                finally
                {
                    if (conn != null || conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }

            }


            public static int Modify(int id, int tid, string name)
            {
                string sql = "";
                switch (tid)
                {
                    case 0:
                        sql = "update class set code='" + name + "' where id="+id.ToString();
                        break;
                    case 1:
                        sql = "update student set name='" + name + "' where id="+id.ToString();
                        break;
                    default:
                        break;
                }
                if (string.IsNullOrEmpty(sql))
                    return 0;

                MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(BConstants.CONNECTION_MYSQL);
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    return cmd.ExecuteNonQuery();

                    
                }
                catch (MySql.Data.MySqlClient.MySqlException ex) { return 0; }

                finally
                {
                    if (conn != null || conn.State == System.Data.ConnectionState.Open)
                        conn.Close();
                }
                return 1;
            }
        }


        public class Dummy
        {
            public static List<Bag> Read()
            {
                List<Bag> list = new List<Bag>();

                Bag b1 = new Bag()
                {
                    id = 0,
                    name = "First Bag",
                    description = "descriptiooon",
                    price = 450,
                    pictures = new List<Pic>()
                    {
                        new Pic()
                        {
                            id=1,
                            guid=Guid.NewGuid().ToString(),
                            id_bag=0,
                            name="Geanta21.jpg",
                            rank=0,
                            url=""
                        },
                        new Pic()
                        {
                            id=2,
                            guid=Guid.NewGuid().ToString(),
                            id_bag=0,
                            name="Geanta22.jpg",
                            rank=1,
                            url=""
                        },
                        new Pic()
                        {
                            id=3,
                            guid=Guid.NewGuid().ToString(),
                            id_bag=0,
                            name="Geanta23.jpg",
                            rank=2,
                            url=""
                        },
                        new Pic()
                        {
                            id=4,
                            guid=Guid.NewGuid().ToString(),
                            id_bag=0,
                            name="Geanta24.jpg",
                            rank=3,
                            url=""
                        }
                    }
                };

                Bag b2 = new Bag()
                {
                    id = 1,
                    name = "Second Bag",
                    description = "descriptiooon",
                    price = 550,
                    pictures = new List<Pic>()
                    {
                        new Pic()
                        {
                            id=5,
                            guid=Guid.NewGuid().ToString(),
                            id_bag=1,
                            name="Geanta31.jpg",
                            rank=0,
                            url=""
                        },
                        new Pic()
                        {
                            id=6,
                            guid=Guid.NewGuid().ToString(),
                            id_bag=1,
                            name="Geanta32.jpg",
                            rank=1,
                            url=""
                        },
                         new Pic()
                        {
                            id=7,
                            guid=Guid.NewGuid().ToString(),
                            id_bag=1,
                            name="Geanta33.jpg",
                            rank=2,
                            url=""
                        },
                         new Pic()
                        {
                            id=8,
                            guid=Guid.NewGuid().ToString(),
                            id_bag=1,
                            name="Geanta34.jpg",
                            rank=3,
                            url=""
                        }
                    }
                };

                list.Add(b1);
                list.Add(b2);

                return list;
            }

            public static bool Write()
            {
                return true;
            }

        }
    }
}
