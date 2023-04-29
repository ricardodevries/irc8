﻿using Irc.Commands;
using Irc.Enumerations;

namespace Irc.Extensions.Commands;

internal class Create : Command, ICommand
{
    public Create() : base(0) { }
    public new EnumCommandDataType GetDataType() => EnumCommandDataType.None;

    public new void Execute(ChatFrame chatFrame)
    {
        chatFrame.User.Send(Raw.IRCX_ERR_NOTIMPLEMENTED(chatFrame.Server, chatFrame.User, nameof(Access)));
    }
}