using HotelManagmentSystem.Factory.AbstractFactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagmentSystem.Factory.AbstractFactory
{
    public class LuxuryFactoryKing : IRoomFactory
    {
        public IRoomType RoomType()
        {
            return new Luxury();
        }

        public virtual IRoomTypeServies RoomTypeServies()
        {
            return new King();
        }

        public virtual IRoomTypeServiesPrice RoomTypeServiesPrice()
        {
            return new RS7000();
        }
    }
    public class LuxuryFactoryQueen: LuxuryFactoryKing
    {
        public override IRoomTypeServies RoomTypeServies()
        {
            return new Queen();
        }

        public override IRoomTypeServiesPrice RoomTypeServiesPrice()
        {
            return new RS6000();
        }
    }
}