﻿using Irc.Objects;

namespace Irc.Commands;

public class Privmsg : Command
{
    public Privmsg() : base(2)
    {
    }

    public override void Execute(ChatFrame chatFrame)
    {
        SendMessage(chatFrame, false);
    }

    public static void SendMessage(ChatFrame chatFrame, bool Notice)
    {
        var targetName = chatFrame.Message.Parameters.First();
        var message = chatFrame.Message.Parameters[1];

        string?[] targets = targetName.Split(',', StringSplitOptions.RemoveEmptyEntries);
        foreach (var target in targets)
        {
            ChatObject chatObject = null;
            if (Channel.ValidName(target))
                chatObject = chatFrame.Server.GetChannelByName(target);
            else
                chatObject = (ChatObject)chatFrame.Server.GetUserByNickname(target);

            if (chatObject == null)
            {
                // TODO: To make common function for this
                chatFrame.User.Send(Raw.IRCX_ERR_NOSUCHCHANNEL_403(chatFrame.Server, chatFrame.User, target));
                return;
            }

            if (chatObject is Channel)
            {
                var channel = (Channel)chatObject;
                var channelMember = channel.GetMember(chatFrame.User);
                var isOnChannel = channelMember != null;
                var noExtern = channel.NoExtern;
                var moderated = channel.Moderated;

                if (
                    // No External Messages
                    (!isOnChannel && noExtern) ||
                    // Moderated
                    (isOnChannel && moderated && channelMember.IsNormal())
                )
                {
                    chatFrame.User.Send(
                        Raw.IRCX_ERR_CANNOTSENDTOCHAN_404(chatFrame.Server, chatFrame.User, channel));
                    return;
                }

                if (Notice) ((Channel)chatObject).SendNotice(chatFrame.User, message);
                else ((Channel)chatObject).SendMessage(chatFrame.User, message);
            }
            else if (chatObject is User)
            {
                if (Notice)
                    ((User)chatObject).Send(
                        Raw.RPL_NOTICE_USER(chatFrame.Server, chatFrame.User, chatObject, message)
                    );
                else
                    ((User)chatObject).Send(
                        Raw.RPL_PRIVMSG_USER(chatFrame.Server, chatFrame.User, chatObject, message)
                    );
            }
        }
    }
}