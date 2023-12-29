using HotelManagmentSystem.Factory.AbstractFactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagmentSystem.Factory.AbstractFactory
{
    public class Room
    {
        IRoomFactory _IRoomFactory;
        
        public Room(IRoomFactory iroomfactory)
        {
            _IRoomFactory = iroomfactory;

        }
        public string GetRoom()
        {
            IRoomTypeServies RoomTypeServies = _IRoomFactory.RoomTypeServies();
            IRoomType RoomType = _IRoomFactory.RoomType();
            IRoomTypeServiesPrice RoomTypeServiesPrice = _IRoomFactory.RoomTypeServiesPrice();
            string returnvalue = string.Format("{0} {1} {2}", RoomType.GetRoomType(), RoomTypeServies.GetRoomServies(), RoomTypeServiesPrice.GetRoomTypeServiesPrice());
            return returnvalue;
        }
    }
}