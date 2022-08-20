using DuAnQLNCKH.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
 
namespace DuAnQLNCKH.Controllers
{
    public class TopicOfLectureController : Controller
    {
        // GET: DeTaiGV
        DHTDTTDNEntities1 qLNCKHDHTDTD = new DHTDTTDNEntities1();
        TopicOfLectureModel dtgv = new TopicOfLectureModel();
        List<TopicOfStudent> studentList = new List<TopicOfStudent>();
        List<Subject> subjects = new DHTDTTDNEntities1().Subjects.ToList();
        List<Extension> extensions = new DHTDTTDNEntities1().Extensions.ToList();
        List<TopicOfLecture> topicOfLectures = new DHTDTTDNEntities1().TopicOfLectures.ToList();
        List<TopicOfStudent> topicOfStudents = new DHTDTTDNEntities1().TopicOfStudents.ToList();
        List<Information> information = new DHTDTTDNEntities1().Information.ToList();
        List<PointTable> pointTables = new DHTDTTDNEntities1().PointTables.ToList();
        List<Faculty> faculties = new DHTDTTDNEntities1().Faculties.ToList();
        List<ProgressLe> progressLes = new DHTDTTDNEntities1().ProgressLes.ToList();
         List<Author> authors = new DHTDTTDNEntities1().Authors.ToList();
        List<Models.Type> types = new DHTDTTDNEntities1().Types.ToList();
         
        public ActionResult getTypeList(int IdTy)
        {
            qLNCKHDHTDTD.Configuration.ProxyCreationEnabled = false;
            List<PointTable> DetailList = qLNCKHDHTDTD.PointTables.Where(x => x.IdP == IdTy).ToList();
            return Json(DetailList, JsonRequestBehavior.AllowGet);
        }
          
