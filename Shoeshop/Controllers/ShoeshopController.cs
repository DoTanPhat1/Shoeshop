using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shoeshop.Models;
using PagedList;
using PagedList.Mvc;

namespace WEBBANGIAY.Controllers
{
    public class ShoeshopController : Controller
    {
        // tao them 1 đối tượng để quản lý csdl:
        dbQLShoeshopDataContext data = new dbQLShoeshopDataContext();

        private List<GIAY> Laygiaymoi(int count)
        {
            return data.GIAYs.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
        // GET: Shoeshop
        public ActionResult Index(int ? page, string searchString)
        {
            ViewBag.Keyword = searchString;
            //Tao bien quy dinh so luong sp/trang
            int pageSize = 6;
            //Tao bien so trang
            int pageNum = (page ?? 1);

           // var giaymoi = Laygiaymoi(21);
            // return View(giaymoi.ToPagedList(pageNum,pageSize));
            return View(GIAY.GetAll(searchString).ToPagedList(pageNum, pageSize));


        }
        public ActionResult Thuonghieu()
        {
            var thuonghieu = from cd in data.THUONGHIEUs select cd;
            return PartialView(thuonghieu);
        }
        public ActionResult Loaigiay()
        {
            var loaigiay = from cd in data.LOAIGIAYs select cd;
            return PartialView(loaigiay);
        }
        public ActionResult GiayTheoThuongHieu(int id)
        {
            var giay = from g in data.GIAYs where g.MaTH == id select g;
            return View(giay);
        }
        public ActionResult GiayTheoLoai(int id)
        {
            var giay = from g in data.GIAYs where g.Maloai == id select g;
            return View(giay);
        }
        public ActionResult Details(int id)
        {
            var giay = from g in data.GIAYs where g.Magiay == id select g;
            return View(giay.Single());
        }
    }
}