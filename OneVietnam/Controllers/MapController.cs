﻿using Microsoft.AspNet.Identity.Owin;
using OneVietnam.BLL;
using OneVietnam.DTL;
using OneVietnam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OneVietnam.Controllers
{
    public class MapController : Controller
    {
        public static AddLocationViewModel LocationView;               

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

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<ActionResult> AddLocation(AddLocationViewModel l)
        //{
        //    Location location = new Location(l);
        //    await UserManager.AddLocationAsync(l.userid,location);
        //    return RedirectToAction("Index", "Home");
        //}

        public async Task<ActionResult> ShowMap()
        {
            var userslist = await UserManager.AllUsersAsync();
            var list = userslist.Select(user => new AddLocationViewModel
            {
                X = user.Location.XCoordinate, Y = user.Location.YCoordinate, UserId = user.Id, Gender = user.Gender
            }).ToList();
            var postlist = await PostManager.FindAllAsync(false);
            list.AddRange(postlist.Select(p => new AddLocationViewModel
            {
                UserId = p.UserId, X = p.PostLocation.XCoordinate, Y = p.PostLocation.YCoordinate, PostId = p.Id, PostType = p.PostType
            }));
            ViewBag.top5PostModel = await GetTop5PostInfo();

            return View(list);
        }
   

        //[HttpPost] // can be HttpGet
        public async Task<ActionResult> GetUserInfo(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            return Json(user, JsonRequestBehavior.AllowGet);
            //var user = await UserManager.FindByIdAsync(userId);
            //var result = new UserViewModel(user);
            //return PartialView("_UserModal",result);
        }

        public async Task<ActionResult> GetPostInfo(string postId)
        {
            var post = await PostManager.FindByIdAsync(postId);
            var user = await UserManager.FindByIdAsync(post.UserId);
            var result = new PostInfoWindowModel();
            result.UserId = user.Id;
            result.postId = postId;
            result.Title = post.Title;
            result.PublishDate = (DateTimeOffset)post.PublishDate;
            result.Address = post.PostLocation.Address;
            ViewBag.PostInfo = result;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> CustomInfoWindow(string postId)
        {
            var post = await PostManager.FindByIdAsync(postId);
            var user = await UserManager.FindByIdAsync(post.UserId);
            var result = new PostInfoWindowModel();
            result.UserId = user.Id;
            result.postId = postId;
            result.Title = post.Title;
            result.PublishDate = (DateTimeOffset)post.PublishDate;
            result.Address = post.PostLocation.Address;
            ViewBag.PostInfo = result;
            return View();
        }

        public async Task<ActionResult> UserAndPostInfo(string postId)
        {
            //var post = await PostManager.FindById(postId);
            //var user = await UserManager.FindByIdAsync(post.UserId);
            //var result = new PostInfoWindowModel();
            //result.UserId = user.Id;
            //result.UserName = user.UserName;
            //result.Title = post.Title;
            //result.PublishDate = (DateTimeOffset)post.PublishDate;
            //result.Address = post.PostLocation.Address;
            //ViewBag.PostInfo = result;
            var post = await PostManager.FindByIdAsync(postId);
            var result = new PostViewModel(post);
            //return PartialView("_ShowPostDetail",result); ;
            return PartialView("../Post/_ShowPostDetail", result);
        }
        

        public async Task<List<PostInfoWindowModel>> GetTop5PostInfo()
        {
            var baseFilter = new BaseFilter {Limit = 5};
            var top5PostList = await PostManager.FindAllAsync(baseFilter);
            var result = new PostInfoWindowModel();
            List<PostInfoWindowModel> top5PostModel = new List<PostInfoWindowModel>();
            PostInfoWindowModel postModel;
            foreach (Post p in top5PostList)
            {
                postModel = new PostInfoWindowModel();
                postModel.Address = p.PostLocation.Address;
                postModel.postId = p.Id;
                postModel.PublishDate = (DateTimeOffset)p.PublishDate;
                postModel.Title = p.Title;
                top5PostModel.Add(postModel);
            }

            return top5PostModel;
        }
    }
}