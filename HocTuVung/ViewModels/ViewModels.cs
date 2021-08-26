using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace HocTuVung.ViewModels
{
    using Models;

    public class UnitViewModel
    {
        public UnitViewModel()
        {

        }

        public IList<Unit> GetList()
        {
            IList<Unit> units = new List<Unit>();
            using (TuVungContext context = new TuVungContext())
            {
                units = context.Units.ToList();
            }
            return units;
        }

        public object Create(string name)
        {
            using (TuVungContext context = new TuVungContext())
            {
                try
                {
                    context.Units.Add(new Unit() { Name = name });
                    context.SaveChanges();
                }
                catch
                {
                    return "Unit này đã có";
                }
            }
            return null;
        }

        public object Update(int Id, string name)
        {
            using (TuVungContext context = new TuVungContext())
            {
                try
                {
                    var tmp = context.Units.SingleOrDefault(item => item.Id == Id);
                    if (tmp != null)
                    {
                        tmp.Name = name;
                        context.SaveChanges();
                    }
                }
                catch
                {
                    return "Unit này đã có";
                }
            }
            return null;
        }

        public object Delete(int Id)
        {
            using (TuVungContext context = new TuVungContext())
            {
                try
                {
                    var tmp = context.Units.SingleOrDefault(item => item.Id == Id);
                    if (tmp != null)
                    {
                        context.Units.Remove(tmp);
                        context.SaveChanges();
                    }
                }
                catch
                {
                    return "Không thể xóa dữ liệu này";
                }
            }
            return null;
        }
    }

    public class VocabViewModel
    {
        public VocabViewModel()
        {

        }

        public IList<Vocab> GetList(int unit_id)
        {
            IList<Vocab> units = new List<Vocab>();
            using (TuVungContext context = new TuVungContext())
            {
                units = (from x in context.Vocabs
                         where x.UnitId == unit_id
                         select x).ToList();
            }
            return units;
        }

        public object Create(int unit_id, string english, string vietnam, string audio)
        {
            using (TuVungContext context = new TuVungContext())
            {
                try
                {
                    context.Vocabs.Add(new Vocab() { English = english, Vietnam = vietnam, UnitId = unit_id, Audio = audio });
                    context.SaveChanges();
                }
                catch
                {
                    return "Từ này đã có";
                }
            }
            return null;
        }

        public object Edit(int vocab_id, string english, string vietnam)
        {
            using (TuVungContext context = new TuVungContext())
            {
                try
                {
                    var vocab = context.Vocabs.SingleOrDefault(item => item.Id == vocab_id);
                    if (vocab != null)
                    {
                        vocab.English = english;
                        vocab.Vietnam = vietnam;
                        context.SaveChanges();
                    }
                }
                catch
                {
                    return "Từ này đã có";
                }
            }
            return null;
        }

        public object Delete(int vocab_id)
        {
            using (TuVungContext context = new TuVungContext())
            {
                try
                {
                    var tmp = context.Vocabs.SingleOrDefault(item => item.Id == vocab_id);
                    if (tmp != null)
                    {
                        context.Vocabs.Remove(tmp);
                        context.SaveChanges();
                    }
                }
                catch
                {
                    return "Không thể xóa dữ liệu này";
                }
            }
            return null;
        }
    }
}
