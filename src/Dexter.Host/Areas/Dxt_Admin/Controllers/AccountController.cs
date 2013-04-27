﻿#region Disclaimer/Info

// ////////////////////////////////////////////////////////////////////////////////////////////////
// File:			AccountController.cs
// Website:		http://dexterblogengine.com/
// Authors:		http://dexterblogengine.com/aboutus
// Created:		2013/04/26
// Last edit:	2013/04/27
// License:		New BSD License (BSD)
// For updated news and information please visit http://dexterblogengine.com/
// Dexter is hosted to Github at https://github.com/imperugo/Dexter-Blog-Engine
// For any question contact info@dexterblogengine.com
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

namespace Dexter.Host.Areas.Dxt_Admin.Controllers
{
	using System.Linq;
	using System.Web.Mvc;
	using System.Web.Security;

	using AutoMapper;

	using Common.Logging;

	using Dexter.Dependency.Extensions;
	using Dexter.Entities.Extensions;
	using Dexter.Host.Areas.Dxt_Admin.Models.Account;
	using Dexter.Services;
	using Dexter.Shared;
	using Dexter.Web.Core.Controllers.Web;

	[Authorize(Roles = Constants.AdministratorRole)]
	public class AccountController : DexterControllerBase
	{
		#region Constructors and Destructors

		public AccountController(ILog logger, IConfigurationService configurationService)
			: base(logger, configurationService)
		{
		}

		#endregion

		#region Public Methods and Operators

		public ActionResult Index(int pageIndex = 0, int pageSize = 30)
		{
			int numberOfUsers;
			MembershipUserCollection users = Membership.GetAllUsers(pageIndex, pageSize, out numberOfUsers);

			IndexViewModel model = new IndexViewModel();
			model.Users = users.Cast<MembershipUser>()
			                   .AsEnumerable()
			                   .MapTo<User>()
			                   .ToPagedResult(pageIndex, pageSize, numberOfUsers);

			model.Users.Result.ForEach(u => u.Roles = Roles.GetRolesForUser(u.Username));

			return this.View(model);
		}

		#endregion
	}
}