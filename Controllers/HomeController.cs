using GSF.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using OnlineShopeProject.Models;


namespace OnlineShopeProject.Controllers
{
    public class HomeController : Controller
    {
        Model4 db = new Model4();

        public ActionResult IndexCustomer()
        {
            
            return View();
        }
        public ActionResult IndexAdmin()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {

           
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
         [HttpPost]

        public ActionResult Login(Admin admin)
        {
           
           Admin a = db.Admins.Where(x => x.ADMIN_EMAIL == admin.ADMIN_EMAIL && x.ADMIN_PASSWORD == admin.ADMIN_PASSWORD).FirstOrDefault();
           if(a !=null)
            {
                Session["Admin"] = a;
                return RedirectToAction("IndexAdmin","Home");
            }
           else
            {
                ViewBag.Message = "Invalid Email or Password";
                return View();

            }

        }

        public ActionResult Logout()
        {
            Session["Admin"] = null;
            return RedirectToAction("Login");
        }

        public ActionResult MyAccount()
        {
            return View();
        }


        public ActionResult Cart()
        {
            return View();
        }

        public ActionResult Shipping()
        {
            return View();
        }

        //var o = db.Orders.Where(x => x.ORDER_TYPE == "Sale" & x.ORDER_DATE >= fm.DateFrom & x.ORDER_DATE <= fm.DateTo & od.Contains(x.ORDER_ID)).OrderByDescending(x => x.ORDER_ID).ToList();
        //return View(o);
        public ActionResult Shop(int? id)
        {
            ShopModel s = new ShopModel();
            s.Cat = db.Categories.ToList();
            if (id==null)
            s.Pro = db.Products.OrderByDescending(p =>p.PRODUCT_ID).ToList();
            else
               s.Pro = db.Products.Where(p=>p.CATEGORY_FID==id).ToList();
            return View(s);
        }
        public ActionResult AddToCart(int id)
        {
            List<Product> list;
            if(Session["myCart"]==null)
            {
                list = new List<Product>();
            }
            else
            {
                list = (List<Product>)Session["myCart"];
            }
            Boolean isProductExist = false;
            foreach(var item in list)
            {
                if(id==item.PRODUCT_ID)
                {
                    isProductExist = true;
                    item.PRO_QUANTITY++;
                }
            }
            if (isProductExist ==false)
            { 
                list.Add(db.Products.Where(p => p.PRODUCT_ID == id).FirstOrDefault());
                list[list.Count - 1].PRO_QUANTITY = 1;
            }
           
            Session["myCart"] = list;
            return RedirectToAction("Cart");
        }
        public ActionResult MinusFromCart(int RowNo)
        {
            List<Product> list = ( List<Product>)Session["myCart"];
           
            list[RowNo].PRO_QUANTITY--;
            if (list[RowNo].PRO_QUANTITY == 0)
                list.RemoveAt(RowNo);
            Session["myCart"] = list;
            return RedirectToAction("Cart");
        }
        public ActionResult PlusToCart(int RowNo)
        {
            List<Product> list = (List<Product>)Session["myCart"];
            int P_ID = list[RowNo].PRODUCT_ID;
            int? available = db.OrderDetails.Where(x => x.PRODUCT_FID == P_ID).Sum(x => x.QUANTITY);
            if(available>list[RowNo].PRO_QUANTITY)
                list[RowNo].PRO_QUANTITY++;
            Session["myCart"] = list;
            return RedirectToAction("Cart");
        }
        public ActionResult RemoveFromCart(int RowNo)
        {
            List<Product> list = (List<Product>)Session["myCart"];
            list.RemoveAt(RowNo);
            Session["myCart"] = list;
            return RedirectToAction("Cart");
        }
        public ActionResult PayNow(Order o)
        {

            o.ORDER_DATE = System.DateTime.Now;
            o.ORDER_TYPE = "Sale";
            o.STATUS = "Active";
            if(Session["Customer"] != null)
            {
                Customer c = (Customer)Session["Customer"];
                o.CUSTOMER_FID = c.CUSTOMER_ID;
            }
            Session["Order"] = o;
            if(o.ORDER_STATUS =="Cash on Delivery")
            {
                return RedirectToAction("OrderBooked");
            }
            else
            {
                
                return Redirect("https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_xclick&business=iqrasaleem4562@gmail.com&item_name=OnlineShopProducts&return=https://localhost:44314/Home/OrderBooked&amount=" + double.Parse(Session["totalAmount"].ToString()) / 160);
            }
        }
           
            

        public ActionResult OrderBooked()
        {

            Order o = (Order)Session["Order"];

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("onlineshoppingmart119@gmail.com");
            mail.To.Add(o.ORDER_EMAIL);
            mail.Subject = "Order Confirmation";
            mail.Body = "<b>Online Shopping Mart</b>| Your Shopping Partner! 🛒. We have received your order. It will be deliver soon in working days. For any Enquery, You can mail us or directly message on <b> 03351510000 </b>";
            mail.IsBodyHtml = true;

            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("onlineshoppingmart119@gmail.com", "biledycydojttthr");
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);

            ////Single message sending code
            //String api = "https://lifetimesms.com/json?api_token=56158a0db71745a91c6e2542ecd2aee2ad09fc6916&api_secret=VarieQuick&to=" + o.ORDER_CONTACT + "&from=Varie Quick&message=<b> VARIE QUICK <b> | Your Shopping Partner 🛒. Thanks for Shopping, Stay Safe & Stay Happy🙂...";
            //mail.IsBodyHtml = true;
            //HttpWebRequest req = (HttpWebRequest)WebRequest.Create(api);
            //var httpResponse = (HttpWebResponse)req.GetResponse();


            ////save changes in database

            db.Orders.Add(o); 
            db.SaveChanges();
            List<Product> p = (List<Product>)Session["mycart"];
            for (int i = 0; i<p.Count; i++)
            {
                OrderDetail od = new OrderDetail();
                int orderID = db.Orders.Max(x=>x.ORDER_ID);
                od.ORDER_FID = orderID;
                od.PRODUCT_FID = p[i].PRODUCT_ID;
                od.QUANTITY = p[i].PRO_QUANTITY *-1;
                od.PURCHASE_PRICE = p[i].PRODUCT_PURCHASEPRICE;
                od.SALE_PRICE = p[i].PRODUCT_SALEPRICE;
                db.OrderDetails.Add(od);
                db.SaveChanges();
            }

                return View();
        }
           
        public ActionResult CloseOrder()
        {
            Session["Order"] = null;
            Session["myCart"] = null;
            return RedirectToAction("IndexCustomer");
        }

        public ActionResult ProductDetail(int id)
        {
            ProductDetail p = new ProductDetail();
            p.ProA = db.Products.Where(x =>x.PRODUCT_ID==id).ToList();
            return View(p);
        }
        

    }
}

//var o = db.Orders.Where(x => x.ORDER_ID == id).ToList();
//return View(o);