﻿using Irc.Enumerations;
using Irc.Interfaces;
using Irc.Resources;

namespace Irc.Commands;

internal class Ircx : Command, ICommand
{
    public Ircx() : base(0, false)
    {
    }

    public new EnumCommandDataType GetDataType()
    {
        return EnumCommandDataType.None;
    }

    public new void Execute(IChatFrame chatFrame)
    {
        var protocol = chatFrame.User.Protocol.Ircvers;
        if (protocol < EnumProtocolType.IRCX)
        {
            protocol = EnumProtocolType.IRCX;
        }

        var isircx = protocol > EnumProtocolType.IRC;
        chatFrame.User.Ircx = true;


        chatFrame.User.Send(Raw.IRCX_RPL_IRCX_800(chatFrame.Server, chatFrame.User, isircx ? 1 : 0, 0,
            chatFrame.Server.MaxInputBytes, IrcStrings.IRCXOptions));
    }
}