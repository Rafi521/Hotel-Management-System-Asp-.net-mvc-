using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagmentSystem.Factory.AbstractFactor
{
    public class Enumeretions
    {
        public enum RoomType
        {
            Normal,
            Suite,
            Luxury
        }
        public enum RoomTypeServies
        {
            Single,
            Double,
            Triple,
            Quad,
            MasterSuite,
            JuniorSuite,
            Queen,
            King,

        }

        public enum RoomTypeServiesPrice
        {
          RS1000,
          RS2000,
          RS3000,
          RS4000,
          RS6000,
          RS7000,
          RS10000,
          RS8000


        }
    }
}