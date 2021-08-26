using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace HocTuVung.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Unit : INotifyPropertyChanged
    {
        private int _id;
        [Key]
        public int Id
        {
            get { return _id; }
            set { _id = value; this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Id")); }
        }

        private string _name;
        [Required]
        public string Name
        {
            get { return _name; }
            set { _name = value; this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name")); }
        }

        [NotMapped]
        public int STT { get; set; }

        public ICollection<Vocab> Vocabs { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    [Index(nameof(UnitId), nameof(English), IsUnique = true)]
    public class Vocab : INotifyPropertyChanged
    {
        private int _id;
        [Key]
        public int Id
        {
            get { return _id; }
            set { _id = value; this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Id")); }
        }

        private string _english;
        [Required]
        public string English
        {
            get { return _english; }
            set { _english = value; this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("English")); }
        }

        private string _vietnam;
        [Required]
        public string Vietnam
        {
            get { return _vietnam; }
            set { _vietnam = value; this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Vietnam")); }
        }

        private string _audio;
        //Dùng base64
        public string Audio
        {
            get { return _audio; }
            set { _audio = value; this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Audio")); }
        }

        [NotMapped]
        public int STT { get; set; }

        private int _unitid;
        [Required]
        public int UnitId
        {
            get { return _unitid; }
            set { _unitid = value; this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UnitId")); }
        }
        
        public Unit Unit { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public Vocab Clone()
        {
            return new Vocab()
            {
                STT = STT,
                Id = Id,
                English = English,
                Vietnam = Vietnam,
                UnitId = UnitId,
                Unit = Unit,
                Audio = Audio,
            };
        }
    }

    internal class TuVungContext : DbContext
    {
        //Dùng factory design pattern
        private static bool _iscreated = false;

        public TuVungContext() : base()
        {
            if (!_iscreated)
            {
                _iscreated = true;
                //this.Database.EnsureDeleted();
                this.Database.EnsureCreated();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=tuvung.sqlite3;");
        }

        public DbSet<Unit> Units { get; set; }
        public DbSet<Vocab> Vocabs { get; set; }
    }
}
