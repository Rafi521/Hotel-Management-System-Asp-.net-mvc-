using HotelManagmentSystem.Factory.AbstractFactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagmentSystem.Factory.AbstractFactory
{
    public class SuiteFactoryMasterSuite : IRoomFactory
    {
        public IRoomType RoomType()
        {
            return new Suite();
        }

        public virtual IRoomTypeServies RoomTypeServies()
        {
            return new MasterSuite();
        }

        public virtual IRoomTypeServiesPrice RoomTypeServiesPrice()
        {
            return new RS10000();
        }
    }
    public class SuiteFactoryJuniorSuite : SuiteFactoryMasterSuite
    {
      
        public override IRoomTypeServies RoomTypeServies()
        {
            return new JuniorSuite();
        }

        public override IRoomTypeServiesPrice RoomTypeServiesPrice()
        {
            return new RS8000();
        }
    }
}