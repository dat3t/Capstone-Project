
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using OneVietnam.BLL;
using OneVietnam.DTL;
using OneVietnam.Models;

namespace OneVietnam.Controllers
{
    public class PostController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public static bool CreatedPost = false;
        public static PostViewModel PostView;

        public PostController()
        {
        }

        public PostController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private PostManager _postManager;
        public PostManager PostManager
        {
            get
            {
                return _postManager ?? HttpContext.GetOwinContext().Get<PostManager>();
            }
            private set { _postManager = value; }
        }
        private TagManager _tagManager;
        public TagManager TagManager
        {
            get
            {
                return _tagManager ?? HttpContext.GetOwinContext().Get<TagManager>();
            }
            private set { _tagManager = value; }
        }

        private IconManager _iconManager;
        public IconManager IconManager
        {
            get
            {
                return _iconManager ?? HttpContext.GetOwinContext().Get<IconManager>();
            }
            private set { _iconManager = value; }
        }
        public List<Tag> TagList
        {
            get
            {
                var tags = TagManager.GetTagsAsync();
                return tags?.Result;
            }
        }
        public List<Icon> IconList
        {
            get
            {
                var icons = IconManager.GetIconPostAsync();
                return icons;
            }
        }


        public void _CreatePost()
        {
            if (TagList != null)
            {
                ViewData["TagList"] = TagList;
            }
            if (IconList != null)
            {
                ViewData["PostTypes"] = IconList;
            }

        }
        [System.Web.Mvc.Authorize]
        public ActionResult CreatePost()
        {
            if (TagList != null)
            {
                ViewData["TagList"] = TagList;
            }
            if (IconList != null)
            {
                ViewData["PostTypes"] = IconList;
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePost(CreatePostViewModel p)
        {
            ViewData.Clear();
            p.UserName = User.Identity.Name;
            var tagList = AddAndGetAddedTags(Request, TagManager, "TagsInput");
            var illList = GetAddedImage(Request, "Img", "Des");
            if (tagList != null)
            {
                p.Tags = tagList;
            }

            if (illList != null)
            {
                p.Illustrations = illList;
            }
            var post = new Post(p)
            {
                PublishDate = System.DateTime.Now,
                UserId = User.Identity.GetUserId()
            };

            await PostManager.CreatePostAsync(post);
            CreatedPost = true;
            PostView = new PostViewModel(post);
            return RedirectToAction("ShowPostDetail", "Post", new { postId = post.Id });
        }
        public async Task<ActionResult> TimeLine()
        {
            return RedirectToAction("GetPosts");
        }

        public const int RecordsPerPage = 60;

        public async Task<ActionResult> GetPosts(int? pageNum)
        {
            pageNum = pageNum ?? 0;
            ViewBag.IsEndOfRecords = false;

            if (Request.IsAjaxRequest())
            {
                var posts = GetRecordsForPage(pageNum.Value);
                ViewBag.IsEndOfRecords = (posts.Any()) && ((pageNum.Value * RecordsPerPage) >= posts.Last().Key);
                return PartialView("_PostRow", posts);
            }
            else
            {
                // LoadAllPostsToSession
                List<Post> list = await PostManager.FindAllPostsAsync();
                var posts = list;
                int postIndex = 1;
                Session["Posts"] = posts.ToDictionary(x => postIndex++, x => x);

                ViewBag.Posts = GetRecordsForPage(pageNum.Value);
                return View("TimeLine");
            }
        }
        public Dictionary<int, Post> GetRecordsForPage(int pageNum)
        {
            Dictionary<int, Post> posts = (Session["Posts"] as Dictionary<int, Post>);

            int from = (pageNum * RecordsPerPage);
            int to = from + RecordsPerPage;

            return posts
                .Where(x => x.Key > from && x.Key <= to)
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.Value);
        }
        public async Task<ActionResult> ShowPost()
        {
            //List<Post> list = await PostManager.FindByUserId(User.Identity.GetUserId());
            List<Post> list = await PostManager.FindAllPostsAsync();
            List<PostViewModel> pViewList = list.Select(post => new PostViewModel(post)).ToList();
            return View(pViewList);
        }

        public void _ShowPostDetail(string postId)
        {
            ViewData.Clear();
            var post = PostManager.FindById(postId);
            if (post.Result != null)
            {
                ViewBag.PostDetail = post.Result;

                if (TagList != null)
                {
                    ViewData["TagList"] = TagList;
                }
                if (IconList != null)
                {
                    ViewData["PostTypes"] = IconList;
                }

                var postUser = UserManager.FindByIdAsync(post.Result.UserId);
                if (postUser.Result != null)
                {
                    ViewBag.PostUser = postUser.Result;
                }
            }
        }

        public async Task<ActionResult> ShowPostDetail(string postId)
        {
            if (TagList != null)
            {
                ViewData["TagList"] = TagList;
            }
            if (IconList != null)
            {
                ViewData["PostTypes"] = IconList;
            }

            Post post = await PostManager.FindById(postId);
            ApplicationUser postUser = await UserManager.FindByIdAsync(post.UserId);
            if (postUser != null)
            {
                ViewData["PostUser"] = postUser;
            }
            PostViewModel showPost = new PostViewModel(post);
            return View(showPost);
        }
        [HttpPost]
        public ActionResult ShowPostDetail(PostViewModel pPostView)
        {
            ViewData.Clear();
            string strPostId = "";
            if (Request.Form.Count > 0)
            {
                strPostId = Request.Form["PostId"];
            }
            return RedirectToAction("DeletePost", "Post", new { postId = strPostId });
        }

        public async Task<ActionResult> EditPost(string postId)
        {
            if (TagList != null)
            {
                ViewData["TagList"] = TagList;
            }
            if (IconList != null)
            {
                ViewData["PostTypes"] = IconList;
            }
            Post post = await PostManager.FindById(postId);
            PostViewModel showPost = new PostViewModel(post);
            return View(showPost);
        }

        [HttpPost]
        public async Task<ActionResult> EditPost(PostViewModel pPostView)
        {
            ViewData.Clear();
            string strPostId = "";
            if (Request.Form.Count > 0)
            {
                strPostId = Request.Form["PostId"];
            }
            ViewData.Clear();
            var tagList = AddAndGetAddedTags(Request, TagManager, "TagsInput");
            var illList = GetAddedImage(Request, "Img", "Des");
            if (tagList != null)
            {
                pPostView.Tags = tagList;
            }

            if (illList != null)
            {
                pPostView.Illustrations = illList;
            }
            Post post = new Post(pPostView, strPostId);
            await PostManager.UpdatePostAsync(post);
            return RedirectToAction("ShowPostDetail", "Post", new { postId = strPostId });
        }

        public async Task<ActionResult> DeletePost(string postId)
        {
            Post post = await PostManager.FindById(postId);
            post.DeletedFlag = true;
            //await PostManager.DeleteByIdAsync(postId);            
            await PostManager.UpdatePostAsync(post);
            return RedirectToAction("CreatePost", "Post");
        }

        public class MyHub : Hub
        {
            public override Task OnConnected()
            {
                if (PostController.CreatedPost)
                {
                    var javaScriptSerializer = new JavaScriptSerializer();
                    string jsonString = javaScriptSerializer.Serialize(PostController.PostView);
                    Clients.Others.loadNewPost(jsonString);
                }
                return base.OnConnected();
            }
        }

        public List<Tag> AddAndGetAddedTags(HttpRequestBase pRequestBase, TagManager pTagManager, string pFormId)
        {
            if (pRequestBase.Form.Count > 0)
            {
                var addedTagValueList = pRequestBase.Form[pFormId];
                if (!string.IsNullOrEmpty(addedTagValueList))
                {
                    List<Tag> newList = new List<Tag>();
                    var tagsInDb = pTagManager.GetTagsValueAsync();
                    int numberTags = 0;
                    if (tagsInDb != null)
                    {
                        numberTags = tagsInDb.Count;
                    }

                    foreach (var tag in addedTagValueList.Split(','))
                    {
                        if (tagsInDb == null | (tagsInDb != null && !tagsInDb.Contains(tag)))
                        {
                            Tag newTag = new Tag(string.Concat("Tag_", numberTags.ToString()), tag);
                            pTagManager.CreateAsync(newTag);
                            numberTags = numberTags + 1;
                            newList.Add(newTag);
                        }
                        else
                        {
                            var existTag = pTagManager.FindTagByValueAsync(tag);
                            newList.Add(existTag[0]);
                        }
                    }
                    return newList;
                }
                return null;
            }
            return null;
        }

        public List<Illustration> GetAddedImage(HttpRequestBase pRequestBase, string pImgSrc, string pImgDes)
        {
            List<Illustration> illList = new List<Illustration>();
            if (pRequestBase.Form.Count > 0)
            {
                var addedImgSrcList = pRequestBase.Form[pImgSrc];
                var addedImgDesList = pRequestBase.Form[pImgDes];
                if (!string.IsNullOrEmpty(addedImgSrcList))
                {
                    var imgSrcs = addedImgSrcList.Split(',');
                    var imgDes = addedImgDesList.Split(',');
                    for (int index = 0; index < imgDes.Length; index++)
                    {

                        Illustration newIll = new Illustration(string.Concat(imgSrcs[2 * index] + ",", imgSrcs[2 * index + 1]), imgDes[index]);
                        illList.Add(newIll);
                    }
                }
            }
            return illList;
        }

        // TraNT: Auto create post
        #region AutoGeneratePosts
        private readonly ArrayList _arrRawDesciptions = new ArrayList();
        private readonly String[] _strRawDescriptions =
        {
            "Chiều ngày 17/7/2016, Hội đồng bầu cử quốc gia đã họp đột xuất Phiên thứ tám tại Nhà Quốc hội, Thủ đô Hà Nội dưới sự chủ trì của Chủ tịch Quốc hội, Chủ tịch Hội đồng bầu cử quốc gia Nguyễn Thị Kim Ngân. Tham dự Phiên họp có các thành viên Hội đồng bầu cử quốc gia; đại diện thường trực Ủy ban trung ương Mặt trận tổ quốc Việt Nam; đại diện lãnh đạo Ban tổ chức trung ương, Văn phòng Trung ương Đảng, Văn phòng Quốc hội, Ban công tác đại biểu, đại diện các Tiểu ban giúp việc của Hội đồng bầu cử quốc gia.", "Với tinh thần nghiêm túc, trách nhiệm, Hội đồng bầu cử quốc gia đã thận trọng xem xét, xác nhận tư cách đại biểu Quốc hội khóa XIV. Tại phiên họp này, Hội đồng bầu cử quốc gia đã tiến hành biểu quyết bằng hình thức bỏ phiếu kín để xác nhận tư cách người trúng cử đại biểu Quốc hội khóa XIV đối 494 đại biểu."
            ,"Bên cạnh đó, Hội đồng bầu quốc gia cũng tiến hành bỏ phiếu kín không xác nhận tư cách đại biểu Quốc hội khóa XIV đối với Bà Nguyễn Thị Nguyệt Hường, đại biểu Quốc hội khóa XIII."
            ,"Kết quả kiểm phiếu, có 100% các thành viên trong Hội đồng bầu cử quốc gia có mặt tại phiên họp nhất trí xác nhận tư cách đại biểu Quốc hội khóa XIV đối với 494 đại biểu. 100% các thành viên Hội đồng bầu cử quốc gia bỏ phiếu không xác nhận tư cách đại biểu Quốc hội khóa XIV đối với bà Nguyễn Thị Nguyệt Hường vì không đủ tiêu chuẩn đại biểu Quốc hội khóa XIV."
            ,"Theo Văn phòng Hội đồng bầu cử quốc gia cũng cho biết, cá nhân bà Hường có đơn xin rút không tham gia đại biểu Quốc hội khóa XIV."
            ,"Bà Nguyệt Hường ứng cử tại đơn vị bầu cử số 5 TP.Hà Nội (gồm các huyện Bắc Từ Liêm, Nam Từ Liêm, Đan Phượng, Hoài Đức) và trúng cử với tỷ lệ 78,51%."
                ,"Sinh năm 1970 tại Nam Định, bà Hường là một đại diện của giới doanh nhân tại diễn đàn Quốc hội, là đại biểu Quốc hội khóa XII, XIII. Kỳ này, nữ Chủ tịch HĐQT Cty cổ phần đầu tư TNG Holdings Việt Nam tiếp tục được giới thiệu tái cử khóa XIV, là một đại biểu trong khối của UB TƯ Mặt trận tổ quốc Việt Nam."
                ,"Như vậy, cùng với ông Trịnh Xuân Thanh – Tỉnh ủy viên tỉnh Hậu Giang, nguyên Phó Chủ tịch UBND tỉnh này không được công nhận tư cách đại biểu Quốc hội khóa XIV, nữ doanh nhân Nguyễn Thị Nguyệt Hường là trường hợp thứ 2 cũng không qua được vòng thẩm tra tư cách đại biểu dù đã trúng cử. Quốc hội khóa XIV, đến thời điểm này, có 494 đại biểu."
                ,"Tài năng nhí nổi bật được khán giả cả nước công nhận sẽ khuấy động đêm chung kết và khiến bầu không khí trở nên “sục sôi” hơn bao giờ hết."
                ,"Top 4 gồm 4 gương mặt thí sinh nhí xuất sắc nhất và được khán giả yêu mến gồm Jayden, Gia Khiêm, Hồ Văn Cường và Bảo Trân đang tập luyện nghiêm túc để ghi dấu bản thân trong đêm thi cuối cùng."
                ,"Trong đêm Gala Chung kết, mỗi thí sinh sẽ biểu diễn hai ca khúc. Trong đó, một ca khúc đã từng được các bé diễn ở các đêm liveshow trước và một ca khúc lần đầu tiên xuất hiện trên sân khấu Idol Kids. Ở đêm thi quan trọng này, các bé sẽ chọn những bài hát mình yêu thích và dòng nhạc sở trường của mình để nhận được những phiếu bầu chọn cuối cùng của khán giả."
                ,"Ngoài các tiết mục biểu diễn cá nhân, top 4 sẽ có một tiết mục chung mở màn cùng các bạn nhỏ trong top 13 gồm: Bảo Khương, Diệp Nhi, Khánh Linh, Thuỳ Anh."
                ,"Top 4 đã có rất nhiều những kỉ niệm, khi ca sĩ Tóc Tiên yêu cầu vui “Kể tật xấu của các bạn trong nhà chung” thì Gia Khiêm “méc” là Bảo Trân nóng tính. Jayden kể Gia Khiêm rất nghịch nhưng vì là “trẻ em” nên dù nghịch vẫn “chấp nhận được”. “Cặp đôi anh em thân thiết” là Hồ Văn Cường và Jayden thì khẳng định “đối phương” không hề có tật xấu nào cả khiến “đôi bên” đều hài lòng"
                ,"PGS-TS Đỗ Văn Dũng, Hiệu trưởng Trường ĐH Sư phạm kỹ thuật TPHCM, đơn vị chủ trì cụm thi tỉnh Bình Thuận, cho biết nhìn chung điểm năm nay đẹp hơn năm trước vì độ phân hóa cao hơn, điểm thấp hơn. Năm ngoái mức điểm từ 7-8 nhiều quá nên các trường khó chọn nhưng năm nay phổ điểm là 5-6 điểm, mức 7-8 điểm ít hơn nên các trường sẽ dễ tuyển hơn. Năm nay điểm cao ít, như ở môn Toán và Văn điểm cao nhất chỉ 9,25. Ngược lại điểm liệt của các môn cũng ít hơn. Môn Sử và Địa, mức điểm phổ biến chủ yếu từ 4-6 điểm."
                ,"TS Lê Chí Thông, Trưởng phòng đào tạo trường ĐH Bách khoa (ĐH Quốc gia TPHCM) cũng cho biết nhìn chung điểm thi không cao lắm. Đối với Văn, phổ điểm ở mức từ 4-6, không có bài thi nào đạt điểm 9. Trong khi đó, điểm môn Toán nhỉnh hơn một chút với mức điểm phổ biến từ 5-7 điểm và không có bài thi nào đạt 10 điểm."
                ,"Qua thống kê sơ bộ của cụm thi do trường ĐH Nông lâm TPHCM chủ trì, “Môn Toán mức điểm phổ biến từ 4 đến cận 7 điểm chiếm đến gần 50%. Đến hôm qua, bài điểm cao nhất môn toán ở cụm này là 9,25. Môn Văn đã có điểm 9. Trong khi đó, phổ điểm môn Sử thống kê sơ bộ có phần thấp hơn năm ngoái. Môn Địa xuất hiện bài thi đạt cao nhất là 9,75 điểm, phổ điểm có phần cao hơn môn Sử nhưng cũng không bằng năm ngoái”, TS Trần Đình Lý - Trưởng phòng đào tạo trường cho biết."
                ,"Theo Ths Phạm Thái Sơn, Phó trưởng phòng Đào tạo trường ĐH Công nghiệp Thực phẩm TPHCM, đơn vị chủ trì cụm thi tỉnh Tây Ninh, Phổ điểm các môn cũng tương đối thấp như môn Toán: 4- 6 điểm, môn Văn: 4- 5,5 điểm; Sử: 3,5- 5 điểm ; Địa: 4- 5,5; Hóa: 4- 5,5; Lý: 4,5- 6; Anh Văn: 3,5- 5. Tuy nhiên, ông Sơn cũng cho biết điểm liệt các môn năm nay rất ít, và phổ điểm phân bố khá chuẩn chứng tỏ đề thi có sự phân loại cao."
                ,"Từ kết quả điểm thi THPT quốc gia 2016, nhiều chuyên gia đã nhận định điểm chuẩn vào nhiều ngành, nhiều trường sẽ giảm mạnh so với năm 2015. Dự báo về điểm chuẩn của Trường ĐH Sư phạm Kỹ thuật TPHCM, PGS-TS Đỗ Văn Dũng cho rằng: “Qua nắm tình hình chấm thi của các tỉnh, thành phố cùng với tình hình thí sinh đăng ký xét tuyển ĐH, CĐ giảm, tôi cho rằng điểm chuẩn năm nay của trường sẽ thấp hơn từ 1-2 điểm"
                ,"Tương tự, ông Phạm Thái Sơn cho biết riêng với cụm thi Tây Ninh, mức điểm từ 7,5 trở lên không nhiều mà phổ điểm tập trung từ 4-6 điểm ở các môn tự luận. Riêng môn Sử phổ điểm thấp hơn chủ yếu ở 3,5-5 điểm. Ngược lại số điểm liệt rất ít chỉ khoảng 10 bài/môn. Đối với các môn trắc nghiệm, môn Lý và Hóa có nhích hơn một chút so với năm trước, phổ điểm dao động từ 4,5-6 điểm. Với phổ điểm này, đối với các tổ hợp môn truyền thống gồm A, A1, D thì ngưỡng điểm sẽ an toàn mức tương đương như năm ngoái. Nếu điểm chuẩn có giảm thì chủ yếu do lượng thí sinh năm nay ít, thì ngưỡng điểm khả năng cũng sẽ giảm nhưng không quá nhiều."
                ,"Dự đoán, điểm trúng tuyển của các trường tốp trên và tốp dưới sẽ không thay đổi nhiều so với năm ngoái. Riêng nhóm trường giữa vốn có điểm chuẩn dao động từ 19-20 điểm thì khả năng điểm chuẩn sẽ có biến động, nhất là các ngành không phải nhóm tổ hợp môn truyền thống khả năng điểm sẽ giảm."
                ,"Điều đáng mừng là đề thi có tính phân hóa tốt nên khi xét tuyển các trường sẽ không gặp khó đến mức đưa ra nhiều tiêu chí phụ như năm trước. Ông Sơn cũng lưu ý thí sinh năm nay: “Khi xét tuyển nên chú ý ngưỡng điểm xét tuyển của các trường, bên cạnh đó chú ý theo dõi thông tin phân tích của các chuyên gia đồng thời đừng nên vội vàng quá khi nộp hồ sơ đăng ký xét tuyển. Năm nay có nhiều hình thức nộp xét tuyển nhưng tốt nhất nên nộp qua đường bưu điện, vừa đỡ mất thời gian, tiền bạc truy cập online. Ở các bưu điện cũng có dịch vụ thu phí đăng ký xét tuyển nên thí sinh sẽ không phải vất vả nhiều"
                ,"Dự đoán ngưỡng điểm chuẩn vào trường ĐH Công nghiệp thực phẩm TPHCM năm nay sẽ giảm hơn năm ngoái từ 1-2 điểm tùy theo ngành."
                ,"Trong khi đó, TS Trần Đình Lý - Trưởng phòng đào tạo trường ĐH Nông lâm TPHCM, chủ trì cụm thi tỉnh Gia Lai cho rằng với phổ điểm năm nay, khả năng điểm chuẩn của trường sẽ bằng hoặc cao hơn năm ngoái một ít. “Trong 3 năm liên tiếp gần đây, điểm chuẩn của trường luôn năm sau cao hơn năm trước. Một số ngành cao hơn năm trước khoảng 1 điểm”, TS Lý cho hay."
        };
        // TraNT: AutoCreatePost
        public async Task<ActionResult> AutoGeneratePost()
        {
            Int32 numberOfPost = 2000;

            foreach (var str in _strRawDescriptions)
            {
                _arrRawDesciptions.Add(str);
            }

            CreatePostViewModel p = new CreatePostViewModel();
            p.Title = "0";
            p.Description = "Description";
            ViewData.Clear();
            p.UserName = User.Identity.Name;
            var tagList = AddAndGetAddedTags(Request, TagManager, "TagsInput");
            var illList = GetAddedImage(Request, "Img", "Des");
            if (tagList != null)
            {
                p.Tags = tagList;
            }

            if (illList != null)
            {
                p.Illustrations = illList;
            }

            for (int i = numberOfPost; i > 0; i--)
            {
                Random r = new Random();
                Int32 rIndex = r.Next(0, Int32.MaxValue);
                rIndex = rIndex % _arrRawDesciptions.Count;
                String strDescriptions = _arrRawDesciptions[rIndex].ToString();

                p.Title = i.ToString();
                p.Description = strDescriptions;

                var post = new Post(p)
                {
                    PublishDate = System.DateTime.Now,
                    UserId = User.Identity.GetUserId()
                };

                await PostManager.CreatePostAsync(post);
            }

            //CreatedPost = true;
            //PostView = new PostViewModel(post);
            //return RedirectToAction("ShowPostDetail", "Post", new { postId = post.Id });
            return null;
        }
        #endregion
        // End comment TraNT: AutoCreatePost
    }
}
