using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static HotelManagmentSystem.Factory.AbstractFactor.Enumeretions;

namespace HotelManagmentSystem.Factory.AbstractFactor
{
    public class RS1000 : IRoomTypeServiesPrice
    {
        public string GetRoomTypeServiesPrice()
        {
            return RoomTypeServiesPrice.RS1000.ToString();
        }
    }
    public class RS2000 : IRoomTypeServiesPrice
    {
        public string GetRoomTypeServiesPrice()
        {
            return RoomTypeServiesPrice.RS2000.ToString();
        }
    }
    public class RS3000 : IRoomTypeServiesPrice
    {
        public string GetRoomTypeServiesPrice()
        {
            return RoomTypeServiesPrice.RS3000.ToString();
        }
    }
    public class RS4000 : IRoomTypeServiesPrice
    {
        public string GetRoomTypeServiesPrice()
        {
            return RoomTypeServiesPrice.RS4000.ToString();
        }
    }
    public class RS6000 : IRoomTypeServiesPrice
    {
        public string GetRoomTypeServiesPrice()
        {
            return RoomTypeServiesPrice.RS6000.ToString();
        }
    }
    public class RS7000 : IRoomTypeServiesPrice
    {
        public string GetRoomTypeServiesPrice()
        {
            return RoomTypeServiesPrice.RS7000.ToString();
        }
    }
    public class RS10000 : IRoomTypeServiesPrice
    {
        public string GetRoomTypeServiesPrice()
        {
            return RoomTypeServiesPrice.RS10000.ToString();
        }
    }
    public class RS8000 : IRoomTypeServiesPrice
    {
        public string GetRoomTypeServiesPrice()
        {
            return RoomTypeServiesPrice.RS8000.ToString();
        }
    }
}