using ServiceCommission.Models.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;


namespace ServiceCommission.Models
{
    [Serializable]
    public abstract class Entity : IEntity
    {
        int? _requestedHashCode;
        Guid _Id;
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

        public Entity()
        {
            _Id = Guid.NewGuid();
            CreateAt = DateTime.Now;
            UpdateAt = DateTime.Now;
        }


        [Key]
        public virtual Guid Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (value != Guid.Empty) _Id = value;
                else _Id = Guid.NewGuid();
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            Entity item = (Entity)obj;

            if (item.Id == Guid.Empty || this.Id == Guid.Empty)
                return false;
            else
                return item.Id == this.Id;
        }

        public override int GetHashCode()
        {
            if (Id != Guid.Empty)
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = this.Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();

        }


    }
}
