﻿#region Disclaimer/Info

// ////////////////////////////////////////////////////////////////////////////////////////////////
// File:			ItemTrackbacks.cs
// Website:		http://dexterblogengine.com/
// Authors:		http://dexterblogengine.com/About.ashx
// Created:		2012/11/11
// Last edit:	2012/11/11
// License:		GNU Library General Public License (LGPL)
// For updated news and information please visit http://dexterblogengine.com/
// Dexter is hosted to Github at https://github.com/imperugo/Dexter-Blog-Engine
// For any question contact info@dexterblogengine.com
// ////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

namespace Dexter.Data.Raven.Domain
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Dexter.Entities;

	using global::Raven.Imports.Newtonsoft.Json;

	public class ItemTrackbacks : EntityBase<string>
	{
		#region Constructors and Destructors

		public ItemTrackbacks()
		{
			this.Approved = new List<Trackback>();
			this.Pending = new List<Trackback>();
			this.Spam = new List<Trackback>();
		}

		#endregion

		#region Public Properties

		public List<Trackback> Approved { get; set; }

		public ItemReference Item { get; set; }

		public List<Trackback> Pending { get; set; }

		public List<Trackback> Spam { get; set; }

		[JsonIgnore]
		public int TotalApprovedComments
		{
			get
			{
				return this.Approved.Count;
			}
		}

		[JsonIgnore]
		public int TotalPendingComments
		{
			get
			{
				return this.Pending.Count;
			}
		}

		[JsonIgnore]
		public int TotalSpamComments
		{
			get
			{
				return this.Spam.Count;
			}
		}

		#endregion

		public void AddComment(Trackback item, TrackbackStatus status)
		{
			switch (status)
			{
				case TrackbackStatus.Pending:
					{
						this.Pending.Add(item);
						break;
					}

				case TrackbackStatus.IsSpam:
					{
						this.Spam.Add(item);
						break;
					}

				case TrackbackStatus.IsApproved:
					{
						this.Approved.Add(item);
						break;
					}

				default:
					{
						throw new ArgumentException("Unable to add a trackback for the specified status", "status");
					}
			}
		}

		public void Delete(int trackbackId, TrackbackStatus status)
		{
			Trackback itemToRemove;

			switch (status)
			{
				case TrackbackStatus.Pending:
					{
						itemToRemove = this.Pending.SingleOrDefault(x => x.Id == trackbackId);
						this.Pending.Remove(itemToRemove);
						break;
					}

				case TrackbackStatus.IsSpam:
					{
						itemToRemove = this.Spam.SingleOrDefault(x => x.Id == trackbackId);
						this.Spam.Remove(itemToRemove);
						break;
					}

				case TrackbackStatus.IsApproved:
					{
						itemToRemove = this.Approved.SingleOrDefault(x => x.Id == trackbackId);
						this.Approved.Remove(itemToRemove);
						break;
					}

				default:
					{
						throw new ArgumentException("Unable to find a trackback with the specified id", "trackbackId");
					}
			}
		}
	}
}