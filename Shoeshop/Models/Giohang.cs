using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoeshop.Models
{
    public class Giohang
    {
        // tao them 1 đối tượng để quản lý csdl:
        dbQLShoeshopDataContext data = new dbQLShoeshopDataContext();
        public int iMagiay { get; set; }
        public string sTengiay { get; set; }
        public string sAnhbia { get; set; }
        public double dDongia { get; set; }
        public int iSoluong { get; set; }
        public double dThanhtien
        {
            get { return iSoluong * dDongia; }

        }
        public Giohang(int Magiay)
        {
            iMagiay = Magiay;
            GIAY giay = data.GIAYs.Single(n => n.Magiay == iMagiay);
            sTengiay = giay.Tengiay;
            sAnhbia = giay.AnhBia;
            dDongia = double.Parse(giay.GiaBan.ToString());
            iSoluong = 1;

        }

    }
}