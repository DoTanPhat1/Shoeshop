using Shoeshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO;


namespace Shoeshop.Controllers
{
    public class AdminController : Controller
    {
        //Tao dt de quan ly data
        dbQLShoeshopDataContext db = new dbQLShoeshopDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Giay(int? page, string searchString)
        {
            ViewBag.Keyword = searchString;
            int pageNumber = (page ?? 1);
            int pageSize = 5;

            return View(GIAY.GetAll(searchString).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ThuongHieu()
        {

            return View(db.THUONGHIEUs.ToList());
        }
        public ActionResult LoaiGiay()
        {

            return View(db.LOAIGIAYs.ToList());
        }




        // GET: Admin

        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
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
                ADMIN admin  = db.ADMINs.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (admin != null)
                {
                    ViewBag.ThongBao = "Bạn đã đăng nhập thành công";
                    Session["Taikhoan"] = admin;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        [HttpGet]
        public ActionResult ThemmoiGiay()
        {
            ViewBag.MaTH = new SelectList(db.THUONGHIEUs.ToList().OrderBy(n => n.TenThuongHieu), "MaTH", "TenThuongHieu");
            ViewBag.Maloai = new SelectList(db.LOAIGIAYs.ToList().OrderBy(n => n.Tenloai), "Maloai", "Tenloai");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiGiay(GIAY giay, HttpPostedFileBase fileupload)
        {
            ViewBag.MaTH = new SelectList(db.THUONGHIEUs.ToList().OrderBy(n => n.TenThuongHieu), "MaTH", "TenThuongHieu");
            ViewBag.Maloai = new SelectList(db.LOAIGIAYs.ToList().OrderBy(n => n.Tenloai), "Maloai", "Tenloai");
            if (fileupload==null)
            {
                ViewBag.Thongbao = "Chọn hình ảnh vào";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {

                    //Them ten file
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Lưu đường dẫn
                    var path = Path.Combine(Server.MapPath("~/product_imgs"), fileName);
                    //Kiem tra hinh anh tồn tại chưa 
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    giay.AnhBia = fileName;
                    //Lưu file
                    db.GIAYs.InsertOnSubmit(giay);
                    db.SubmitChanges();
                
                }
                return RedirectToAction("GIAY");
            }

        }


        //Hiển thị sản phẩm
        public ActionResult ChitietGiay(int id)
        {
            //Lấy ra đối tượng giày theo mã giày
            GIAY giay = db.GIAYs.SingleOrDefault(n => n.Magiay == id);
            ViewBag.Magiay = giay.Magiay;
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(giay);
        }
        [HttpGet]
        public ActionResult XoaGiay(int id)
        {
            //Lấy ra đối tượng giày theo mã giày
            GIAY giay = db.GIAYs.SingleOrDefault(n => n.Magiay == id);
            ViewBag.Magiay = giay.Magiay;
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(giay);
        }
        [HttpPost, ActionName("XoaGiay")]
        public ActionResult Xacnhanxoa(int id)
        {
            GIAY giay = db.GIAYs.SingleOrDefault(n => n.Magiay == id);
            ViewBag.Magiay = giay.Magiay;
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.GIAYs.DeleteOnSubmit(giay);
            db.SubmitChanges();
            return RedirectToAction("Giay");
        }

        [HttpGet]
        public ActionResult XoaThuongHieu(int id)
        {
            //Lấy ra đối tượng giày theo mã giày
           THUONGHIEU  thuonghieu = db.THUONGHIEUs.SingleOrDefault(n => n.MaTH == id);
            ViewBag.MaTH = thuonghieu.MaTH;
            if (thuonghieu == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(thuonghieu);
        }
        [HttpPost, ActionName("XoaThuongHieu")]
        public ActionResult XacnhanxoaTH(int id)
        {
            THUONGHIEU thuonghieu = db.THUONGHIEUs.SingleOrDefault(n => n.MaTH == id);
            ViewBag.MaTH = thuonghieu.MaTH;
            if (thuonghieu == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.THUONGHIEUs.DeleteOnSubmit(thuonghieu);
            db.SubmitChanges();
            return RedirectToAction("ThuongHieu");
        }

        [HttpGet]
        public ActionResult XoaLoaiGiay(int id)
        {
            //Lấy ra đối tượng giày theo mã giày
            LOAIGIAY loaigiay = db.LOAIGIAYs.SingleOrDefault(n => n.Maloai == id);
            ViewBag.Maloai = loaigiay.Maloai;
            if (loaigiay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(loaigiay);
        }
        [HttpPost, ActionName("XoaLoaiGiay")]
        public ActionResult XacnhanxoaLG(int id)
        {
            LOAIGIAY loaigiay = db.LOAIGIAYs.SingleOrDefault(n => n.Maloai == id);
            ViewBag.MaTH = loaigiay.Maloai;
            if (loaigiay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.LOAIGIAYs.DeleteOnSubmit(loaigiay);
            db.SubmitChanges();
            return RedirectToAction("LOAIGIAY");
        }
        [HttpGet]
        public ActionResult SuaGiay(int id)
        {
            if (Session["Taikhoan"] == null)
            {
                return RedirectToAction("login", "Admin");
            }
            else
            {
                GIAY giay = db.GIAYs.SingleOrDefault(n => n.Magiay == id);
                //Lay du liệu tư table Chude để đổ vào Dropdownlist, kèm theo chọn MaCD tương tưng 
                ViewBag.MaTH = new SelectList(db.THUONGHIEUs.ToList().OrderBy(n => n.TenThuongHieu), "MaTH", "TenThuongHieu");
                ViewBag.Maloai = new SelectList(db.LOAIGIAYs.ToList().OrderBy(n => n.Tenloai), "Maloai", "Tenloai");
               
                    return View(giay);
            }
        }

        [HttpPost]
        public ActionResult SuaGiay(int id, HttpPostedFileBase fileUpload)
        {
            if (Session["Taikhoan"] == null)
            {
                return RedirectToAction("login", "Admin");
            }
            else
            {
                GIAY giay = db.GIAYs.SingleOrDefault(n => n.Magiay == id);
                ViewBag.MaTH = new SelectList(db.THUONGHIEUs.ToList().OrderBy(n => n.TenThuongHieu), "MaTH", "TenThuongHieu");
                ViewBag.Maloai = new SelectList(db.LOAIGIAYs.ToList().OrderBy(n => n.Tenloai), "Maloai", "Tenloai");
                //Kiem tra duong dan file
                if (fileUpload == null)
                {
                    ViewBag.Thongbao1 = "Vui lòng chọn ảnh";
                    return View(giay);
                }
                //Them vao CSDL
                else
                {
                    if (ModelState.IsValid)
                    {
                        //Luu ten fie, luu y bo sung thu vien using System.IO;
                        var fileName = Path.GetFileName(fileUpload.FileName);

                        //Luu duong dan cua file
                        var path = Path.Combine(Server.MapPath("~/product_imgs"), fileName);

                        //Kiem tra hình anh ton tai chua?
                        if (System.IO.File.Exists(path))
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        else
                        {
                            //Luu hinh anh vao duong dan
                            fileUpload.SaveAs(path);

                        }
                        giay.AnhBia = fileName;
                        //Luu vao CSDL
                        UpdateModel(giay);
                        db.SubmitChanges();
                    }
                    return RedirectToAction("Giay", "Admin");
                }
            }
        }
        [HttpGet]
         public ActionResult ThemMoiThuongHieu()
        {
            ViewBag.MaTH = new SelectList(db.THUONGHIEUs.ToList().OrderBy(n => n.TenThuongHieu), "MaTH", "TenThuongHieu");
            return View();
        }

        [HttpPost]
        public ActionResult ThemMoiThuongHieu(THUONGHIEU thuonghieu)
        {
            ViewBag.MaTH = new SelectList(db.THUONGHIEUs.ToList().OrderBy(n => n.TenThuongHieu), "MaTH", "TenThuongHieu");
            db.THUONGHIEUs.InsertOnSubmit(thuonghieu);
            db.SubmitChanges();
            return RedirectToAction("THUONGHIEU");
        }

        [HttpGet]
        public ActionResult ThemMoiLoaiGiay()
        {
            ViewBag.Maloai = new SelectList(db.LOAIGIAYs.ToList().OrderBy(n => n.Tenloai), "Maloai", "Tenloai");
            return View();
        }

        [HttpPost]
        public ActionResult ThemMoiLoaiGiay(LOAIGIAY loaigiay)
        {
            ViewBag.Maloai = new SelectList(db.LOAIGIAYs.ToList().OrderBy(n => n.Tenloai), "Maloai", "Tenloai");
            db.LOAIGIAYs.InsertOnSubmit(loaigiay);
            db.SubmitChanges();
            return RedirectToAction("LOAIGIAY");
        }
    }   
}