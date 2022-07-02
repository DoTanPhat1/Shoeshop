using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shoeshop.Models;

namespace DAweb.Controllers
{
    public class UserController : Controller
    {
        dbQLShoeshopDataContext db = new dbQLShoeshopDataContext();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        // GET: User
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KHACHHANG kh)
        {
            //gán giá trị vào form
            var hoten = collection["Hoten"];
            var tendn = collection["TenDN"];
            var matkhau = collection["MatKhau"];
            var matkhaunhaplai = collection["MatKhauNhapLai"];
            var email = collection["Email"];
            var diachi = collection["DiaChi"];
            var dienthoai = collection["DienThoai"];
            var ngaysinh = string.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);
            if (string.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên không được để trống";
            }
            if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Tài khoản không được để trống";
            }
            if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Mật khẩu không được để trống";
            }

            if (string.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi4"] = "Mật khẩuu nhập lại không được để trống";
            }
            if (string.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Email không được để trống";
            }
            if (string.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi6"] = "Điện thoại không được để trống";
            }
            else
            {
                kh.HoTen = hoten;
                kh.TaiKhoan = tendn;
                kh.MatKhau = matkhau;
                kh.Email = email;
                kh.DiachiKH = diachi;
                kh.DienThoaiKH = dienthoai;
                kh.NgaySinh = DateTime.Parse(ngaysinh);
                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Dangnhap");
            }


            return this.Dangky();
        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["MatKhau"];
            if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Tên đăng nhập không để trống";
            }
            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Nhập mật khẩu vào";
            }
            else
            {
                //gan gia tri va lay session
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == tendn && n.MatKhau == matkhau);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Bạn đã đăng nhập thành công";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Index", "Shoeshop");
                }
                else
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
    }
}