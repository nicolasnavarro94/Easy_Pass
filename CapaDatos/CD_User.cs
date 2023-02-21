using CapaModelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public static class CD_User
    {
        private static User user;

        public static int userExist(out string message)
        {
            message = null;
            using (var db = new easyPassEntities())
            {
                try
                {
                    return db.User.Where(c => c.username == Environment.MachineName).Count();
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    return 0;
                }
            }
        }

        public static bool createUser(string pass, out string message)
        {
            message = null;
            using (var db = new easyPassEntities())
            {
                try
                {
                    user = new User();
                    user.username = Environment.MachineName;
                    user.password = pass;
                    db.User.Add(user);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    return false;
                }
            }
        }

        public static int userLogin(string pass, out string message)
        {
            message = null;
            using (var db = new easyPassEntities())
            {
                try
                {
                    return db.User.Where(c => c.username == Environment.MachineName).Where(c=>c.password == pass).Count();
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    return 0;
                }
            }
        }
    }
}
