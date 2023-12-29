using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static HotelManagmentSystem.Factory.AbstractFactor.Enumeretions;

namespace HotelManagmentSystem.Factory.AbstractFactor
{
    public class Single : IRoomTypeServies
    {
        public string GetRoomServies()
        {
            return RoomTypeServies.Single.ToString();
        }
    }
}