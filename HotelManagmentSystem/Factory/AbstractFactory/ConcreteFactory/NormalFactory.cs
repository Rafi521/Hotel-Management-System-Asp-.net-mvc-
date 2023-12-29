using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagmentSystem.Factory.AbstractFactor
{
    public class NormalFactorySingle : IRoomFactory
    {
        public IRoomType RoomType()
        {
            return new Normal();
        }

        public virtual IRoomTypeServies RoomTypeServies()
        {
            return new Single();
        }

        public virtual IRoomTypeServiesPrice RoomTypeServiesPrice()
        {
            return new RS1000();
        }
    }
    public class NormalFactoryDouble : NormalFactorySingle
    {
   

        public override IRoomTypeServies RoomTypeServies()
        {
            return new Double();
        }

        public override IRoomTypeServiesPrice RoomTypeServiesPrice()
        {
            return new RS2000();
        }
    }
    public class NormalFactoryTriple : NormalFactorySingle
    {
    
        public override IRoomTypeServies RoomTypeServies()
        {
            return new Triple();
        }

        public override IRoomTypeServiesPrice RoomTypeServiesPrice()
        {
            return new RS3000();
        }
    }

    public class NormalFactoryQuad : NormalFactorySingle
    {

        public override IRoomTypeServies RoomTypeServies()
        {
            return new Quad();
        }

        public override IRoomTypeServiesPrice RoomTypeServiesPrice()
        {
            return new RS4000();
        }
    }
}