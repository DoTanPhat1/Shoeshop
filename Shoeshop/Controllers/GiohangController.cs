using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shoeshop.Models;

namespace WEBBANGIAY.Controllers
{
    public class GioHangController : Controller
    {
        // tao them 1 đối tượng để quản lý csdl:
        dbQLShoeshopDataContext data = new dbQLShoeshopDataContext();
        // GET: Giohang
        public List<Giohang> LayGiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            //Neu ton tai han vao gio hang 
            if (lstGiohang == null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        public ActionResult ThemGiohang(int iMagiay, string strURL)
        {
            List<Giohang> lstGiohang = LayGiohang();
            //Kiem tra giay da chon vao gio hang chua 
            Giohang sanpham = lstGiohang.Find(n => n.iMagiay == iMagiay);
            if (sanpham == null)
            {
                sanpham = new Giohang(iMagiay);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }
        //Tong so luong
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoluong);
            }
            return iTongSoLuong;
        }
        //Tinh tong tien
        private double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhtien);
            }

            return iTongTien;
        }
        //Tạo giỏ hàng
        public ActionResult Giohang()
        {
            List<Giohang> lstGiohang = LayGiohang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Shoeshop");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }
        //Xoa gio hang
        public ActionResult XoaGiohang(int iMaSP)
        {
            List<Giohang> lstGiohang = LayGiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMagiay == iMaSP);
            if (lstGiohang != null)
            {
                lstGiohang.RemoveAll(n => n.iMagiay == iMaSP);
                return RedirectToAction("Giohang");
            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Shoeshop");
            }
            return RedirectToAction("Giohang");
        }

        //Xóa tất cả giỏ hàng
        public ActionResult XoaTatCaGioHang()
        {
            //lấy giỏ hàng từ session
            List<Giohang> lstGiohang = LayGiohang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "Shoeshop");
        }
        //Cap nhat gio hang
        public ActionResult CapnhatGiohang(int iMaSP, FormCollection f)
        {
            List<Giohang> lstGiohang = LayGiohang();
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMagiay == iMaSP);
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());

            }
            return RedirectToAction("Giohang");
        }
        [HttpGet]
        public ActionResult Dathang()
        {
            //Kiểm tra việc đăng nhập
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "User");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "Shoeshop");
            }
            //Lấy giỏ hàng
            List<Giohang> lstGiohang = LayGiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }
        public ActionResult Dathang(FormCollection collection)
        {
            //Thêm đơn hàng
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<Giohang> gh = LayGiohang();
            ddh.MaKH = kh.MaKH;
            ddh.Ngaydat = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.Ngaygiao = DateTime.Parse(ngaygiao);
            ddh.TinhTrangGiao = false;
            ddh.DaThanhToan = false;
            data.DONDATHANGs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            //Thêm chi tiết đơn hàng
            foreach (var item in gh)
            {
                CHITIETDONDATHANG ctdh = new CHITIETDONDATHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.Magiay = item.iMagiay;
                ctdh.Soluong = item.iSoluong;
                ctdh.Dongia = (decimal)item.dDongia;
                data.CHITIETDONDATHANGs.InsertOnSubmit(ctdh);

            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}