﻿using Lamb.UI.FollowerInteractionWheel;
using System;
using System.Collections.Generic;

namespace COTL_API.CustomFollowerCommand
{
    public abstract class CustomFollowerCommandItem : CommandItem
    {
        public virtual string InternalName { get; set; }
        public string ModPrefix;

        public virtual List<CommandItem> GetSubCommands()
        {
            return new List<CommandItem>();
        }

        public virtual bool CheckSelectionPreconditions(Follower follower)
        {
            return true;
        }

        public abstract bool Execute(interaction_FollowerInteraction interaction, FollowerCommands finalCommand = FollowerCommands.None);
    }
}