        [HttpPost]
        public ActionResult editInfo(Information model)
        {

            var data = qLNCKHDHTDTD.Information.Find(model.Email);            
            data.NameLe = model.NameLe;
            data.Phone = model.Phone;
            data.Email = model.Email;
            data.DateOfBirth = model.DateOfBirth;
            data.Address = model.Address;
            data.CMND = model.CMND;
            qLNCKHDHTDTD.Entry(data).State = EntityState.Modified;
            qLNCKHDHTDTD.SaveChanges();

            return RedirectToAction("listInformation");
          

        }
        public ActionResult Index()
        {                
                var topicofLecture = (from t in topicOfLectures
                                      join a in authors on t.IdTp equals a.IdTp
                                      join i in information on a.Email equals i.Email
                                      join f in faculties on t.IdFa equals f.IdFa
                                      join pr in progressLes on t.IdTp equals pr.IdTp
                                      select new TopicOfLectureView
                                      {
                                          author=a,
                                           topicOfLecture = t,
                                          information = i,
                                           faculty=f,
                                          progressLe=pr
                                      }).ToList();
                ViewBag.TopicOfLecture = topicofLecture;
                var topics = (from t in topicOfStudents                              
                              join s in subjects on t.IdSu equals s.IdSu
                               select new TopicOfStudentView
                              {

                                  topicOfStudent = t,
                                  subject=s
                                 
                              }).ToList();
                ViewBag.listTopicOfStudent = topics;
            
          
            return View();

        }
        public ActionResult listAcceptanced()
        {                
            var topicofLecture = (from t in topicOfLectures
                                  join ty in types on t.IdType equals ty.IdType
                                     where t.Status==4 
                                    select new TopicOfLectureView
                                    {
                                        topicOfLecture = t,
                                        type=ty,
                                         
                                    }).ToList();
            ViewBag.listAcceptancedLec = topicofLecture;
            var topics = (from t in topicOfStudents
                             join s in subjects on t.IdSu equals s.IdSu
                             select new TopicOfStudentView
                            {
                                 
                                topicOfStudent = t,
                                subject=s,
                                
                            }).ToList();
            ViewBag.listAcceptancedStu = topics;
            
          
            return View();

        }
        public void dataMyTopic()
        {

            //var topicofLecture = (from t in topicOfLectures                                  
            //                     join a in authors on t.IdTp equals a.IdTp
            //                      join i in information on a.Email equals i.Email                                  
            //                       where i.Email == Session["UserName"].ToString() && t.Status == 0
            //                      select new TopicOfLectureView
            //                      {
            //                           topicOfLecture = t,
            //                          information = i,
            //                          author = a
            //                      }).ToList();
            //ViewBag.topicExtension = topicofLecture;
            
            var topic = (from t in topicOfLectures
                         join ty in types on t.IdType equals ty.IdType
                          join a in authors on t.IdTp equals a.IdTp
                         join i in information on a.Email equals i.Email
                          where i.Email == Session["UserName"].ToString() && t.Status == 1
                         select new TopicOfLectureView
                         {                              
                             topicOfLecture = t,
                             type=ty 
                         }).ToList();
            ViewBag.topicProgress = topic;
            //var topic2 = (from t in topicOfLectures
            //              join a in authors on t.IdTp equals a.IdTp
            //              join i in information on a.Email equals i.Email
            //               join e in extensions on t.IdTp equals e.IdTp
            //              join pr in progressLes on t.IdTp equals pr.IdTp
            //              where i.Email == Session["UserName"].ToString() && t.Status == 3 && new TopicOfLectureModel().dateLec(t.IdTp) == pr.Date
            //              select new TopicOfLectureView
            //              {
            //                  author = a,
            //                   topicOfLecture = t,
            //                  extension = e,
            //                  information = i,
            //                  progressLe = pr
            //              }).ToList();
            //ViewBag.topicExEnd = topic2;
            var topic1 = (from t in topicOfLectures
                          join ty in types on t.IdType equals ty.IdType
                          join a in authors on t.IdTp equals a.IdTp
                          join i in information on a.Email equals i.Email
                           where i.Email == Session["UserName"].ToString() && t.Status == 4
                          select new TopicOfLectureView
                          {
                              topicOfLecture = t,
                              type=ty
                          }).ToList();
            ViewBag.topicEnd = topic1;
        }
        public ActionResult myTopicLecture()
        {
            dataMyTopic();
            return View();
             
        }
        //To Handle connection related activities
        [HttpPost]
        public void ShowIdP(string id)
        {
            ViewBag.idp = id;
            
        }
        [HttpPost]
        public ActionResult CreateTopicOfLecture(HttpPostedFileBase FileDemo1, TopicOfLecture topicOfLecture, List<string> email, List<string> nameAu, List<int> Hours, byte typeRegister, int IdType=0, int HourAdmin=0, string NameAdmin=null, string EmailAdmin=null, int HourAu=0)
        {
            string IdTp = dtgv.IdTp();
            
             if (ModelState.IsValid)
             {
                 string username = Session["UserName"].ToString();
                TopicOfLectureModel topic = new TopicOfLectureModel();
                 
                if (typeRegister == 2)
                {
                    topicOfLecture.Status = 0;
                }
                else
                {
                    
                   topicOfLecture.Status = 3;
                }    
                topicOfLecture.IdTp = IdTp;
                topicOfLecture.IdType = IdType;
                if (FileDemo1!=null)
                {
                    var Extension = Path.GetExtension(FileDemo1.FileName);
                    var fileName = "fileTopic-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Extension;
                    string path = Path.Combine(Server.MapPath("~/File/FileTopic"), fileName);
                    topicOfLecture.FileDemo = Url.Content(Path.Combine("~/File/FileTopic/", fileName));
                    FileDemo1.SaveAs(path);
                }
                topic.AddTopicLecture(topicOfLecture);

                ViewBag.Message = "Đăng ký công trình thành công";
                ViewBag.listtype = new SelectList(types, "IdType", "NameType");
                
                Author author = new Author();
                if (EmailAdmin==null)
                {
                    EmailAdmin= Session["UserName"].ToString();                   
                    NameAdmin = qLNCKHDHTDTD.Information.Where(x => x.Email == EmailAdmin).Select(x => x.NameLe).FirstOrDefault();
                    HourAdmin = HourAu;
                }
                else
                {
                    var emailA = Session["UserName"].ToString();
                    email.Add(emailA);
                    string name = qLNCKHDHTDTD.Information.Where(x => x.Email == emailA).Select(x => x.NameLe).FirstOrDefault().ToString();
                    nameAu.Add(name);
                    Hours.Add(HourAu);
                }
                author.Email = EmailAdmin;
                author.NameAu = NameAdmin;
                author.IdTp = IdTp;
                author.Grade = 0;
                qLNCKHDHTDTD.Authors.Add(author);
                qLNCKHDHTDTD.SaveChanges();
                if (email!=null)
                {
                    for (int i = 0; i < email.Count; i++)
                    {
                        author.Email = email[i];
                        author.NameAu = nameAu[i];
                        author.IdTp = IdTp;
                        author.Grade = 1;
                        qLNCKHDHTDTD.Authors.Add(author);
                        qLNCKHDHTDTD.SaveChanges();

                    }
                }    
                
                qLNCKHDHTDTD.SaveChanges();
                PointTable pointTable = new PointTable();
                pointTable.IdAu = qLNCKHDHTDTD.Authors.Where(x => x.IdTp == IdTp && x.Grade==0).Select(x => x.IdAu).FirstOrDefault();
                pointTable.IdTp = IdTp;
                pointTable.Hours = HourAdmin;
                pointTable.Status = typeRegister;
                qLNCKHDHTDTD.PointTables.Add(pointTable);
                qLNCKHDHTDTD.SaveChanges();
                if (nameAu!=null)
                {
                    for (int i = 0; i < nameAu.Count; i++)
                    {
                        var emailAu = email[i];
                        pointTable.IdAu = qLNCKHDHTDTD.Authors.Where(x => x.Email == emailAu && x.IdTp == IdTp).Select(x => x.IdAu).FirstOrDefault();
                        pointTable.IdTp = IdTp;
                        pointTable.Hours = Hours[i];
                        pointTable.Status = typeRegister;
                        qLNCKHDHTDTD.PointTables.Add(pointTable);
                        qLNCKHDHTDTD.SaveChanges();
                    }
                }
                ViewBag.listtype = new SelectList(types, "IdType", "NameType");
                ViewBag.listFaculty = new SelectList(faculties, "IdFa", "Name");
                return View("ViewCreateTopicOfLecture");

             }

             ViewBag.listFaculty = new SelectList(faculties, "IdFa", "Name");
            return View("ViewCreateTopicOfLecture", topicOfLecture);
           
        }
           
