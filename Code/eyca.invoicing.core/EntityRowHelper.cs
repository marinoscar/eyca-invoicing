using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eyca.invoicing.core
{
    public static class EntityRowHelper
    {
        public static void ZeroRowEmployee()
        {
            if (CheckForZero("Employee")) return;
            var sql = "INSERT INTO Employee VALUES (0, 'NOT FOUND', 'EMPLOYEE NOT FOUND', 'oscar.marin@cr.ey.com', 'NOT FOUND', 'NOT FOUND');";
            var db = Helper.GetDb();
            db.ExecuteNonQuery(sql);
        }

        public static void ZeroRowProject()
        {
            if (CheckForZero("Project")) return;
            var sql = "INSERT INTO Project (Id, Code) VALUES (0, 'NOT FOUND');";
            var db = Helper.GetDb();
            db.ExecuteNonQuery(sql);
        }

        public static bool CheckForZero(string tableName)
        {
            var sql = string.Format("SELECT COUNT(*) FROM {0} WHERE Id = 0", tableName);
            var db = Helper.GetDb();
            var result = db.ExecuteScalarOr<int>(sql, 0);
            return result > 0;
        }

        public static long GetNextId(string tableName)
        {
            var sql = string.Format("SELECT MAX(Id) FROM {0}", tableName);
            var db = Helper.GetDb();
            var result = db.ExecuteScalarOr<long>(sql, 0) + 1;
            return result;
        }

    }
}
