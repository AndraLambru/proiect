using BDataBase;
using BEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BUtilities;

namespace BBusiness
{
    public class Operations
    {
        public static List<Bag> Read(float priceMin,float priceMax)
        {
            List<Bag> list = OperationsDB.Read();
            list = list.Where(b => b.price >= priceMin && b.price <= priceMax).ToList();
            SetURL(list);
            return list;
        }

        public static bool WriteBag(string name, string description,float price,ref int id)
        {
            return OperationsDB.WriteBag(name, description, price,ref id);
        }

        private static void SetURL(List<Bag> list)
        {
            if (list == null)
                return;

           foreach(Bag b in list)
            if(b.pictures!=null)
                foreach(Pic p in b.pictures)
                {
                    p.url =BConstants.HANDLERPIC + "?name=" + p.name;
                }

        }

        public static bool WritePic(int id_bag, string name, int rank, ref int id)
        {
            return OperationsDB.WritePic(id_bag, name, rank, ref id);
        }
    }


    public class Operations2
    {
        public static List<Class> Get()
        {
            return OperationsDB.Classes.Get();
        }
        public static bool Delete(int id, int tid){
            return OperationsDB.Classes.Delete(id, tid);
        }
        public static int Add(int tid, string name, int classid){
            return OperationsDB.Classes.Add(tid, name, classid);
        }
        public static int Modify(int id, int tid, string name)
        {
            return OperationsDB.Classes.Modify(id, tid, name);
        }
    }
}
