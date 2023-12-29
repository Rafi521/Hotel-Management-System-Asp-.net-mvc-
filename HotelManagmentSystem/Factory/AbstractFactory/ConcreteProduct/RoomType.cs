using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static HotelManagmentSystem.Factory.AbstractFactor.Enumeretions;

namespace HotelManagmentSystem.Factory.AbstractFactor
{
    public class Normal : IRoomType
    {
        public string GetRoomType()
        {
            return RoomType.Normal.ToString();
        }
    }
    public class Suite : IRoomType
    {
        public string GetRoomType()
        {
            return RoomType.Suite.ToString();
        }
    }

    public class Luxury : IRoomType
    {
        public string GetRoomType()
        {
            return RoomType.Luxury.ToString();
        }
    }

}