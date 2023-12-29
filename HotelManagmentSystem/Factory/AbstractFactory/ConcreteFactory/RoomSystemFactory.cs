using HotelManagmentSystem.Factory.AbstractFactor;
using HotelManagmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagmentSystem.Factory.AbstractFactory
{
    public class RoomSystemFactory
    {
        public IRoomFactory Create(tbl_room e)
        {
            IRoomFactory returnvalue = null;
            if (e.room_type_id == 1)
            {
                if (e.tbl_room_type.room_name == "Single")
                {
                    returnvalue = new NormalFactorySingle();
                }
               else if (e.tbl_room_type.room_name == "Double")
                {
                    returnvalue = new NormalFactoryDouble();
                }
                else if (e.tbl_room_type.room_name == "Triple")
                {
                    returnvalue = new NormalFactoryTriple();
                }
                else
                {
                    returnvalue = new NormalFactoryQuad(); ;
                }
            }
            else if (e.room_type_id == 2)
            {
                if (e.tbl_room_type.room_name == "MasterSuite")
                {
                    returnvalue = new SuiteFactoryMasterSuite();
                }
                else
                {
                    returnvalue = new SuiteFactoryJuniorSuite(); ;
                }
            }
            else if (e.room_type_id == 3)
            {
                if (e.tbl_room_type.room_name == "King")
                {
                    returnvalue = new LuxuryFactoryKing();
                }
                else
                {
                    returnvalue = new LuxuryFactoryKing(); ;
                }
            }
            return returnvalue;

        }
    }
}