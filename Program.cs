/*
* Author: PECman
* Date: 2018/3/18
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using MySql.Data.MySqlClient;

namespace ConsoleApplication1
{

    namespace Test
    {
        public class MysqlConnector
        {
            string server = null;
            string userid = null;
            string password = null;
            string database = null;
            string port = "3306";
            string charset = "utf-8";

            public MysqlConnector() { }
            public MysqlConnector SetServer(string server)
            {
                this.server = server;
                return this;
            }

            public MysqlConnector SetUserID(string userid)
            {
                this.userid = userid;
                return this;
            }

            public MysqlConnector SetDataBase(string database)
            {
                this.database = database;
                return this;
            }

            public MysqlConnector SetPassword(string password)
            {
                this.password = password;
                return this;
            }
            public MysqlConnector SetPort(string port)
            {
                this.port = port;
                return this;
            }
            public MysqlConnector SetCharset(string charset)
            {
                this.charset = charset;
                return this;
            }



            #region  建立MySql数据库连接
            /// <summary>
            /// 建立数据库连接.
            /// </summary>
            /// <returns>返回MySqlConnection对象</returns>
            private MySqlConnection GetMysqlConnection()
            {
                string M_str_sqlcon = string.Format("server={0};user id={1};password={2};database={3};port={4};Charset={5}", server, userid, password, database, port, charset);
                MySqlConnection myCon = new MySqlConnection(M_str_sqlcon);
                return myCon;
            }
            #endregion

            #region  执行MySqlCommand命令
            /// <summary>
            /// 执行MySqlCommand
            /// </summary>
            /// <param name="M_str_sqlstr">SQL语句</param>
            public void ExeUpdate(string M_str_sqlstr)
            {
                try
                {
                    MySqlConnection mysqlcon = this.GetMysqlConnection();
                    mysqlcon.Open();
                    MySqlCommand mysqlcom = new MySqlCommand(M_str_sqlstr, mysqlcon);
                    mysqlcom.ExecuteNonQuery();
                    mysqlcom.Dispose();
                    mysqlcon.Close();
                    mysqlcon.Dispose();
                }
                catch(MySqlException e)
                {
                    Console.WriteLine(e.ToString());
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            #endregion

            #region  创建MySqlDataReader对象
            /// <summary>
            /// 创建一个MySqlDataReader对象
            /// </summary>
            /// <param name="M_str_sqlstr">SQL语句</param>
            /// <returns>返回MySqlDataReader对象</returns>
            public MySqlDataReader ExeQuery(string M_str_sqlstr)
            {
                Console.WriteLine(M_str_sqlstr);
                MySqlConnection mysqlcon = this.GetMysqlConnection();
                MySqlCommand mysqlcom = new MySqlCommand(M_str_sqlstr, mysqlcon);
                mysqlcon.Open();
                MySqlDataReader mysqlread = mysqlcom.ExecuteReader(CommandBehavior.CloseConnection);
                return mysqlread;
            }
            #endregion
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var mc = new Test.MysqlConnector();
            mc.SetServer("127.0.0.1")
          .SetDataBase("test")  //选择数据库
          .SetUserID("root")    //输入用户名
          .SetPassword("root")  //输入用户密码
          .SetPort("3306")      //设置端口号
          .SetCharset("utf8");  //设置字符集
            Console.WriteLine("********_ Welcome to PEC-Ta _********");
            while (true)
            {
                Console.WriteLine("Enter A for adding words, enter D for dictating, enter E for exiting:");
                switch (Console.ReadLine())
                {
                    case "A": while (addWords(mc)) ; Console.WriteLine(); break;
                    case "D": dictate(mc); break;
                    case "E": return;
                    default: Console.WriteLine("Ouch!Wrong letter!"); break;
                }
            }
        }
        static bool addWords(Test.MysqlConnector mc)
        {
            Console.Write("Please enter a chinese word: ");
            var cn = Console.ReadLine();
            Console.Write("\nPlease enter a french word: ");
            var fr = Console.ReadLine();
            mc.ExeUpdate(string.Format("INSERT INTO words ( Chinese, French ) VALUES( \"{0}\", \"{1}\" );", cn, fr));
            Console.Write("Would you like to continue typing?(Y or N)");
            return Console.ReadKey().ToString() == "Y";

        }
        static void dictate(Test.MysqlConnector mc)
        {
            var reader = mc.ExeQuery("SELECT * FROM words");
            var ans = "";
            var wrong = 0;
            var right = 0;
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (i % 2 != 0)
                    {
                        Console.Write(string.Format("Which word means {0} in French?\n"), reader.GetValue(i));
                        ans = Console.ReadLine();
                    }
                    else
                    {
                        if (ans == reader.GetValue(i).ToString())
                        {
                            Console.WriteLine("Bingo!You did a good job!");
                            right++;
                        }
                        else
                        {
                            Console.WriteLine("What a pity!The correct answer is ", reader.GetValue(i));
                            wrong++;
                        }
                    }
                }
            }
            Console.WriteLine(string.Format("You mispelt {0} word(s) and you spelt {1} word(s) correctly!", wrong, right));
            Console.ReadKey();
        }
    }
}
