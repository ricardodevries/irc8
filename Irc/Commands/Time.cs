﻿namespace Irc.Commands;

public class Time : Command
{
    public override void Execute(ChatFrame chatFrame)
    {
        chatFrame.User.Send(Raw.IRCX_RPL_TIME_391(chatFrame.Server, chatFrame.User));
    }
}