        public ActionResult ViewCreateTopicOfLecture()
        {
            
            string email = Session["UserName"].ToString();
            ViewBag.NameLe = qLNCKHDHTDTD.Information.Where(x => x.Email == email).Select(x=>x.NameLe).FirstOrDefault();
            ViewBag.Message = "";
            ViewBag.listtype = new SelectList(types, "IdType", "NameType");
            List<Faculty> faculties = qLNCKHDHTDTD.Faculties.ToList();
            ViewBag.listFaculty = new SelectList(faculties, "IdFa", "Name");
            return View();
        }
        public ActionResult getPoint(int IdDetail, byte Level, int IdChild=0)
        {
            qLNCKHDHTDTD.Configuration.ProxyCreationEnabled = false;
            int Hour = 0;
            if (Level==0)
                Hour = int.Parse(qLNCKHDHTDTD.DetailType1.Where(x => x.IdDetail == IdDetail).Select(x => x.Hours).FirstOrDefault().ToString());           
            else
                Hour = int.Parse(qLNCKHDHTDTD.ChildDetails.Where(x => x.IdChild == IdChild).Select(x => x.Hours).FirstOrDefault().ToString());
             return Json(Hour, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getPointMember(int IdDetail)
        {
            qLNCKHDHTDTD.Configuration.ProxyCreationEnabled = false;
            int Hour = int.Parse(qLNCKHDHTDTD.ChildDetails.Where(x => x.IdDetail == IdDetail).OrderBy(x=>x.Hours).Select(x => x.Hours).FirstOrDefault().ToString());
             return Json(Hour, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult getHour(string IdTp)
        {
            qLNCKHDHTDTD.Configuration.ProxyCreationEnabled = false;
            var pointTable1 = (from p in pointTables
                                                   join a in authors on p.IdAu equals a.IdAu
                                                   where p.IdTp == IdTp
                                                   orderby a.Grade
                                                   select new 
                                                   {
                                                       a.NameAu,
                                                       p.Hours,
                                                       a.IdAu
                                                   }
                                                   ).ToList();
 
            return Json(pointTable1, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult getDetailTypes(int IdType)
        {
            qLNCKHDHTDTD.Configuration.ProxyCreationEnabled = false;
            List<DetailType1> DetailList = qLNCKHDHTDTD.DetailType1.Where(x => x.IdType == IdType).ToList();            
            return Json(DetailList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getLevel(int IdType)
        {
            qLNCKHDHTDTD.Configuration.ProxyCreationEnabled = false;
            int Level = byte.Parse(qLNCKHDHTDTD.Types.Where(x => x.IdType == IdType).Select(x => x.Level).FirstOrDefault().ToString());
            return Json(Level, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getDetailChilds(int IdDetail)
        {
            qLNCKHDHTDTD.Configuration.ProxyCreationEnabled = false;
            List<ChildDetail> ChildList = qLNCKHDHTDTD.ChildDetails.Where(x => x.IdDetail == IdDetail).ToList();
            return Json(ChildList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult chuaduyet()
        {
             
                var topicofLecture = (from t in topicOfLectures
                                      join ty in types on t.IdType equals ty.IdType
                                        where t.Status == 0
                                      select new TopicOfLectureView
                                      {
                                           topicOfLecture = t,
                                          type=ty
                                      }).ToList();
                ViewBag.TopicOfLecture = topicofLecture;
                 
           
            
            return View();
         
        }
        public ActionResult topicRejected()
        {             
                var topicofLecture = (from t in topicOfLectures
                                      join a in authors on t.IdTp equals a.IdTp
                                      join i in information on a.Email equals i.Email
                                       join f in faculties on t.IdFa equals f.IdFa
                                        where t.Status == 2
                                      select new TopicOfLectureView
                                      {
                                          topicOfLecture = t,
                                          information = i,
                                          faculty=f,
                                          author=a
                                      }).ToList();
                ViewBag.TopicOfLecture = topicofLecture;
                var topicofStudent = (from t in topicOfStudents
                                     join s in subjects on t.IdSu equals s.IdSu
                                       select new TopicOfLectureView
                                      {
                                           
                                          topicOfStudent = t,
                                          subject=s
                                      }).ToList();
                ViewBag.TopicOfStudent = topicofStudent;
            
            return View();
         
        }
         
        public ActionResult detailTopicLecture(string IdTp)
        {
            var listDetail = (from t in topicOfLectures
                              join ty in types on t.IdType equals ty.IdType
                              join a in authors on t.IdTp equals a.IdTp
                              join i in information on a.Email equals i.Email
                              join f in faculties on t.IdFa equals f.IdFa
                              where t.IdTp==IdTp
                              select new TopicOfLectureView
                              {
                                  topicOfLecture = t,
                                  information = i,
                                  faculty = f,
                                  author=a,
                                  type=ty
                              }
                            ).ToList();
            ViewBag.listDetail = listDetail;
            return View();
        }
        public void rejectTopic(string IdTp)
        {
             
            var topic = qLNCKHDHTDTD.TopicOfLectures.Find(IdTp);
            topic.Status = 2;            
            qLNCKHDHTDTD.Entry(topic).State = EntityState.Modified;
            qLNCKHDHTDTD.SaveChanges();
             
              
        }
        public ActionResult detailTopicSt(string IdTp)
        {
            var listDetail = (from t in topicOfStudents
                               join s in subjects on t.IdSu equals s.IdSu
                              where t.IdTp==IdTp
                              select new TopicOfLectureView
                              {
                                  topicOfStudent = t,                                  
                                  subject = s
                               }
                            ).ToList();
            ViewBag.listDetail = listDetail;
            return View();
        }
        
        [HttpPost]
        [Route("/TopicOfLecture/extension")]
        
        public ActionResult updateExtension(string IdTp, string DateEx, string Reason)
        {
            using (DHTDTTDNEntities1 entities = new DHTDTTDNEntities1())
            {
                List<TopicOfLecture> topicOfLectures = entities.TopicOfLectures.ToList();
                List<Extension> extensions = entities.Extensions.ToList();
                List<Information> information = entities.Information.ToList();

                var topicextension = (from t in topicOfLectures
                                      join a in authors on t.IdTp equals a.IdTp
                                      join i in information on a.Email equals i.Email
                                      join e in extensions on t.IdTp equals e.IdTp
                                      where i.Email == Session["UserName"].ToString()

                                      select new TopicOfLectureView
                                      {
                                          extension = e,
                                          topicOfLecture = t,
                                          information = i
                                      }).ToList();
                ViewBag.topicextension = topicextension;
                DateTime date = Convert.ToDateTime(DateEx);
                entities.Database.ExecuteSqlCommand("set dateformat dmy  update Extension set Times = '"+date+"', Reason=N'"+Reason+"' where IdTp='" + IdTp+"'");
                entities.SaveChanges();
            }
            return RedirectToAction("viewRegisterExtension");
        }
        [HttpPost]       
        public void xetduyet2(string IdTp)
        {

            
            string a = IdTp;
                
            var t = qLNCKHDHTDTD.TopicOfLectures.Find(IdTp);
            t.Status = 1;
            qLNCKHDHTDTD.Entry(t).State = EntityState.Modified;
                //entities.Database.ExecuteSqlCommand("set dateformat dmy update TopicOfLecture set Status=1 where IdTp='" + IdTp +"' set dateformat dmy insert into ProgressLe values('"+IdTp+"', '"+DateTime.Now.ToString("d")+"', 1)"+ " insert into Extension(IdTp, Times) values('" + IdTp + "', '" + Time + "')");
            qLNCKHDHTDTD.SaveChanges();
//                qLNCKHDHTDTD.Entry(t).State = EntityState.Detached;


                // By using a Message with no constructors, you can define your To recipients below

             
        }
       
        public void editExpense(string IdTp, float expense)
        {
            var topic = qLNCKHDHTDTD.TopicOfLectures.Find(IdTp);
            topic.Expense = expense;
            qLNCKHDHTDTD.Entry(topic).State = EntityState.Modified;
            qLNCKHDHTDTD.SaveChanges();
        }
       
        public ActionResult Register1(string IdTp, int HourAdmin, int[] Hours, int[] IdAu, HttpPostedFileBase FileDemo1)
        {                                      
            var topic = qLNCKHDHTDTD.TopicOfLectures.Find(IdTp);
            topic.Status = 3;
            var Extension = Path.GetExtension(FileDemo1.FileName);
            var fileName = "fileTopic-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Extension;
            string path = Path.Combine(Server.MapPath("~/File/FileTopic"), fileName);
            topic.FileDemo = Url.Content(Path.Combine("~/File/FileTopic/", fileName));
            FileDemo1.SaveAs(path);
            qLNCKHDHTDTD.Entry(topic).State = EntityState.Modified;
            qLNCKHDHTDTD.SaveChanges();
            string email = Session["UserName"].ToString();
            int IdAuAdmin =int.Parse(qLNCKHDHTDTD.Authors.Where(x => x.Email == email && x.IdTp == IdTp).Select(x=>x.IdAu).FirstOrDefault().ToString());
            PointTable pA = qLNCKHDHTDTD.PointTables.Where(x=>x.IdTp == IdTp && x.IdAu==IdAuAdmin).FirstOrDefault();
            pA.Hours = HourAdmin;
            qLNCKHDHTDTD.SaveChanges();
            if (Hours!=null)
            {
                for (int i = 0; i < Hours.Length; i++)
                {
                    int Au = IdAu[i];
                    int Hour = Hours[i];
                    PointTable p = qLNCKHDHTDTD.PointTables.Where(x => x.IdAu == Au && x.IdTp == IdTp).FirstOrDefault();
                    p.Hours = Hour;
                    qLNCKHDHTDTD.SaveChanges();
                }
            }    
            
             
            return RedirectToAction("myTopicLecture");
        }
        public ActionResult DownloadFile(string filePath)
        {
            string fullName = Server.MapPath("~" + filePath);

            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filePath);
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }
 
        //public void xetduyetsv(TopicOfStudent topicOfStudent)
        //{

        //        TopicOfStudent topic = (from c in qLNCKHDHTDTD.TopicOfStudents
        //                                where c.IdTp == topicOfStudent.IdTp
        //                                select c).FirstOrDefault();
        //        qLNCKHDHTDTD.Database.ExecuteSqlCommand("update TopicOfStudent set Status=N'đã duyệt' where IdTp='" + topic.IdTp + "'");
        //        qLNCKHDHTDTD.SaveChanges();

        //}
        //delete TopicLecture
        //public void deleteTopicLec(string IdTp)
        //{
        //    TopicOfLecture t= qLNCKHDHTDTD.TopicOfLectures.Find(IdTp);
        //    qLNCKHDHTDTD.TopicOfLectures.Remove(t);
        //    qLNCKHDHTDTD.SaveChanges();
        //}
        //delete TopicStudent
        //public void deleteTopicStu(string IdTp)
        //{
        //    TopicOfStudent t= qLNCKHDHTDTD.TopicOfStudents.Find(IdTp);
        //    qLNCKHDHTDTD.TopicOfStudents.Remove(t);
        //    qLNCKHDHTDTD.SaveChanges();
        //}
        //public void rejectTopicSt(string IdTp, string Reason)
        //{
        //    var email = (from t in topicOfStudents

        //                    where t.IdTp == IdTp
        //                    select new
        //                    {
        //                        t.Email
        //                    }).FirstOrDefault().Email;
        //    TopicOfStudent topicOfStudent = new TopicOfStudent();
        //    var topic = qLNCKHDHTDTD.TopicOfStudents.Find(IdTp);
        //    topic.Status = 2;            
        //    qLNCKHDHTDTD.Entry(topic).State = EntityState.Modified;
        //    qLNCKHDHTDTD.SaveChanges();

        //    var nameTopic = qLNCKHDHTDTD.TopicOfStudents.Where(x => x.IdTp == IdTp).Select(x => x.Name).FirstOrDefault();
        //    string from1 = qLNCKHDHTDTD.Emails.Select(x => x.NameEmail).FirstOrDefault();
        //    string pass = qLNCKHDHTDTD.Emails.Where(x => x.NameEmail == from1).Select(x => x.PassWord).FirstOrDefault();

        //    using (MailMessage mail = new MailMessage())
        //    {
        //        // Define your senders
        //        mail.From = new MailAddress(from1);
        //        mail.Body = "Thông báo về đề tài " + nameTopic + " không được duyệt, Lí do : "+Reason;
        //        mail.Subject = "Thông báo đề tài không được duyệt";

        //        mail.To.Add(email.ToString());

        //        mail.IsBodyHtml = true;
        //        SmtpClient smtp = new SmtpClient();
        //        smtp.Host = "smtp.gmail.com";
        //        smtp.EnableSsl = true;
        //        NetworkCredential networkCredential = new NetworkCredential(from1, pass);
        //        smtp.UseDefaultCredentials = true;
        //        smtp.Credentials = networkCredential;
        //        smtp.Port = 587;
        //        smtp.Send(mail);
        //    }
        //}
        //public void extension(int IdEx, string NameTo, string Email)
        //{

        //    using (DHTDTTDNEntities1 entities = new DHTDTTDNEntities1())
        //    {
        //        entities.Database.ExecuteSqlCommand("update Extension set Status = N'Đã duyệt' where IdEx=" + IdEx);
        //        entities.SaveChanges();
        //        string from1 = qLNCKHDHTDTD.Emails.Select(x => x.NameEmail).FirstOrDefault();
        //        string pass = qLNCKHDHTDTD.Emails.Where(x => x.NameEmail == from1).Select(x => x.PassWord).FirstOrDefault();
        //        // By using a Message with no constructors, you can define your To recipients below
        //        using (MailMessage mail = new MailMessage())
        //        {
        //            // Define your senders
        //            mail.From = new MailAddress(from1);
        //            mail.Body = "Thông báo đề tài " + NameTo + " đã được gia hạn";
        //            mail.Subject = "Thông báo gia hạn đề tài";

        //            mail.To.Add(Email);

        //            mail.IsBodyHtml = true;
        //            SmtpClient smtp = new SmtpClient();
        //            smtp.Host = "smtp.gmail.com";
        //            smtp.EnableSsl = true;
        //            NetworkCredential networkCredential = new NetworkCredential(from1, pass);
        //            smtp.UseDefaultCredentials = true;
        //            smtp.Credentials = networkCredential;
        //            smtp.Port = 587;
        //            smtp.Send(mail);
        //        }

        //    }

        //}
        //public ActionResult registerExtension(string IdTp, DateTime Times, string Reason)
        //{


        //        qLNCKHDHTDTD.Database.ExecuteSqlCommand("set dateformat dmy update Extension set Times='" + Times + "', Reason=N'" + Reason + "', Status=N'chưa duyệt' where IdTp='"+IdTp+"'");


        //        var topicextension = (from t in topicOfLectures
        //                              join a in authors on t.IdTp equals a.IdTp
        //                              join i in information on a.Email equals i.Email
        //                              join e in extensions on t.IdTp equals e.IdTp
        //                              where i.Email == Session["UserName"].ToString()

        //                              select new TopicOfLectureView
        //                              {
        //                                  extension = e,
        //                                  topicOfLecture = t,
        //                                  information = i
        //                              }).ToList();
        //        ViewBag.topicextension = topicextension;


        //    return RedirectToAction("viewRegisterExtension");
        //}
        //public ActionResult viewRegisterExtension()
        //{     

        //        var topicextension = (from t in topicOfLectures
        //                              join a in authors on t.IdTp equals a.IdTp
        //                              join i in information on a.Email equals i.Email

        //                             join e in extensions on t.IdTp equals e.IdTp
        //                             where new TopicOfLectureModel().dateLecEx(t.IdTp) == e.Times && t.Status==1
        //                              select new TopicOfLectureView
        //                             {
        //                                 extension=e,
        //                                 topicOfLecture = t,
        //                                 information=i,

        //                             }).ToList();
        //        ViewBag.topicextension = topicextension;
        //        return View();

        //}
        //public ActionResult listextension()
        //{

        //        var topicextension =(from t in topicOfLectures 
        //                             join a in authors on t.IdTp equals a.IdTp
        //                             join e in extensions on t.IdTp equals e.IdTp
        //                             join i in information on a.Email equals i.Email
        //                             join f in faculties on i.IdKhoa equals f.IdFa
        //                             join pr in progressLes on t.IdTp equals pr.IdTp
        //                             where e.Status == "chưa duyệt"
        //                             select new TopicOfLectureView
        //                             {
        //                                 extension = e,
        //                                 topicOfLecture = t,
        //                                 information=i,
        //                                 faculty=f,
        //                                 progressLe=pr

        //                             }).ToList();
        //        ViewBag.listextension = topicextension;
        //        return View();

        //}

        //public ActionResult chuaduyetsv()
        //{            

        //     var model = dtgv.listchuaduyetsv();
        //    return View(model);
        //}
    }
}