using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagmentSystem.Factory.AbstractFactor
{
    public interface IRoomFactory
    {
        IRoomTypeServiesPrice RoomTypeServiesPrice();
        IRoomTypeServies RoomTypeServies();
        IRoomType RoomType();

    }
}
