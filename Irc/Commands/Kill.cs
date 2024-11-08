using Irc.Enumerations;

namespace Irc.Commands;

internal class Kill : Command
{
    public Kill() : base(2)
    {
    }

    public override void Execute(ChatFrame chatFrame)
    {
        var server = chatFrame.Server;
        var user = chatFrame.User;
        var target = chatFrame.Message.Parameters.First();
        var reason = chatFrame.Message.Parameters[1];

        if (user.Level < EnumUserAccessLevel.Sysop)
        {
            chatFrame.User.Send(Raw.IRCX_ERR_SECURITY_908(server, user));
            return;
        }

        var channels = user.Channels;
        if (channels.Count > 0)
        {
            var channel = channels.First().Key;
            var member = channel.GetMemberByNickname(target);

            if (member == null)
            {
                chatFrame.User.Send(Raw.IRCX_ERR_NOSUCHNICK_401(server, user, target));
                return;
            }

            var targetUser = member.GetUser();

            if (targetUser.Level > user.Level)
            {
                chatFrame.User.Send(Raw.IRCX_ERR_SECURITY_908(server, user));
                return;
            }

            targetUser.RemoveChannel(channel);
            channel.GetMembers().Remove(member);
            channel.Send(Raw.RPL_KILL_IRC(user, targetUser, reason));
            targetUser.Disconnect(
                Raw.IRCX_CLOSINGLINK_007_SYSTEMKILL(server, targetUser, targetUser.Address.RemoteIP));
        }
    }
}